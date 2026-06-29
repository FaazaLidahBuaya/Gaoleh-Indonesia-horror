using UnityEngine;
using UnityEngine.Events;

public class AksiGerbangKabur : MonoBehaviour
{
    [Header("Syarat Wajib (Biar bisa lewat)")]
    public string namaKunciGerbang = "Kunci Gerbang";
    public string pesanGagalKunci = "Gemboknya masih terkunci. Aku butuh kuncinya.";

    [Header("Pengecekan Dokumen Normal")]
    public string[] daftarSemuaDokumen;

    [Header("CABANG ENDING KABUR BIASA")]
    public UnityEvent eventEnding1_KotorNoDokumen;
    public UnityEvent eventEnding9_Bersih;
    public UnityEvent eventEndingLainnya;

    [Header("CABANG ENDING SOMPRAL")]
    public int batasPoinSompral = 30;
    public UnityEvent eventEnding_SompralKabur;

    [Header("SISTEM CABANG KERIS (Prioritas Utama)")]
    public string namaItemKeris = "Keris";
    public UnityEvent eventEnding_KerisMeninggal;
    public UnityEvent eventEnding_KerisKeberuntungan;
    public UnityEvent eventEnding_KerisKesempatanLain;

    [Header("SISTEM CABANG BUKU KAKAK (Baru)")]
    public string namaItemBuku = "Buku Catatan Kakak";
    [Tooltip("Ending jika pemain membawa kabur buku catatan kakak")]
    public UnityEvent eventEnding_BukuDiikuti;

    private bool sudahKabur = false;

    public void CobaKabur() 
    {
        if (sudahKabur) return;

        // --- 0. CEK KUNCI GERBANG DULU ---
        if (InventorySystem.instance != null && !InventorySystem.instance.CekPunyaBarang(namaKunciGerbang))
        {
            TampilkanPesanGagal();
            return; 
        }

        // --- 1. PENGECEKAN KERIS (Prioritas Mutlak 1) ---
        if (InventorySystem.instance != null && InventorySystem.instance.CekPunyaBarang(namaItemKeris))
        {
            sudahKabur = true;
            MatikanInteraksi();

            if (SistemKeris.instance != null && SistemKeris.instance.sudahDiritualkan)
                if (eventEnding_KerisKeberuntungan != null) eventEnding_KerisKeberuntungan.Invoke();
            else
                if (eventEnding_KerisMeninggal != null) eventEnding_KerisMeninggal.Invoke();
            return; 
        }

        // --- 1.5 PENGECEKAN BUKU KAKAK (Prioritas 2) ---
        if (InventorySystem.instance != null && InventorySystem.instance.CekPunyaBarang(namaItemBuku))
        {
            sudahKabur = true;
            MatikanInteraksi();
            Debug.Log("[KABUR] Bawa Buku Kakak! Membuka ENDING DIIKUTI.");
            if (eventEnding_BukuDiikuti != null) eventEnding_BukuDiikuti.Invoke();
            return;
        }

        sudahKabur = true;
        MatikanInteraksi();

        // --- 2. PENGECEKAN POIN SOMPRAL ---
        int poinSompralSekarang = 0;
        if (SompralManager.instance != null) poinSompralSekarang = SompralManager.instance.totalPoinSompral;

        if (poinSompralSekarang >= batasPoinSompral)
        {
            if (eventEnding_SompralKabur != null) eventEnding_SompralKabur.Invoke();
            return; 
        }

        // --- 3. LOGIKA KABUR BIASA ---
        int dokumenTerkumpul = 0;
        if (InventorySystem.instance != null && daftarSemuaDokumen.Length > 0)
        {
            foreach (string namaDok in daftarSemuaDokumen)
                if (InventorySystem.instance.CekPunyaBarang(namaDok)) dokumenTerkumpul++; 
        }
        bool isTanpaDokumen = (dokumenTerkumpul == 0);
        bool isRumahBersih = false;
        if (SistemBersihBersih.instance != null) isRumahBersih = SistemBersihBersih.instance.ApakahRumahBersihTotal();

        if (isRumahBersih) { if (eventEnding9_Bersih != null) eventEnding9_Bersih.Invoke(); }
        else if (!isRumahBersih && isTanpaDokumen) { if (eventEnding1_KotorNoDokumen != null) eventEnding1_KotorNoDokumen.Invoke(); }
        else { if (eventEndingLainnya != null) eventEndingLainnya.Invoke(); }
    }

    public void CobaJualKeris() 
    {
        // (Sama seperti sebelumnya)
        if (sudahKabur) return;
        if (InventorySystem.instance != null && !InventorySystem.instance.CekPunyaBarang(namaKunciGerbang)) { TampilkanPesanGagal(); return; }
        if (InventorySystem.instance != null && InventorySystem.instance.CekPunyaBarang(namaItemKeris))
        {
            sudahKabur = true; MatikanInteraksi();
            if (SistemKeris.instance != null && SistemKeris.instance.sudahDiritualkan) { if (eventEnding_KerisKesempatanLain != null) eventEnding_KerisKesempatanLain.Invoke(); }
            else { if (eventEnding_KerisMeninggal != null) eventEnding_KerisMeninggal.Invoke(); }
        }
    }

    private void TampilkanPesanGagal()
    {
        PlayerInteract player = FindAnyObjectByType<PlayerInteract>();
        if (player != null) player.MunculkanSubtitleKustom(pesanGagalKunci, 3f);
    }

    private void MatikanInteraksi()
    {
        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;
    }
}