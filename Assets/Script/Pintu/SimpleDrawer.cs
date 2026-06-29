using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleDrawer : MonoBehaviour
{
    public enum SumbuGeser { X_Positif, X_Negatif, Y_Positif, Y_Negatif, Z_Positif, Z_Negatif }

    [Header("Pengaturan Tarikan Laci")]
    [Tooltip("Sumbu arah laci ditarik keluar. Biasanya Z atau X.")]
    public SumbuGeser arahBuka = SumbuGeser.Z_Positif; 
    [Tooltip("Seberapa jauh laci akan ditarik keluar (dalam meter)")]
    public float jarakBuka = 0.5f; 
    public float smoothSpeed = 6f;

    [Header("Pengaturan Suara (SFX)")]
    public AudioClip soundOpen;  
    public AudioClip soundClose; 

    private AudioSource audioSource;
    private bool isOpen = false;
    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Simpan posisi asli laci saat game baru mulai
        startLocalPosition = transform.localPosition;
        targetLocalPosition = startLocalPosition;
    }

    void Update()
    {
        // Pindahkan laci secara perlahan (mulus) menggunakan Lerp
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPosition, Time.deltaTime * smoothSpeed);
    }

    // Fungsi ini dipanggil dari event InteractableObject
    public void ToggleDrawer()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            Vector3 arah = Vector3.zero;
            
            // Menentukan arah tarikan berdasarkan opsi di Inspector
            if (arahBuka == SumbuGeser.X_Positif) arah = Vector3.right;       // X
            else if (arahBuka == SumbuGeser.X_Negatif) arah = Vector3.left;   // -X
            else if (arahBuka == SumbuGeser.Y_Positif) arah = Vector3.up;     // Y
            else if (arahBuka == SumbuGeser.Y_Negatif) arah = Vector3.down;   // -Y
            else if (arahBuka == SumbuGeser.Z_Positif) arah = Vector3.forward;// Z
            else if (arahBuka == SumbuGeser.Z_Negatif) arah = Vector3.back;   // -Z

            // Hitung posisi target: Posisi Awal + (Arah x Jarak)
            targetLocalPosition = startLocalPosition + (arah * jarakBuka);
            PlayDrawerSound(soundOpen);
        }
        else
        {
            // Kembalikan ke posisi awal
            targetLocalPosition = startLocalPosition;
            PlayDrawerSound(soundClose);
        }
    }

    private void PlayDrawerSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}