using UnityEngine;

public class SyaratItemRitual : MonoBehaviour
{
    // 1. Dipanggil saat memeriksa Kertas Petunjuk
    public void BacaKertas()
    {
        if (SistemKeris.instance != null)
        {
            SistemKeris.instance.kertasRitualDibaca = true;
            
            PlayerInteract player = FindAnyObjectByType<PlayerInteract>();
            if (player != null) player.MunculkanSubtitleKustom("Sepertinya ini cara untuk menyucikan keris kakek...", 4f);
        }
    }

    // 2. Dipanggil dari tombol "Ambil Bunga" / "Ambil Dupa"
    public void CobaAmbilSesajen()
    {
        if (SistemKeris.instance != null && SistemKeris.instance.kertasRitualDibaca)
        {
            // Jika sudah baca kertas, barang masuk tas!
            GetComponent<InteractableObject>().AmbilMasukInventory();
        }
        else
        {
            // Jika belum baca kertas, Arya menolak mengambilnya
            PlayerInteract player = FindAnyObjectByType<PlayerInteract>();
            if (player != null) player.MunculkanSubtitleKustom("Hanya benda biasa, aku tidak membutuhkannya sekarang.", 3f);
        }
    }
}