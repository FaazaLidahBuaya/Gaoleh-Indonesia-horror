using UnityEngine;
using UnityEngine.Events;

public class SistemGembok : MonoBehaviour
{
    [Header("Pengaturan Syarat")]
    [Tooltip("Harus SAMA PERSIS dengan 'Nama Objek' kunci di Inspector")]
    public string namaKunciYgDibutuhkan = "Kunci Utama";
    
    [Tooltip("Teks yang muncul kalau pemain belum punya kuncinya")]
    public string pesanGagal = "Aku tidak punya kuncinya...";

    [Header("Jika Berhasil Buka:")]
    [Tooltip("Masukkan fungsi Ending atau animasi buka pintu di sini")]
    public UnityEvent aksiJikaBerhasil;

    // Fungsi ini yang akan dipanggil dari tombol UI
    public void CobaBuka()
    {
        // 1. Cek apakah tas punya barangnya?
        if (InventorySystem.instance != null && InventorySystem.instance.CekPunyaBarang(namaKunciYgDibutuhkan))
        {
            // 2. Kunci ketemu! Eksekusi ending
            aksiJikaBerhasil.Invoke();
        }
        else
        {
            // 3. Kunci tidak ada! Panggil teks subtitle
            PlayerInteract player = FindAnyObjectByType<PlayerInteract>();
            if (player != null)
            {
                player.MunculkanSubtitleKustom(pesanGagal, 3f);
            }
        }
    }
}