using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GrassSpawner))]
public class GrassSpawnerEditor : Editor
{
    SerializedProperty targetPlane, grassMesh, grassMaterial;
    SerializedProperty obstacleLayer, obstacleCheckRadius; // Tambahan
    SerializedProperty grassCount, minDistance, usePoissonDistribution;
    SerializedProperty baseScale, scaleVariation, randomRotationY, tiltAngle;
    SerializedProperty renderDistance, useFrustumCulling, chunkCount;
    SerializedProperty enableWind, windSpeed, windStrength, windFrequency, windTurbulence, windDirection;
    SerializedProperty showGizmos, showChunks;

    bool foldDist = true, foldSize = true, foldOpt = true, foldWind = true, foldDbg = false;

    void OnEnable()
    {
        targetPlane           = serializedObject.FindProperty("targetPlane");
        grassMesh             = serializedObject.FindProperty("grassMesh");
        grassMaterial         = serializedObject.FindProperty("grassMaterial");
        
        obstacleLayer         = serializedObject.FindProperty("obstacleLayer");
        obstacleCheckRadius   = serializedObject.FindProperty("obstacleCheckRadius");

        grassCount            = serializedObject.FindProperty("grassCount");
        minDistance           = serializedObject.FindProperty("minDistance");
        usePoissonDistribution= serializedObject.FindProperty("usePoissonDistribution");
        baseScale             = serializedObject.FindProperty("baseScale");
        scaleVariation        = serializedObject.FindProperty("scaleVariation");
        randomRotationY       = serializedObject.FindProperty("randomRotationY");
        tiltAngle             = serializedObject.FindProperty("tiltAngle");
        renderDistance        = serializedObject.FindProperty("renderDistance");
        useFrustumCulling     = serializedObject.FindProperty("useFrustumCulling");
        chunkCount            = serializedObject.FindProperty("chunkCount");
        enableWind            = serializedObject.FindProperty("enableWind");
        windSpeed             = serializedObject.FindProperty("windSpeed");
        windStrength          = serializedObject.FindProperty("windStrength");
        windFrequency         = serializedObject.FindProperty("windFrequency");
        windTurbulence        = serializedObject.FindProperty("windTurbulence");
        windDirection         = serializedObject.FindProperty("windDirection");
        showGizmos            = serializedObject.FindProperty("showGizmos");
        showChunks            = serializedObject.FindProperty("showChunks");
    }

    public override void OnInspectorGUI()
    {
        var spawner = (GrassSpawner)target;
        serializedObject.Update();

        EditorGUILayout.Space(4);
        Header("🌿 Grass Spawner");
        EditorGUILayout.Space(6);

        if (spawner.grassMaterial != null && !spawner.grassMaterial.enableInstancing)
            EditorGUILayout.HelpBox("⚠️ GPU Instancing BELUM aktif pada material!", MessageType.Warning);

        SectionLabel("Setup");
        EditorGUILayout.PropertyField(targetPlane,   new GUIContent("Target Plane"));
        EditorGUILayout.PropertyField(grassMesh,     new GUIContent("Grass Mesh"));
        EditorGUILayout.PropertyField(grassMaterial, new GUIContent("Grass Material"));

        EditorGUILayout.Space(6);
        SectionLabel("Area Anti-Rumput");
        EditorGUILayout.PropertyField(obstacleLayer, new GUIContent("Layer Penghalang", "Pilih layer objek yang tidak boleh ada rumputnya"));
        EditorGUILayout.PropertyField(obstacleCheckRadius, new GUIContent("Radius Cek"));
        EditorGUILayout.Space(8);

        foldDist = EditorGUILayout.BeginFoldoutHeaderGroup(foldDist, "Jumlah & Distribusi");
        if (foldDist)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(grassCount,             new GUIContent("Jumlah Rumput"));
            EditorGUILayout.PropertyField(minDistance,            new GUIContent("Jarak Minimum"));
            EditorGUILayout.PropertyField(usePoissonDistribution, new GUIContent("Distribusi Poisson"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        foldSize = EditorGUILayout.BeginFoldoutHeaderGroup(foldSize, "Ukuran & Rotasi");
        if (foldSize)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(baseScale,      new GUIContent("Ukuran Dasar"));
            EditorGUILayout.PropertyField(scaleVariation, new GUIContent("Variasi Ukuran"));
            EditorGUILayout.PropertyField(randomRotationY,new GUIContent("Rotasi Y Acak"));
            EditorGUILayout.PropertyField(tiltAngle,      new GUIContent("Sudut Miring Max (°)"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        foldOpt = EditorGUILayout.BeginFoldoutHeaderGroup(foldOpt, "Optimisasi");
        if (foldOpt)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(renderDistance,   new GUIContent("Render Distance"));
            EditorGUILayout.PropertyField(useFrustumCulling,new GUIContent("Frustum Culling"));
            EditorGUILayout.PropertyField(chunkCount,        new GUIContent("Chunk Count"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        foldWind = EditorGUILayout.BeginFoldoutHeaderGroup(foldWind, "Angin");
        if (foldWind)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(enableWind,      new GUIContent("Aktifkan Angin"));
            if (spawner.enableWind)
            {
                EditorGUILayout.PropertyField(windDirection,  new GUIContent("Arah"));
                EditorGUILayout.PropertyField(windSpeed,      new GUIContent("Kecepatan"));
                EditorGUILayout.PropertyField(windStrength,   new GUIContent("Kekuatan"));
                EditorGUILayout.PropertyField(windFrequency,  new GUIContent("Frekuensi"));
                EditorGUILayout.PropertyField(windTurbulence, new GUIContent("Turbulensi"));
            }
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        foldDbg = EditorGUILayout.BeginFoldoutHeaderGroup(foldDbg, "Debug");
        if (foldDbg)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(showGizmos);
            EditorGUILayout.PropertyField(showChunks);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.Space(12);
        GUI.backgroundColor = new Color(0.35f, 0.8f, 0.35f);
        if (GUILayout.Button("🌱  Generate Rumput", GUILayout.Height(38)))
        {
            spawner.GenerateGrass();
            EditorUtility.SetDirty(spawner);
        }
        GUI.backgroundColor = Color.white;

        serializedObject.ApplyModifiedProperties();
    }

    void Header(string title)
    {
        var s = new GUIStyle(EditorStyles.boldLabel) { fontSize = 14, alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField(title, s, GUILayout.Height(22));
    }

    void SectionLabel(string title) => EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
}
#endif