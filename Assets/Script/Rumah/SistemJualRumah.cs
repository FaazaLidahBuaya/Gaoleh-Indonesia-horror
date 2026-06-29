using UnityEngine;
using UnityEngine.Events;

public class SistemJualRumah : MonoBehaviour
{
    [Header("Syarat Wajib (Biar bisa keluar)")]
    public string namaKunciGerbang = "Kunci Gerbang"; 
    public string pesanGagalKunci = "Gemboknya masih terkunci. Aku butuh kuncinya.";

    [Header("Syarat Dokumen (Sertifikat)")]
    public string[] daftarDokumenYgDibutuhkan; 

    [Header("CABANG ENDING JUAL BIASA")]
    public UnityEvent eventEnding2_KotorNoDokumen;
    public UnityEvent eventEnding3_KotorLengkap;
    public UnityEvent eventEnding7_BersihLengkap;
    public UnityEvent eventEndingLainnya;

    [Header("CABANG ENDING SOMPRAL")]
    [Tooltip("Batas poin sompral untuk memicu ending khusus ini")]
    public int batasPoinSompral = 30;
    [Tooltip("Poin >= 30, gagal jual (kotor / tanpa dokumen)")]
    public UnityEvent eventEnding_SompralJualGagal;
    [Tooltip("Poin >= 30, jual berhasil (bersih + dokumen lengkap)")]
    public UnityEvent eventEnding_SompralBersihLengkap;

    [Header("SISTEM CABANG KERIS (Prioritas Utama)")]
    public string namaItemKeris = "Keris";
    [Tooltip("Kalau nekat jual rumah bawa keris kotor, bakal mati!")]
    public UnityEvent eventEnding_KerisMeninggal;

    private bool sudahTerjual = false;

    public void CobaJual()
    {
        if (sudahTerjual) return;

        // --- 0. CEK KUNCI GERBANG DULU ---
        if (InventorySystem.instance != null && !InventorySystem.instance.CekPunyaBarang(namaKunciGerbang))
        {
            PlayerInteract player = FindAnyObjectByType<PlayerInteract>();
            if (player != null) player.MunculkanSubtitleKustom(pesanGagalKunci, 3f);
            return; 
        }

        // --- 1. CEK KUTUKAN KERIS (Prioritas Mutlak) ---
        if (InventorySystem.instance != null && InventorySystem.instance.CekPunyaBarang(namaItemKeris))
        {
            if (SistemKeris.instance != null && !SistemKeris.instance.sudahDiritualkan)
            {
                sudahTerjual = true;
                MatikanInteraksi();
                Debug.Log("[JUAL RUMAH] Nekat bawa Keris Terkutuk -> ENDING MENINGGAL");
                if (eventEnding_KerisMeninggal != null) eventEnding_KerisMeninggal.Invoke();
                return; 
            }
        }

        sudahTerjual = true;
        MatikanInteraksi();

        // --- CEK DOKUMEN & KEBERSIHAN ---
        int dokumenTerkumpul = 0;
        if (InventorySystem.instance != null && daftarDokumenYgDibutuhkan.Length > 0)
        {
            foreach (string namaDok in daftarDokumenYgDibutuhkan)
            {
                if (InventorySystem.instance.CekPunyaBarang(namaDok)) dokumenTerkumpul++; 
            }
        }
        bool isSertifikatLengkap = (dokumenTerkumpul == daftarDokumenYgDibutuhkan.Length && daftarDokumenYgDibutuhkan.Length > 0);
        bool isTanpaDokumen = (dokumenTerkumpul == 0);

        bool isRumahBersih = false;
        if (SistemBersihBersih.instance != null) isRumahBersih = SistemBersihBersih.instance.ApakahRumahBersihTotal();

        // --- 2. PENGECEKAN POIN SOMPRAL ---
        int poinSompralSekarang = 0;
        if (SompralManager.instance != null) poinSompralSekarang = SompralManager.instance.totalPoinSompral;

        if (poinSompralSekarang >= batasPoinSompral)
        {
            if (isRumahBersih && isSertifikatLengkap)
            {
                Debug.Log($"[JUAL] Sompral ({poinSompralSekarang}) + Bersih + Lengkap -> ENDING SOMPRAL BERSIH LENGKAP");
                if (eventEnding_SompralBersihLengkap != null) eventEnding_SompralBersihLengkap.Invoke();
            }
            else
            {
                Debug.Log($"[JUAL] Sompral ({poinSompralSekarang}) + Gagal -> ENDING SOMPRAL JUAL GAGAL");
                if (eventEnding_SompralJualGagal != null) eventEnding_SompralJualGagal.Invoke();
            }
            return; // Hentikan eksekusi ending biasa
        }

        // --- 3. LOGIKA JUAL RUMAH BIASA ---
        if (isRumahBersih && isSertifikatLengkap)
        {
            if (eventEnding7_BersihLengkap != null) eventEnding7_BersihLengkap.Invoke();
        }
        else if (!isRumahBersih && isSertifikatLengkap)
        {
            if (eventEnding3_KotorLengkap != null) eventEnding3_KotorLengkap.Invoke();
        }
        else if (!isRumahBersih && isTanpaDokumen)
        {
            if (eventEnding2_KotorNoDokumen != null) eventEnding2_KotorNoDokumen.Invoke();
        }
        else
        {
            if (eventEndingLainnya != null) eventEndingLainnya.Invoke();
        }
    }

    private void MatikanInteraksi()
    {
        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;
    }
}