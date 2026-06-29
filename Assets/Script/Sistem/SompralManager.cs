using UnityEngine;
using System.Collections.Generic;

public class SompralManager : MonoBehaviour
{
    public static SompralManager instance;

    [Header("Data Sompral Pemain")]
    public int totalPoinSompral = 0;
    
    [Tooltip("Daftar kelakuan buruk yang akan di-print di akhir game")]
    public List<string> daftarDosaPemain = new List<string>(); 

    void Awake()
    {
        instance = this;
    }

    // Fungsi ini akan dipanggil setiap kali pemain melakukan hal sompral
    public void TambahSompral(int poin, string catatanUntukEnding)
    {
        totalPoinSompral += poin;
        
        // Simpan ke buku catatan jika teksnya tidak kosong
        if (!string.IsNullOrEmpty(catatanUntukEnding))
        {
            daftarDosaPemain.Add(catatanUntukEnding);
        }

        Debug.Log("[SOMPRAL BERTAMBAH] Poin sekarang: " + totalPoinSompral + " | Aksi: " + catatanUntukEnding);

        // =========================================================
        // EFEK HUKUMAN: Bikin hantu makin marah & teror makin cepat!
        // =========================================================
        PercepatTeror();
    }

    private void PercepatTeror()
    {
        // Hubungkan ke sistem teror HP yang sudah kita buat
        if (NightManager.instance != null && NightManager.instance.terrorManager != null)
        {
            TerrorMessageManager teror = NightManager.instance.terrorManager;

            // Setiap 1 poin sompral, potong waktu tunggu teror sebanyak 5 detik
            teror.jedaMinimalPesan -= 5f;
            teror.jedaMaksimalPesan -= 5f;

            // Pastikan jeda tidak tembus ke angka minus (mentok di 5 detik)
            if (teror.jedaMinimalPesan < 5f) teror.jedaMinimalPesan = 5f;
            if (teror.jedaMaksimalPesan < 10f) teror.jedaMaksimalPesan = 10f;
        }
    }

    // Fungsi ini nanti dipanggil oleh EndingManager untuk menampilkan dosa pemain
    public string DapatkanLaporanSompral()
    {
        if (daftarDosaPemain.Count == 0) return "Kamu bertahan hidup dengan sangat sopan. Tidak ada gangguan yang terpancing.";

        string laporan = "Riwayat Sompral:\n";
        foreach (string dosa in daftarDosaPemain)
        {
            laporan += "- " + dosa + "\n";
        }
        return laporan;
    }
}