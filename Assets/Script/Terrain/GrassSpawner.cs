using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GrassSpawner : MonoBehaviour
{
    [Header("Area Plane")]
    [Tooltip("Plane tempat rumput di-spawn (otomatis detect jika kosong)")]
    public MeshRenderer targetPlane;

    [Header("Grass Mesh & Material")]
    public Mesh      grassMesh;
    public Material  grassMaterial;

    [Header("Area Exclusion (Area Anti-Rumput)")]
    [Tooltip("Pilih layer untuk objek yang menolak rumput (misal: Rumah, Jalan)")]
    public LayerMask obstacleLayer;
    [Range(0.1f, 2f)]
    [Tooltip("Seberapa sensitif jarak rumput dengan obstacle")]
    public float obstacleCheckRadius = 0.2f;

    [Header("Jumlah & Distribusi")]
    [Range(10, 100000)]
    public int grassCount = 2000;
    [Range(0.01f, 5f)]
    public float minDistance = 0.3f;
    public bool usePoissonDistribution = false;

    [Header("Ukuran Rumput")]
    public Vector3 baseScale = new Vector3(0.2f, 0.4f, 0.2f);
    [Range(0f, 1f)]
    public float scaleVariation = 0.3f;

    [Header("Rotasi")]
    public bool randomRotationY = true;
    [Range(0f, 45f)]
    public float tiltAngle = 8f;

    [Header("Optimisasi — Area Kecil")]
    [Range(1f, 200f)]
    public float renderDistance = 12f;
    public bool useFrustumCulling = true;
    [Range(1, 20)]
    public int chunkCount = 4;

    [Header("Angin")]
    public bool enableWind = true;
    [Range(0f, 5f)]  public float windSpeed     = 1.2f;
    [Range(0f, 2f)]  public float windStrength  = 0.4f;
    [Range(0f, 5f)]  public float windFrequency = 1.0f;
    [Range(0f, 1f)]  public float windTurbulence = 0.15f;
    public Vector3 windDirection = new Vector3(1f, 0f, 0.3f);

    private const int BATCH_SIZE = 1023;
    private List<Matrix4x4[]> batches    = new List<Matrix4x4[]>();
    private List<int>         batchCounts = new List<int>();
    private Bounds            planeBounds;
    private Camera            mainCamera;

    private List<List<Matrix4x4>> chunkData   = new List<List<Matrix4x4>>();
    private List<Bounds>          chunkBounds = new List<Bounds>();

    [Header("Debug")]
    public bool showGizmos = true;
    public bool showChunks = false;

    void OnEnable()  => GenerateGrass();

    void Start()
    {
    mainCamera = Camera.main;
    if (mainCamera == null) mainCamera = FindAnyObjectByType<Camera>();
    }

    void Update()
    {
        if (grassMesh == null || grassMaterial == null) return;
        if (enableWind) UpdateWind();
        RenderGrass();
    }

    void UpdateWind()
    {
        float t = Time.time;
        Vector3 dir = windDirection.normalized;

        float turbX = Mathf.Sin(t * 0.37f) * windTurbulence;
        float turbZ = Mathf.Cos(t * 0.29f) * windTurbulence;

        Vector4 windVec = new Vector4(dir.x + turbX, dir.y, dir.z + turbZ, t * windSpeed);

        Shader.SetGlobalVector("_WindDir",       windVec);
        Shader.SetGlobalFloat ("_WindStrength",  windStrength + Mathf.Sin(t * 0.5f) * windTurbulence * 0.5f);
        Shader.SetGlobalFloat ("_WindFrequency", windFrequency);
    }

    [ContextMenu("Generate Grass")]
    public void GenerateGrass()
    {
        batches.Clear();
        batchCounts.Clear();
        chunkData.Clear();
        chunkBounds.Clear();

        if (grassMesh == null || grassMaterial == null) return;

        if (targetPlane != null)
            planeBounds = targetPlane.bounds;
        else if (TryGetComponent<MeshRenderer>(out var mr))
            planeBounds = mr.bounds;
        else return;

        InitChunks();

        List<Vector3> positions = new List<Vector3>();

        if (usePoissonDistribution)
        {
            // Ambil lebih banyak titik (x2) untuk cadangan jika banyak yang menabrak rumah
            var rawPositions = PoissonDiskSampling.Generate(planeBounds, minDistance, grassCount * 2);
            foreach (var pos in rawPositions)
            {
                if (positions.Count >= grassCount) break;
                if (IsPositionValid(pos)) positions.Add(pos);
            }
        }
        else
        {
            float y = planeBounds.max.y;
            int maxAttempts = grassCount * 5; 
            int attempts = 0;

            while (positions.Count < grassCount && attempts < maxAttempts)
            {
                attempts++;
                Vector3 pt = new Vector3(
                    Random.Range(planeBounds.min.x, planeBounds.max.x),
                    y,
                    Random.Range(planeBounds.min.z, planeBounds.max.z)
                );

                if (IsPositionValid(pt)) positions.Add(pt);
            }
        }

        List<Matrix4x4> allMat = new List<Matrix4x4>(positions.Count);
        foreach (var pos in positions)
        {
            var mat = BuildMatrix(pos);
            allMat.Add(mat);
            AddToChunk(pos, mat);
        }

        for (int i = 0; i < allMat.Count; i += BATCH_SIZE)
        {
            int cnt = Mathf.Min(BATCH_SIZE, allMat.Count - i);
            var batch = new Matrix4x4[cnt];
            for (int j = 0; j < cnt; j++) batch[j] = allMat[i + j];
            batches.Add(batch);
            batchCounts.Add(cnt);
        }
    }

    // Fungsi pengecekan area eksklusif
    bool IsPositionValid(Vector3 pos)
    {
        if (obstacleLayer != 0)
        {
            // Cek apakah ada collider dari layer obstacle di titik ini
            if (Physics.CheckSphere(pos, obstacleCheckRadius, obstacleLayer))
                return false; // Titik ini terlarang
        }
        return true;
    }

    void InitChunks()
    {
        float xStep = planeBounds.size.x / chunkCount;
        float zStep = planeBounds.size.z / chunkCount;

        for (int x = 0; x < chunkCount; x++)
        for (int z = 0; z < chunkCount; z++)
        {
            float minX = planeBounds.min.x + x * xStep;
            float minZ = planeBounds.min.z + z * zStep;
            var center = new Vector3(minX + xStep * 0.5f, planeBounds.center.y, minZ + zStep * 0.5f);
            var size   = new Vector3(xStep, planeBounds.size.y + 2f, zStep);
            chunkBounds.Add(new Bounds(center, size));
            chunkData.Add(new List<Matrix4x4>());
        }
    }

    void AddToChunk(Vector3 pos, Matrix4x4 mat)
    {
        for (int i = 0; i < chunkBounds.Count; i++)
            if (chunkBounds[i].Contains(pos)) { chunkData[i].Add(mat); return; }
    }

    Matrix4x4 BuildMatrix(Vector3 pos)
    {
        float   s   = 1f + Random.Range(-scaleVariation, scaleVariation);
        float   rotY = randomRotationY ? Random.Range(0f, 360f) : 0f;
        float   rotX = Random.Range(-tiltAngle, tiltAngle);
        float   rotZ = Random.Range(-tiltAngle, tiltAngle);
        return Matrix4x4.TRS(pos, Quaternion.Euler(rotX, rotY, rotZ), baseScale * s);
    }

    void RenderGrass()
    {
        if (useFrustumCulling && mainCamera != null) RenderWithCulling();
        else RenderAll();
    }

    void RenderAll()
    {
        for (int i = 0; i < batches.Count; i++) DrawBatch(batches[i], batchCounts[i]);
    }

    void RenderWithCulling()
    {
        Plane[] frustum = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        Vector3 camPos  = mainCamera.transform.position;
        float   sqrDist = renderDistance * renderDistance;
        var visible = new List<Matrix4x4>(1024);

        for (int i = 0; i < chunkBounds.Count; i++)
        {
            if ((chunkBounds[i].center - camPos).sqrMagnitude > sqrDist) continue;
            if (!GeometryUtility.TestPlanesAABB(frustum, chunkBounds[i])) continue;
            visible.AddRange(chunkData[i]);
        }

        for (int i = 0; i < visible.Count; i += BATCH_SIZE)
        {
            int cnt   = Mathf.Min(BATCH_SIZE, visible.Count - i);
            var batch = new Matrix4x4[cnt];
            for (int j = 0; j < cnt; j++) batch[j] = visible[i + j];
            DrawBatch(batch, cnt);
        }
    }

    void DrawBatch(Matrix4x4[] batch, int count)
    {
        Graphics.DrawMeshInstanced(grassMesh, 0, grassMaterial, batch, count, null, UnityEngine.Rendering.ShadowCastingMode.Off, false);
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Gizmos.DrawWireCube(planeBounds.center, planeBounds.size);
        if (mainCamera != null)
        {
            Gizmos.color = new Color(1f, 1f, 0f, 0.15f);
            Gizmos.DrawWireSphere(mainCamera.transform.position, renderDistance);
        }
        if (showChunks)
        {
            Gizmos.color = new Color(0f, 0.8f, 1f, 0.1f);
            foreach (var cb in chunkBounds) Gizmos.DrawWireCube(cb.center, cb.size);
        }
    }

    public void Refresh() => GenerateGrass();
}