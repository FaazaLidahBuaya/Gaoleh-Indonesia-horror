using UnityEngine;
using System.Collections;

public class TriggerKebunBelakang : MonoBehaviour
{
    [Header("Mode Testing (Khusus Developer)")]
    [Tooltip("Centang ini jika ingin mengetes jumpscare tanpa harus mengumpulkan poin asli")]
    public bool modeTesting = false;
    [Tooltip("Masukkan poin buatan (Misal: 5 untuk ngintip, 20 untuk biasa, 40 untuk lari)")]
    public int poinSompralBuatan = 0;

    [Header("Batas Poin Sompral")]
    public int batasNgintip = 10;
    public int batasJumpscareBerat = 35;

    [Header("Pengaturan Waktu Jeda")]
    [Tooltip("Jeda acak (detik) sebelum pocong muncul setelah pemain menginjak trigger")]
    public float jedaJumpscareMin = 2f;
    public float jedaJumpscareMax = 5f;

    [Header("Referensi Objek")]
    public Transform kameraPlayer;
    public GameObject pocongNgintip;
    public GameObject pocongJumpscare;
    
    [Tooltip("Titik awal pocong lari dari kejauhan (Khusus Jumpscare Berat)")]
    public Transform titikSpawnLari;

    [Header("Pengaturan Tampilan Pocong")]
    [Tooltip("Koreksi tinggi agar wajah pocong pas di depan kamera")]
    public float koreksiTinggiPocong = 1.6f;
    [Tooltip("Jika pocong malah menghadap samping/belakang, ubah nilai Y di sini (misal: 90, 180, atau -90)")]
    public Vector3 koreksiRotasiPocong = new Vector3(0, 0, 0);

    [Header("Pengaturan Efek Kamera (Jumpscare Muka)")]
    [Tooltip("Seberapa kuat layar bergoyang?")]
    public float intensitasGetaran = 0.02f;
    [Tooltip("Seberapa cepat goyangannya?")]
    public float kecepatanGetaran = 25f;

    [Header("Audio (SFX)")]
    public AudioClip sfxNgintip;
    public AudioClip sfxJumpscareBiasa;
    public AudioClip sfxJumpscareBerat;

    private bool sudahTerpicu = false;

    private void OnTriggerEnter(Collider other)
    {
        if (sudahTerpicu) return;

        if (other.CompareTag("Player"))
        {
            // Trigger langsung dimatikan agar tidak terpanggil dua kali
            // meskipun jumpscare-nya belum muncul
            sudahTerpicu = true;
            
            int poinSekarang = 0;
            
            // LOGIKA MODE TESTING
            if (modeTesting)
            {
                poinSekarang = poinSompralBuatan;
                Debug.Log($"[MODE TESTING KEBUN] Memaksa Poin Sompral menjadi: {poinSekarang}");
            }
            else if (SompralManager.instance != null)
            {
                poinSekarang = SompralManager.instance.totalPoinSompral;
                Debug.Log($"[KEBUN BELAKANG] Pemain masuk. Poin Asli: {poinSekarang}");
            }

            // Tentukan jenis jumpscare berdasarkan poin sompral
            if (poinSekarang < batasNgintip)
            {
                StartCoroutine(EksekusiNgintip());
            }
            else if (poinSekarang < batasJumpscareBerat)
            {
                StartCoroutine(EksekusiJumpscareBiasa());
            }
            else
            {
                StartCoroutine(EksekusiJumpscareBerat());
            }
        }
    }

    // =========================================================
    // LEVEL 1: POCONG CUMA NGINTIP (< 10 Poin)
    // =========================================================
    private IEnumerator EksekusiNgintip()
    {
        // TUNGGU DULU SEBELUM MUNCUL
        float jedaAcak = Random.Range(jedaJumpscareMin, jedaJumpscareMax);
        yield return new WaitForSeconds(jedaAcak);

        if (pocongNgintip != null)
        {
            pocongNgintip.SetActive(true);
            
            if (sfxNgintip != null) 
                AudioSource.PlayClipAtPoint(sfxNgintip, pocongNgintip.transform.position);

            yield return new WaitForSeconds(2.5f);
            
            pocongNgintip.SetActive(false);
        }
    }

    // =========================================================
    // LEVEL 2: JUMPSCARE MUKA & KAMERA NUNDUK GEMETAR
    // =========================================================
    private IEnumerator EksekusiJumpscareBiasa()
    {
        // 1. TUNGGU DULU SEBELUM MUNCUL (Pemain masih bebas bergerak di fase ini)
        float jedaAcak = Random.Range(jedaJumpscareMin, jedaJumpscareMax);
        yield return new WaitForSeconds(jedaAcak);

        // 2. WAKTU HABIS! BARU KUNCI PEMAIN DAN JUMPSCARE
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) { player.canMove = false; player.enabled = false; }

