using UnityEngine;
using System.Collections;

public class PocongSembunyi : MonoBehaviour
{
    [Header("Referensi Pocong")]
    [Tooltip("Tarik objek Pocong yang sedang mengintip ke sini")]
    public Transform pocong; 
    
    [Tooltip("Buat GameObject kosong (Empty) di balik tembok tempat Pocong akan sembunyi, lalu tarik ke sini")]
    public Transform titikSembunyi; 

    [Header("Pengaturan Gerakan")]
    [Tooltip("Kecepatan Pocong bergeser/minggir")]
    public float kecepatanGeser = 4f;
    
    [Header("Audio (Opsional)")]
    [Tooltip("Suara langkah cepat / suara wush saat dia kabur")]
    public AudioClip sfxKabur;

    private bool sudahTerpicu = false;

    void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang menyentuh area ini adalah Pemain (Pastikan Player punya tag "Player")
        if (!sudahTerpicu && other.CompareTag("Player"))
        {
            sudahTerpicu = true;
            StartCoroutine(AksiSembunyi());
        }
    }

    private IEnumerator AksiSembunyi()
    {
        // 1. Putar Suara Kabur (Kalau ada)
        if (sfxKabur != null)
        {
            AudioSource.PlayClipAtPoint(sfxKabur, pocong.position);
        }

        // 2. Proses Menggeser Pocong ke Balik Tembok
        if (pocong != null && titikSembunyi != null)
        {
            float jarak = Vector3.Distance(pocong.position, titikSembunyi.position);
            
            // Terus geser sampai pocong menyentuh titik target
            while (jarak > 0.1f)
            {
                pocong.position = Vector3.Lerp(pocong.position, titikSembunyi.position, Time.deltaTime * kecepatanGeser);
                jarak = Vector3.Distance(pocong.position, titikSembunyi.position);
                
                yield return null; // Tunggu frame berikutnya
            }
        }

        // 3. Matikan/Hilangkan Pocongnya secara permanen setelah aman di balik tembok
        if (pocong != null)
        {
            pocong.gameObject.SetActive(false);
        }
    }
}