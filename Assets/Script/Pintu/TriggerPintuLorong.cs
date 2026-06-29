using UnityEngine;
using System.Collections;

public class TriggerPintuLorong : MonoBehaviour
{
    [Header("Referensi Objek")]
    [Tooltip("Masukkan Pintu di ujung lorong yang dipasangi script PintuLorongKhusus")]
    public PintuLorongKhusus pintuUjung;
    
    [Tooltip("Masukkan objek Pocong yang ada di balik pintu")]
    public GameObject pocongLorong;

    [Header("Pengaturan Waktu")]
    [Tooltip("Berapa detik pocong bertahan di sana sejak trigger disentuh?")]
    public float durasiPocongTampil = 3.5f;

    private bool sudahTerpicu = false;

    private void OnTriggerEnter(Collider other)
    {
        if (sudahTerpicu) return;

        // Mengecek apakah yang menabrak adalah karakter pemain
        if (other.GetComponent<PlayerController>() != null)
        {
            sudahTerpicu = true;
            StartCoroutine(ProsesJumpscareLorong());
        }
    }

    private IEnumerator ProsesJumpscareLorong()
    {
        // 1. Munculkan pocong secara ghaib di balik pintu
        if (pocongLorong != null)
        {
            pocongLorong.SetActive(true);
        }

        // 2. Perintahkan pintu untuk terbuka dengan animasi pelan
        if (pintuUjung != null)
        {
            pintuUjung.BukaPelan();
        }

        // 3. Jeda waktu pocong menatap pemain dari kejauhan
        yield return new WaitForSeconds(durasiPocongTampil);

        // 4. Pocong menghilang
        if (pocongLorong != null)
        {
            pocongLorong.SetActive(false);
        }

        // 5. Matikan kubus trigger ini agar tidak berat di memori
        gameObject.SetActive(false);
    }
}