        if (pocongJumpscare != null && kameraPlayer != null)
        {
            Vector3 posisiMuka = kameraPlayer.position + kameraPlayer.forward * 1.2f;
            posisiMuka.y -= koreksiTinggiPocong; 
            pocongJumpscare.transform.position = posisiMuka;
            
            // Perbaikan Arah Hadap Pocong
            Vector3 arahTatapan = kameraPlayer.position - pocongJumpscare.transform.position;
            arahTatapan.y = 0; 
            if (arahTatapan != Vector3.zero)
            {
                Quaternion rotasiDasar = Quaternion.LookRotation(arahTatapan);
                pocongJumpscare.transform.rotation = rotasiDasar * Quaternion.Euler(koreksiRotasiPocong);
            }
            
            pocongJumpscare.SetActive(true);
        }

        if (sfxJumpscareBiasa != null && kameraPlayer != null)
        {
            AudioSource.PlayClipAtPoint(sfxJumpscareBiasa, kameraPlayer.position);
        }

        // Efek Kamera Nunduk Sambil Gemetar (SMOOTH MENGGUNAKAN PERLIN NOISE)
        float timer = 0f;
        float durasiNunduk = 0.5f;
        
        Vector3 posisiKameraAwal = kameraPlayer.localPosition;
        Quaternion rotasiKameraAwal = kameraPlayer.localRotation;
        Quaternion rotasiNunduk = Quaternion.Euler(60f, 0, 0);

        while (timer < durasiNunduk)
        {
            timer += Time.deltaTime;
            float persentase = timer / durasiNunduk;

            kameraPlayer.localRotation = Quaternion.Slerp(rotasiKameraAwal, rotasiNunduk, persentase);

            // Perlin Noise menghasilkan getaran yang smooth dan tidak kasar
            float shakeX = (Mathf.PerlinNoise(Time.time * kecepatanGetaran, 0f) - 0.5f) * 2f * intensitasGetaran;
            float shakeY = (Mathf.PerlinNoise(0f, Time.time * kecepatanGetaran) - 0.5f) * 2f * intensitasGetaran;
            
            kameraPlayer.localPosition = posisiKameraAwal + new Vector3(shakeX, shakeY, 0);

            yield return null;
        }

        kameraPlayer.localPosition = posisiKameraAwal;
        kameraPlayer.localRotation = rotasiNunduk;

        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            NightManager.instance.panelTransisiHitam.gameObject.SetActive(true);
            NightManager.instance.panelTransisiHitam.alpha = 1f;
        }
        
        if (pocongJumpscare != null) pocongJumpscare.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            float fadeTimer = 0f;
            while (fadeTimer < 1f)
            {
                fadeTimer += Time.deltaTime;
                NightManager.instance.panelTransisiHitam.alpha = 1f - fadeTimer;
                yield return null;
            }
            NightManager.instance.panelTransisiHitam.gameObject.SetActive(false);
        }

        if (player != null) { player.enabled = true; player.canMove = true; }
    }

    // =========================================================
    // LEVEL 3: JUMPSCARE BERAT POCONG LARI 
    // =========================================================
    private IEnumerator EksekusiJumpscareBerat()
    {
        // 1. TUNGGU DULU SEBELUM MUNCUL (Pemain masih bebas bergerak mencari jalan)
        float jedaAcak = Random.Range(jedaJumpscareMin, jedaJumpscareMax);
        yield return new WaitForSeconds(jedaAcak);

        if (pocongJumpscare != null && titikSpawnLari != null && kameraPlayer != null)
        {
            pocongJumpscare.transform.position = titikSpawnLari.position;
            pocongJumpscare.SetActive(true);

            if (sfxJumpscareBerat != null)
                AudioSource.PlayClipAtPoint(sfxJumpscareBerat, titikSpawnLari.position);

            Vector3 posisiAwal = pocongJumpscare.transform.position;

            float timerLari = 0f;
            float durasiLari = 0.6f; 

            while (timerLari < durasiLari)
            {
                timerLari += Time.deltaTime;
                
                // Perbaikan Arah Hadap Pocong saat mengejar
                Vector3 arahTatapan = kameraPlayer.position - pocongJumpscare.transform.position;
                arahTatapan.y = 0; 
                if (arahTatapan != Vector3.zero) 
                {
                    Quaternion rotasiDasar = Quaternion.LookRotation(arahTatapan);
                    pocongJumpscare.transform.rotation = rotasiDasar * Quaternion.Euler(koreksiRotasiPocong);
                }

                Vector3 posisiTarget = kameraPlayer.position + kameraPlayer.forward * 0.8f;
                posisiTarget.y -= koreksiTinggiPocong; 

                pocongJumpscare.transform.position = Vector3.Lerp(posisiAwal, posisiTarget, timerLari / durasiLari);
                yield return null;
            }

            if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
            {
                NightManager.instance.panelTransisiHitam.gameObject.SetActive(true);
                NightManager.instance.panelTransisiHitam.alpha = 1f;
            }

            pocongJumpscare.SetActive(false);
            
            yield return new WaitForSeconds(2f);

            if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
            {
                float fadeTimer = 0f;
                while (fadeTimer < 1f)
                {
                    fadeTimer += Time.deltaTime;
                    NightManager.instance.panelTransisiHitam.alpha = 1f - fadeTimer;
                    yield return null;
                }
                NightManager.instance.panelTransisiHitam.gameObject.SetActive(false);
            }
        }
    }
}