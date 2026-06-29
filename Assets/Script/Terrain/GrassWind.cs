using UnityEngine;

/// <summary>
/// Controller untuk animasi angin pada rumput via Shader Properties.
/// Butuh shader yang support _WindSpeed, _WindStrength, _WindFrequency.
/// Bisa pakai shader custom atau Nature/Soft Occlusion Leaves.
/// </summary>
public class GrassWind : MonoBehaviour
{
    [Header("Parameter Angin")]
    [Range(0f, 5f)] public float windSpeed     = 1.2f;
    [Range(0f, 2f)] public float windStrength  = 0.5f;
    [Range(0f, 5f)] public float windFrequency = 1.0f;
    public Vector3 windDirection = new Vector3(1f, 0f, 0.5f);

    [Header("Noise Turbulensi")]
    [Range(0f, 1f)] public float turbulence = 0.2f;

    void Update()
    {
        // Update global shader properties (berlaku untuk semua material yang pakai property ini)
        float t = Time.time;
        Vector4 windDir = new Vector4(
            windDirection.normalized.x,
            windDirection.normalized.y,
            windDirection.normalized.z,
            t * windSpeed
        );

        Shader.SetGlobalVector("_WindDir", windDir);
        Shader.SetGlobalFloat("_WindSpeed",     windSpeed);
        Shader.SetGlobalFloat("_WindStrength",  windStrength + Mathf.Sin(t * 0.3f) * turbulence);
        Shader.SetGlobalFloat("_WindFrequency", windFrequency);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, windDirection.normalized * 3f);
    }
}
