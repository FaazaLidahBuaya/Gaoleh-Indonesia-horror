using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AksiPerbaikiBarang : MonoBehaviour
{
    [Header("Pengaturan Animasi")]
    [Tooltip("Waktu yang dibutuhkan barang melayang ke posisi semula (detik)")]
    public float durasiAnimasi = 0.6f;
    public AudioClip sfxMemperbaiki;

    [Header("Event Setelah Selesai")]
    [Tooltip("Apa yang terjadi setelah barang ditaruh kembali?")]
    public UnityEvent aksiSetelahDiperbaiki;

    private Vector3 posisiAwal;
    private Quaternion rotasiAwal;
    private Rigidbody rb;
    private Collider col;
    private bool sedangDiperbaiki = false;

    void Start()
    {
        // Sistem otomatis mengingat posisi rapi benda ini saat game baru dimulai
        posisiAwal = transform.position;
        rotasiAwal = transform.rotation;
        
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void MulaiPerbaiki()
    {
        if (sedangDiperbaiki) return;
        sedangDiperbaiki = true;

        // Matikan interaksi agar tidak diklik dua kali oleh pemain yang spam klik
        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;

        StartCoroutine(ProsesMemperbaiki());
    }

    private IEnumerator ProsesMemperbaiki()
    {
        if (col != null) col.enabled = false; 
        if (rb != null) rb.isKinematic = true; // Matikan gravitasi

        Vector3 posisiJatuh = transform.position;
        Quaternion rotasiJatuh = transform.rotation;
        float timer = 0;

        if (sfxMemperbaiki != null) AudioSource.PlayClipAtPoint(sfxMemperbaiki, transform.position);

        while (timer < durasiAnimasi)
        {
            timer += Time.deltaTime;
            float t = timer / durasiAnimasi;
            t = t * t * (3f - 2f * t); 

            transform.position = Vector3.Lerp(posisiJatuh, posisiAwal, t);
            transform.rotation = Quaternion.Slerp(rotasiJatuh, rotasiAwal, t);
            yield return null;
        }

        transform.position = posisiAwal;
        transform.rotation = rotasiAwal;

        // Panggil event untuk menyadarkan player tentang jendela
        if (aksiSetelahDiperbaiki != null) aksiSetelahDiperbaiki.Invoke();
    }
}