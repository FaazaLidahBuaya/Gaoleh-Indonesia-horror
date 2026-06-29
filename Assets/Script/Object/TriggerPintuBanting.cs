using UnityEngine;
using System.Collections;

public class TriggerPintuBanting : MonoBehaviour
{
    [Header("Objek yang Terlibat")]
    public SimpleDoor pintuDepan;
    public GameObject pocongJumpscare;

    [Header("Pengaturan Waktu")]
    [Tooltip("Berapa detik pocong bertahan di dalam setelah pintu tertutup? (Standar: 1 detik)")]
    public float jedaHilangPocong = 1f;

    private bool sudahTerpemicu = false;

    private void OnTriggerEnter(Collider other)
    {
        // Pengecekan agar tidak berjalan dua kali
        if (sudahTerpemicu) return;

        // Mengecek apakah yang menabrak area ini adalah pemain
        if (other.GetComponent<PlayerController>() != null)
        {
            sudahTerpemicu = true;
            StartCoroutine(ProsesBantingDanHilang());
        }
    }

    private IEnumerator ProsesBantingDanHilang()
    {
        // 1. Banting Pintu (Tutup Paksa) seketika saat pemain menginjak trigger
        if (pintuDepan != null) 
        {
            // Tambahkan 'false' agar pintu dibanting tapi tidak dikunci.
            // (Ubah jadi 'true' kalau kamu mau pemain terkurung bareng pocongnya sebentar)
            pintuDepan.TutupPaksa(false); 
        }

        // 2. Beri jeda beberapa saat di dalam rumah yang tertutup
        yield return new WaitForSeconds(jedaHilangPocong);

        // 3. Setelah jeda selesai, barulah pocong dihilangkan (ghaib)
        if (pocongJumpscare != null) 
        {
            pocongJumpscare.SetActive(false);
        }

        // 4. Matikan trigger ini agar tidak bisa dipakai lagi
        gameObject.SetActive(false);
    }
}