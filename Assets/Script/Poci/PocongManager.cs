using UnityEngine;
using System.Collections.Generic;

public class PocongManager : MonoBehaviour
{
    // Singleton agar gampang dipanggil dari script lain (seperti TriggerRuangan)
    public static PocongManager instance; 

    [Header("Referensi Utama")]
    public PocongAI pocong;
    public Camera kameraPemain;
    
    [Header("Sistem Sompral (Dinamis)")]
    public int poinSompral = 1; 
    [Tooltip("Target poin maksimal untuk membuat pocong paling agresif")]
    public float targetPoinMaksimal = 50f; 
    
    [Header("Pengaturan Waktu Teror (Detik)")]
    public float waktuTungguTerlama = 40f;  // Dipakai saat poin sompral 0
    public float waktuTungguTercepat = 10f; // Dipakai saat poin sompral 50+

    [Header("Pengaturan Logika Spawn")]
    [Tooltip("Disarankan muncul LEBIH JAUH dari jarak ini agar seram (Meter)")]
    public float jarakMinimalSpawn = 8f; 

    [HideInInspector] 
    public string ruanganPemainSekarang = ""; // Ini akan diisi otomatis oleh TriggerRuangan

    private WaypointPocong[] semuaWaypoint;
    private float timerSpawn;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Cari otomatis semua titik ngintip yang ada di map
        semuaWaypoint = FindObjectsByType<WaypointPocong>(FindObjectsInactive.Exclude);
        
        // Sembunyikan pocong di awal game
        pocong.gameObject.SetActive(false); 
        ResetTimer();
    }

    void Update()
    {
    // TAMBAHAN: Hentikan timer dan cegah pocong spawn jika masih Malam 1
    if (NightManager.instance != null && NightManager.instance.malamSekarang == 1) return;

    // Jika pocong sedang aktif ngintip, berhentikan waktu mundur
    if (pocong.gameObject.activeInHierarchy) return;

    timerSpawn -= Time.deltaTime;
        
    if (timerSpawn <= 0)
    {
        CobaSpawnPocong();
        ResetTimer();
    }
    }

    void ResetTimer()
    {
        // 1. Hitung rasio kesompralan dari 0.0 (0%) sampai 1.0 (100%)
        float rasioSompral = Mathf.Clamp01((float)poinSompral / targetPoinMaksimal);
        
        // 2. Gunakan Lerp untuk mencari waktu yang pas di antara Terlama dan Tercepat
        float waktuTunggu = Mathf.Lerp(waktuTungguTerlama, waktuTungguTercepat, rasioSompral);
        
        timerSpawn = waktuTunggu;
    }

    void CobaSpawnPocong()
    {
        List<WaypointPocong> waypointDiLuarLayar = new List<WaypointPocong>();
        List<WaypointPocong> waypointSangatAman = new List<WaypointPocong>();

        foreach (WaypointPocong wp in semuaWaypoint)
        {
            // 1. Cek apakah Waypoint bisa dipakai (Logika pintu tertutup)
            if (!wp.ApakahBisaDipakai()) continue;

            // 2. [SISTEM RUANGAN]: Jangan spawn kalau ruangannya SAMA dengan pemain!
            if (!string.IsNullOrEmpty(ruanganPemainSekarang) && wp.namaRuangan == ruanganPemainSekarang) 
            {
                continue; // Skip Waypoint ini, cari yang lain
            }

            // 3. Cek posisi di layar kamera (Blind spot check)
            Vector3 posisiLayar = kameraPemain.WorldToViewportPoint(wp.transform.position);
            bool diLuarLayar = posisiLayar.x < -0.2f || posisiLayar.x > 1.2f || posisiLayar.y < -0.2f || posisiLayar.y > 1.2f || posisiLayar.z < 0;

            if (diLuarLayar)
            {
                // Masukkan ke daftar kandidat dasar (Yang penting tidak terlihat)
                waypointDiLuarLayar.Add(wp);

                // 4. Cek Jarak Ideal: Apakah jaraknya cukup jauh?
                float jarakPemain = Vector3.Distance(wp.transform.position, kameraPemain.transform.position);
                if (jarakPemain >= jarakMinimalSpawn) 
                {
                    waypointSangatAman.Add(wp);
                }
            }
        }

        // --- SISTEM PEMILIHAN PINTAR (FALLBACK) ---
        
        // Prioritas 1: Titik di luar layar DAN jaraknya sangat jauh
        if (waypointSangatAman.Count > 0)
        {
            int indexAcak = Random.Range(0, waypointSangatAman.Count);
            pocong.MulaiNgintip(waypointSangatAman[indexAcak]);
        }
        // Prioritas 2 (FALLBACK): Map terlalu sempit!
        // Pilih titik mana saja yang penting di luar layar & beda ruangan.
        else if (waypointDiLuarLayar.Count > 0)
        {
            int indexAcak = Random.Range(0, waypointDiLuarLayar.Count);
            pocong.MulaiNgintip(waypointDiLuarLayar[indexAcak]);
        }
        // Prioritas 3: Pemain lagi muter-muter / semua titik sedang dilihat
        else
        {
            // Tunda spawn sebentar agar pocong tidak memaksa muncul di depan muka
            timerSpawn = 5f; 
        }
    }

    [ContextMenu("TEST: Munculkan Pocong Sekarang")]
    void TestSpawnPocong()
    {
        CobaSpawnPocong();
        Debug.Log("Mode Test: Mencari posisi aman untuk Pocong...");
    }
}