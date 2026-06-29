using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))] 
public class SimpleDoor : MonoBehaviour
{
    public enum SumbuPutar { X, Y, Z }

    [Header("Pengaturan Rotasi Pintu")]
    public SumbuPutar sumbuEngsel = SumbuPutar.Y; 
    public float openAngle = 90f;
    public float smoothSpeed = 5f;

    [Header("Pengaturan Suara (SFX)")]
    public AudioClip soundOpen;  
    public AudioClip soundClose; 
    [Tooltip("Suara gagang pintu mentok pas dikunci (Opsional)")]
    public AudioClip soundLocked; 

    [Header("Event Tambahan (Opsional)")]
    [Tooltip("Apa yang terjadi saat pintu ini berhasil DIBUKA?")]
    public UnityEvent saatPintuDibuka;

    private AudioSource audioSource;
    
    [Header("Status Sistem (Bisa Dilihat di Inspector)")]
    public bool isOpen = false;
    // KUNCI UTAMA: Variabel ini yang bakal nahan klik pemain
    public bool terkunci = false; 

    private Quaternion startRotation;
    private Quaternion targetRotation;

    void Start()
    {
        transform.SetParent(null, true);
        audioSource = GetComponent<AudioSource>();
        startRotation = transform.localRotation;
        targetRotation = startRotation;
    }

    void Update()
    {
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * smoothSpeed);
    }

    // Fungsi ini yang dipanggil saat PEMAIN nge-klik pintu
    public void ToggleDoor()
    {
        // 1. JURUS SATPAM: Jika status 'terkunci' aktif, BLOKIR KLIK PEMAIN!
        if (terkunci)
        {
            if (soundLocked != null) PlayDoorSound(soundLocked); // Bunyi ceklek mentok
            Debug.Log("🔒 Pintu terkunci rapat! Pemain tidak bisa membuka.");
            return; // Fungsi langsung berhenti di sini, pintu GAGAL terbuka!
        }

        // Jika tidak terkunci, jalankan sistem buka-tutup normal seperti biasa
        isOpen = !isOpen;

        if (isOpen)
        {
            BukaPintuInti();
            if (saatPintuDibuka != null) saatPintuDibuka.Invoke();
        }
        else
        {
            TutupPintuInti();
        }
    }

    // =========================================================
    // FUNGSI KHUSUS UNTUK SISTEM JUMPSCARE / EVENT (BUKAN PEMAIN)
    // =========================================================

    // Fungsi dibanting tertutup oleh JebakanKamarKakak
    public void TutupPaksa(bool kunciSekalian)
    {
        if (isOpen)
        {
            isOpen = false;
            TutupPintuInti();
        }

        // 2. Setel status kunci berdasarkan keputusan poin sompral dari jebakan
        terkunci = kunciSekalian; 
    }

    // Fungsi dibuka otomatis oleh SutradaraMalam3 setelah gelas jatuh
    public void BukaPaksa()
    {
        // Pastikan gemboknya dilepas dulu secara gaib biar bisa kebuka
        terkunci = false; 

        if (!isOpen)
        {
            isOpen = true;
            BukaPintuInti();
            if (saatPintuDibuka != null) saatPintuDibuka.Invoke();
        }
    }

    // =========================================================
    // LOGIKA INTI PERPUTARAN ENGSEL
    // =========================================================

    private void BukaPintuInti()
    {
        Vector3 arahPutar = Vector3.zero;
        if (sumbuEngsel == SumbuPutar.X) arahPutar = new Vector3(openAngle, 0, 0);
        else if (sumbuEngsel == SumbuPutar.Y) arahPutar = new Vector3(0, openAngle, 0);
        else if (sumbuEngsel == SumbuPutar.Z) arahPutar = new Vector3(0, 0, openAngle);

        targetRotation = startRotation * Quaternion.Euler(arahPutar);
        PlayDoorSound(soundOpen);
    }

    private void TutupPintuInti()
    {
        targetRotation = startRotation;
        PlayDoorSound(soundClose);
    }

    private void PlayDoorSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}