using UnityEngine;
using System.Collections;

public class SyaratSesajenKakak : MonoBehaviour
{
    public enum JenisItemKakak { CatatanPetunjuk, Buku, Kemenyan, BerasKuning, KopiHitam }
    
    [Header("Identitas Barang")]
    public JenisItemKakak jenisItem;
    
    [Header("Pengaturan Teleportasi (Hanya untuk Sesajen)")]
    [Tooltip("Titik berdiri pemain di depan meja ritual")]
    public Transform titikTeleportRitual;
    [Tooltip("Tarik objek Meja Ritual Kakak ke sini")]
    public MejaRitualKakak mejaRitualTujuan;

    [Header("Pengaturan Efek Layar")]
    [Tooltip("Lama waktu layar memudar menjadi gelap / terang (dalam detik)")]
    public float durasiFade = 2.5f; 
    [Tooltip("Lama pemain berada di kegelapan sebelum layar mulai terang lagi (dalam detik)")]
    public float durasiTahanGelap = 1.5f;

    // Dipanggil dari tombol "Baca Catatan" pada kertas petunjuk
    public void BacaCatatan()
    {
        if (SistemBebaskanKakak.instance != null && !SistemBebaskanKakak.instance.catatanDibaca)
        {
            SistemBebaskanKakak.instance.catatanDibaca = true;
            
            PlayerInteract player = FindAnyObjectByType<PlayerInteract>();
            if (player != null) player.MunculkanSubtitleKustom("Ini cara membebaskan kakak...", 5f);

            // ---> INI BAGIAN YANG DIPERBARUI <---
            SyaratSesajenKakak[] semuaBarang = FindObjectsByType<SyaratSesajenKakak>(FindObjectsInactive.Exclude);  
            
            foreach (SyaratSesajenKakak barang in semuaBarang)
            {
                barang.MunculkanTombolAmbil();
            }
        }
    }

    // Menambahkan tombol "Ambil Sesajen" ke InteractableObject secara otomatis
    public void MunculkanTombolAmbil()
    {
        // Jangan tambahkan tombol ini ke kertas petunjuk itu sendiri
        if (jenisItem == JenisItemKakak.CatatanPetunjuk) return;

        InteractableObject io = GetComponent<InteractableObject>();
        if (io != null)
        {
            // Cek dulu, pastikan tombol belum ada agar tidak terbuat ganda
            bool sudahAda = false;
            foreach (var interaksi in io.daftarInteraksi)
            {
                if (interaksi.teksTombol == "Ambil Sesajen") sudahAda = true;
            }

            // Kalau belum ada, buat tombol baru
            if (!sudahAda)
            {
                InteractionOption opsiAmbil = new InteractionOption();
                opsiAmbil.teksTombol = "Ambil Sesajen";
                opsiAmbil.hilangSetelahDiklik = true;
                opsiAmbil.aksi = new UnityEngine.Events.UnityEvent();
                opsiAmbil.aksi.AddListener(CobaAmbilSesajen);
                
                io.daftarInteraksi.Add(opsiAmbil);
            }
        }
    }

    // Dipanggil otomatis dari tombol yang baru dibuat di atas
    public void CobaAmbilSesajen()
    {
        if (SistemBebaskanKakak.instance != null && SistemBebaskanKakak.instance.catatanDibaca)
        {
            StartCoroutine(ProsesAmbilDanTeleport());
        }
    }

    private IEnumerator ProsesAmbilDanTeleport()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        // 1. Tutup UI
        if (interact != null) interact.KlikBatal(); 
        
        // --- JEDA SUPER PENTING ---
        // Tunggu sistem UI selesai memulihkan pergerakan sebelum kita bajak lagi
        yield return new WaitForSeconds(0.1f);

        // 2. Kunci Pergerakan secara Paksa & Brutal
        if (player != null) 
        {
            player.canMove = false;
            player.enabled = false; // Matikan script movement total!
        }
        
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 3. Fade Out 
        CanvasGroup layarHitam = null;
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            layarHitam.blocksRaycasts = true; 

            float t = 0;
            while (t < durasiFade) 
            { 
                t += Time.deltaTime; 
                layarHitam.alpha = t / durasiFade; 
                yield return null; 
            }
            layarHitam.alpha = 1f;
        }

        // 4. Teleportasi Pemain ke Depan Meja
        if (player != null && titikTeleportRitual != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            
            // Pindahkan posisi
            player.transform.position = titikTeleportRitual.position;

            // Ambil rotasi titik teleport
            Vector3 rotasiTeleport = titikTeleportRitual.rotation.eulerAngles;
            
            // Terapkan HANYA sumbu Y (kiri/kanan) ke pemain, paksa X dan Z jadi 0 agar tidak miring/nunduk
            Quaternion rotasiDatar = Quaternion.Euler(0, rotasiTeleport.y, 0);
            player.transform.rotation = rotasiDatar;

            // Terapkan juga ke kamera agar sinkron dan tidak miring
            if (Camera.main != null) Camera.main.transform.rotation = rotasiDatar;
            
            if (cc != null) cc.enabled = true;
        }

        // 5. Lapor ke Meja Ritual
        if (mejaRitualTujuan != null)
        {
            mejaRitualTujuan.OtomatisLetakkanBenda(jenisItem);
        }

        yield return new WaitForSeconds(durasiTahanGelap);

        // 6. Fade In
        if (layarHitam != null)
        {
            float t = 0;
            while (t < durasiFade) 
            { 
                t += Time.deltaTime; 
                layarHitam.alpha = 1f - (t / durasiFade); 
                yield return null; 
            }
            layarHitam.alpha = 0f;
            layarHitam.blocksRaycasts = false; 
            layarHitam.gameObject.SetActive(false);
        }

        // 7. Bebaskan Kembali Pergerakan
        if (player != null) 
        {
            player.enabled = true; // Nyalakan lagi script movement-nya
            player.canMove = true;
        }

        gameObject.SetActive(false);
    }
}