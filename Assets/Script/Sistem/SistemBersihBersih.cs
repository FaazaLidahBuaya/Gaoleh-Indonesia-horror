using UnityEngine;
using UnityEngine.Events;

public class SistemBersihBersih : MonoBehaviour
{
    public static SistemBersihBersih instance;

    [Header("Progress Bersih-Bersih (Otomatis)")]
    [Tooltip("Sistem akan otomatis menghitung jumlah noda di ruangan saat game dimulai.")]
    public int totalNodaDiRumah = 0; 

    [Tooltip("Geser untuk mengatur persentase noda yang wajib dibersihkan agar dianggap selesai.")]
    [Range(1f, 100f)]
    public float persentaseTarget = 70f;
    
    [Tooltip("Jumlah noda yang wajib dibersihkan berdasarkan persentase di atas. Dihitung otomatis.")]
    public int targetNodaBersih = 0; 
    
    [HideInInspector]
    public int nodaDibersihkan = 0;

    [Header("Syarat Alat")]
    [Tooltip("Nama alat yang harus ada di dalam tas (Pastikan huruf besar/kecilnya sama)")]
    public string namaAlatPembersih = "Kain Pel";

    [Header("Event Ketika Rumah Bersih")]
    [Tooltip("Apa yang terjadi saat target noda berhasil dibersihkan?")]
    public UnityEvent aksiSaatSemuaBersih;

    // Gembok agar event selesai tidak terpanggil berkali-kali setiap ngepel noda sisa
    private bool sudahSelesai = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // 1. SISTEM OTOMATIS MENGHITUNG SEMUA NODA DI SCENE
        AksiBersihkanNoda[] semuaNoda = FindObjectsByType<AksiBersihkanNoda>(FindObjectsInactive.Exclude);
        totalNodaDiRumah = semuaNoda.Length;

        // 2. MENGHITUNG TARGET BERDASARKAN PERSENTASE CUSTOM
        targetNodaBersih = Mathf.CeilToInt(totalNodaDiRumah * (persentaseTarget / 100f));

        Debug.Log($"[BERSIH-BERSIH] Ditemukan total {totalNodaDiRumah} noda di rumah.");
        Debug.Log($"[BERSIH-BERSIH] Target {persentaseTarget}% kebersihan: Pemain harus membersihkan {targetNodaBersih} noda.");
    }

    public void CatatSatuNodaBersih()
    {
        nodaDibersihkan++;
        Debug.Log($"[BERSIH-BERSIH] Noda dibersihkan! Progress: {nodaDibersihkan} / {targetNodaBersih} (Menuju {persentaseTarget}%)");

        // 3. CEK APAKAH TARGET SUDAH TERCAPAI
        if (!sudahSelesai && nodaDibersihkan >= targetNodaBersih)
        {
            sudahSelesai = true;
            
            if (aksiSaatSemuaBersih != null) aksiSaatSemuaBersih.Invoke();
            
            Debug.Log($"[BERSIH-BERSIH] RUMAH SUDAH DIANGGAP BERSIH ({persentaseTarget}%+)!");
            
            // Beri petunjuk ke pemain agar mereka tahu tugasnya sudah selesai
            PlayerInteract playerUI = FindAnyObjectByType<PlayerInteract>();
            if (playerUI != null)
            {
                playerUI.MunculkanSubtitleKustom("Huft... Sepertinya rumah ini sudah cukup bersih.", 4f);
            }
        }
    }

    // Fungsi ini nanti dipanggil oleh SistemJualRumah / Pintu Keluar
    public bool ApakahRumahBersihTotal()
    {
        return nodaDibersihkan >= targetNodaBersih;
    }
}