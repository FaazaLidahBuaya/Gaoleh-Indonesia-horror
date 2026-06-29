using UnityEngine;
using System.Collections;

public class SutradaraMalam3 : MonoBehaviour
{
    [Header("Mode Aman Untuk Developer")]
    public bool modeTesting = false;

    [Header("Pengaturan Waktu (Detik)")]
    public float waktuMinimal = 240f; 
    public float waktuMaksimal = 360f; 

    [Header("Fase 1: Gelas Jatuh")]
    public Rigidbody gelasDiMeja;
    public Collider colliderGelas; 
    public AudioSource sfxGelasJatuh;
    private Vector3 posisiAwalGelas;
    private Quaternion rotasiAwalGelas;
    private bool gelasSudahJatuh = false;
    private bool gelasSudahDikembalikan = false;

    [Header("Fase 2: Pocong Ngintip & Pintu")]
    public SimpleDoor pintuKamarKakak; 
    public GameObject pocongNgintip;
    public GameObject triggerPocongHilang; 
    public AudioSource sfxPintuKamarBuka; 

    // KUNCI PENGINGAT BARU: Biar hitung mundur cuma dipicu 1 kali pas masuk Malam 3
    private bool eventMalam3SudahDimulai = false;

    void Start()
    {
        Debug.Log("🕵️ DETEKTIF MALAM 3: Script Sutradara Berhasil Berjalan di Start!");

        // Matikan dulu collider & trigger sejak awal game (Malam 1) agar aman dari interaksi iseng
        if (colliderGelas != null) colliderGelas.enabled = false;
        if (triggerPocongHilang != null) triggerPocongHilang.SetActive(false);

        // Selalu catat posisi awal gelas di awal game mumpung posisinya masih rapi di atas meja
        if (gelasDiMeja != null) 
        {
            posisiAwalGelas = gelasDiMeja.transform.position;
            rotasiAwalGelas = gelasDiMeja.transform.rotation;
            gelasDiMeja.isKinematic = true; 
        }
    }

    void Update()
    {
        // PANDANGAN DETEKTIF: Cek setiap frame apakah malam sudah berubah ke Malam 3
        if (!eventMalam3SudahDimulai && NightManager.instance != null && NightManager.instance.malamSekarang == 3)
        {
            eventMalam3SudahDimulai = true; // Kunci dicentang TRUE agar coroutine tidak terpanggil double di frame berikutnya
            
            float waktuTunggu = Random.Range(waktuMinimal, waktuMaksimal);
            if (modeTesting) waktuTunggu = 10f; 
            
            Debug.Log("⏳ DETEKTIF MALAM 3: Game mendeteksi perubahan ke Malam 3! Coroutine dimulai. Gelas akan dijatuhkan dalam " + waktuTunggu + " detik.");
            StartCoroutine(TungguGelasJatuh(waktuTunggu));
        }
    }

    private IEnumerator TungguGelasJatuh(float delay)
    {
        if (delay >= 3f)
        {
            yield return new WaitForSeconds(delay - 3f);
            Debug.Log("🚨 DETEKTIF MALAM 3: SIAP-SIAP! 3 detik lagi gelas bakal jatoh...");
            yield return new WaitForSeconds(3f);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }
        
        if (gelasDiMeja != null)
        {
            Debug.Log("💥 DETEKTIF MALAM 3: PRANGGG!! Gelas didorong jatuh sekarang!");
            if (colliderGelas != null) colliderGelas.enabled = true;

            gelasDiMeja.isKinematic = false;
            gelasDiMeja.AddForce(Vector3.forward * 1.5f, ForceMode.Impulse); 
            if (sfxGelasJatuh != null) sfxGelasJatuh.Play();
            gelasSudahJatuh = true;
        }
    }

    public void KembalikanGelasKeMeja()
    {
        if (gelasSudahJatuh && !gelasSudahDikembalikan)
        {
            gelasSudahDikembalikan = true;
            Debug.Log("🧹 DETEKTIF MALAM 3: Memulai animasi gelas kembali ke meja...");
            StartCoroutine(ProsesAnimasiGelas());
        }
    }

    private IEnumerator ProsesAnimasiGelas()
    {
        // Matikan fisika agar gelas melayang dengan tenang
        if (colliderGelas != null) colliderGelas.enabled = false;
        gelasDiMeja.isKinematic = true;

        Vector3 posisiJatuh = gelasDiMeja.transform.position;
        Quaternion rotasiJatuh = gelasDiMeja.transform.rotation;
        
        float timer = 0f;
        float durasiAnimasi = 0.6f; // Waktu gelas melayang (0.6 detik)

        // Proses Animasi Melayang Mulus (SmoothStep)
        while (timer < durasiAnimasi)
        {
            timer += Time.deltaTime;
            float t = timer / durasiAnimasi;
            t = t * t * (3f - 2f * t); // Rumus SmoothStep biar gerakannya estetik

            gelasDiMeja.transform.position = Vector3.Lerp(posisiJatuh, posisiAwalGelas, t);
            gelasDiMeja.transform.rotation = Quaternion.Slerp(rotasiJatuh, rotasiAwalGelas, t);
            yield return null;
        }

        // Posisikan pas 100% di akhir
        gelasDiMeja.transform.position = posisiAwalGelas;
        gelasDiMeja.transform.rotation = rotasiAwalGelas;

        // Lanjut efek horor aslinya
        if (sfxPintuKamarBuka != null) sfxPintuKamarBuka.Play();
        if (pintuKamarKakak != null) pintuKamarKakak.BukaPaksa();
        if (pocongNgintip != null) pocongNgintip.SetActive(true);
        if (triggerPocongHilang != null) triggerPocongHilang.SetActive(true);
    }
}