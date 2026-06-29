using UnityEngine;
using System.Collections;

public class PintuLorongKhusus : MonoBehaviour
{
    public enum SumbuPutar { X, Y, Z }

    [Header("Pengaturan Animasi Pintu")]
    [Tooltip("Pilih sumbu engsel yang benar agar pintu tidak buka ke bawah/atas")]
    public SumbuPutar sumbuEngsel = SumbuPutar.Y;
    
    [Tooltip("Sudut terbuka pintu. (Bisa 90 atau -90)")]
    public float sudutBuka = 90f;
    
    [Tooltip("Waktu yang dibutuhkan untuk pintu terbuka penuh (Dalam Detik)")]
    public float durasiBuka = 4f;

    [Header("Audio (Opsional)")]
    [Tooltip("Suara engsel berderit krieek...")]
    public AudioClip sfxPintuDerit;

    [Header("Interaksi Ekstra")]
    [Tooltip("Tarik pintu kamar kakak ke sini agar kuncinya otomatis terbuka saat pintu lorong ini terbuka")]
    public SimpleDoor pintuKamarKakak;

    private void Start()
    {
        if (GetComponent<AudioSource>() == null)
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    public void BukaPelan()
    {
        // === LOGIKA BUKA KUNCI OTOMATIS ===
        // Mengecek apakah pintu kamar kakak ada dan sedang dalam status terkunci
        if (pintuKamarKakak != null && pintuKamarKakak.terkunci)
        {
            pintuKamarKakak.terkunci = false;
            Debug.Log("[PINTU LORONG] Pintu lorong terbuka! Kunci pintu kamar kakak otomatis terlepas secara gaib.");
        }
        
        StartCoroutine(ProsesBuka());
    }

    private IEnumerator ProsesBuka()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        
        if (audioSource != null && sfxPintuDerit != null)
        {
            audioSource.clip = sfxPintuDerit;
            audioSource.Play();
        }

        Quaternion rotasiAwal = transform.localRotation;
        
        // Menentukan arah putaran berdasarkan sumbu yang dipilih di Inspector
        Vector3 arahPutar = Vector3.zero;
        if (sumbuEngsel == SumbuPutar.X) arahPutar = new Vector3(sudutBuka, 0, 0);
        else if (sumbuEngsel == SumbuPutar.Y) arahPutar = new Vector3(0, sudutBuka, 0);
        else if (sumbuEngsel == SumbuPutar.Z) arahPutar = new Vector3(0, 0, sudutBuka);

        Quaternion rotasiTarget = rotasiAwal * Quaternion.Euler(arahPutar);
        float waktu = 0f;
        
        while (waktu < durasiBuka)
        {
            waktu += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(rotasiAwal, rotasiTarget, waktu / durasiBuka);
            yield return null; 
        }
        
        transform.localRotation = rotasiTarget;
    }
}