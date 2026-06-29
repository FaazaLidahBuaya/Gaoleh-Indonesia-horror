using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootstep : MonoBehaviour
{
    [Header("Suara Langkah (SFX)")]
    public AudioClip sfxKiri;
    public AudioClip sfxKanan;

    [Header("Pengaturan Jeda")]
    [Tooltip("Waktu jeda antar langkah kaki (sesuaikan dengan kecepatan jalanmu)")]
    public float jarakAntarLangkah = 0.5f; 
    
    [Tooltip("Batas minimal pergerakan agar suara terpicu")]
    public float kecepatanMinimal = 0.1f;

    private AudioSource audioSource;
    private CharacterController charController;
    private Rigidbody rb;
    
    private float timerLangkah;
    private bool giliranKakiKiri = true; // Sakelar penentu giliran kaki
    private Vector3 posisiTerakhir;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        charController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        
        posisiTerakhir = transform.position;
    }

    void Update()
    {
        bool sedangJalan = false;

        // Cek kecepatan pergerakan (Mendukung CharacterController maupun Rigidbody)
        if (charController != null)
        {
            if (charController.velocity.magnitude > kecepatanMinimal && charController.isGrounded)
            {
                sedangJalan = true;
            }
        }
        else if (rb != null)
        {
            if (rb.linearVelocity.magnitude > kecepatanMinimal)
            {
                sedangJalan = true;
            }
        }
        else 
        {
            // Jika tidak pakai keduanya, cek perubahan posisi manual
            float kecepatanManual = Vector3.Distance(transform.position, posisiTerakhir) / Time.deltaTime;
            if (kecepatanManual > kecepatanMinimal) sedangJalan = true;
        }

        // Jalankan timer langkah jika terdeteksi bergerak
        if (sedangJalan)
        {
            timerLangkah -= Time.deltaTime;
            if (timerLangkah <= 0f)
            {
                MainkanSuaraLangkah();
                timerLangkah = jarakAntarLangkah; // Reset timer
            }
        }
        else
        {
            // Jika pemain berhenti, reset timer agar saat jalan lagi langsung bunyi
            timerLangkah = 0f;
        }

        posisiTerakhir = transform.position;
    }

    private void MainkanSuaraLangkah()
    {
        if (sfxKiri == null || sfxKanan == null) return;

        // Sistem logika IF-ELSE untuk memutar suara bergantian
        if (giliranKakiKiri)
        {
            audioSource.PlayOneShot(sfxKiri);
        }
        else
        {
            audioSource.PlayOneShot(sfxKanan);
        }

        // Tukar sakelar giliran untuk langkah berikutnya (True jadi False, False jadi True)
        giliranKakiKiri = !giliranKakiKiri;
    }
}