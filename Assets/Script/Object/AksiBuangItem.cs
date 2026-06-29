using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AksiBuangItem : MonoBehaviour
{
    [Header("Pengaturan Animasi Buang")]
    [Tooltip("Kecepatan item melayang ke arah kamera")]
    public float kecepatanTerbang = 8f; 
    
    [Header("Sistem Sompral (Penolak Bala)")]
    [Tooltip("Centang jika benda ini adalah pelindung (Gunting/Silet/Jimat)")]
    public bool membuangItuSompral = true;
    public int poinSompral = 2;
    [TextArea]
    public string catatanDosa = "Pemain dengan sengaja membuang benda penolak bala.";

    [Header("Efek Tambahan (Opsional)")]
    public AudioClip sfxLempar; 
    public UnityEvent aksiSetelahDibuang;

    private bool sudahDibuang = false;

    public void LemparBarang()
    {
        if (sudahDibuang) return; 
        sudahDibuang = true;

        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null)
        {
            interactObj.enabled = false;
        }

        if (sfxLempar != null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                AudioSource audioPlayer = player.GetComponent<AudioSource>();
                if (audioPlayer != null) audioPlayer.PlayOneShot(sfxLempar);
            }
        }

        StartCoroutine(AnimasiBuangKeKamera());

        // --- UPDATE PENCATATAN DOSA FORMAT BARU ---
        InteractableObject io = GetComponent<InteractableObject>();
        string namaBenda = io != null ? io.namaObjek : gameObject.name;
        string catatanFormatLengkap = $"{namaBenda}: {catatanDosa}";

        if (membuangItuSompral && SompralManager.instance != null)
        {
            SompralManager.instance.TambahSompral(poinSompral, catatanFormatLengkap);
        }

        if (aksiSetelahDibuang != null) aksiSetelahDibuang.Invoke();
    }

    private IEnumerator AnimasiBuangKeKamera()
    {
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        Transform kamera = Camera.main.transform;
        float jarak = Vector3.Distance(transform.position, kamera.position);

        while (jarak > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, kamera.position, Time.deltaTime * kecepatanTerbang);
            transform.Rotate(Vector3.up * 360 * Time.deltaTime); 
            
            jarak = Vector3.Distance(transform.position, kamera.position);
            yield return null; 
        }

        gameObject.SetActive(false);
    }
}