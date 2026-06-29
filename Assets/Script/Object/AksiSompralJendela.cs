using UnityEngine;
using System.Collections;

public class AksiSompralJendela : MonoBehaviour
{
    [Header("Pengaturan Jeda & Kecepatan")]
    public float jedaSebelumNengok = 3f;
    public float kecepatanNengok = 1.5f;

    [Header("Sutradara Kamera & Suara")]
    public Transform targetLihatKamera;
    public AudioClip sfxNging;
    [Range(0f, 1f)]
    [Tooltip("Atur volume SFX (0.0 sampai 1.0)")]
    public float volumeSfx = 1f; 

    [Header("Animasi Pocong Lewat")]
    public GameObject pocongJendela;
    public Transform titikAwal;
    public Transform titikAkhir;
    public float kecepatanPocong = 4f;

    private bool sudahDieksekusi = false;

    public void EksekusiJumpscare()
    {
        if (sudahDieksekusi) return;
        sudahDieksekusi = true;

        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;

        StartCoroutine(ProsesJumpscare());
    }

    private IEnumerator ProsesJumpscare()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        Camera mainCam = Camera.main;
        
        // Kunci kaki pemain
        if (player != null) player.canMove = false;

        // Fase Hening
        yield return new WaitForSeconds(jedaSebelumNengok);

        // Matikan kendali total
        if (player != null) player.enabled = false;

        // PAKSA KAMERA NENGOK KE JENDELA
        if (mainCam != null && targetLihatKamera != null)
        {
            Quaternion rotasiAwal = mainCam.transform.rotation;
            Vector3 arahTarget = targetLihatKamera.position - mainCam.transform.position;
            Quaternion rotasiTarget = Quaternion.LookRotation(arahTarget);

            float t = 0;
            while (t < 1f)
            {
                t += Time.deltaTime * kecepatanNengok; 
                mainCam.transform.rotation = Quaternion.Slerp(rotasiAwal, rotasiTarget, t);
                yield return null;
            }
            // Kunci pas ke target jendela
            mainCam.transform.rotation = rotasiTarget; 
        }

        // PLAY SFX (Dimainkan tepat di telinga pemain dengan volume khusus)
        if (sfxNging != null && mainCam != null) 
        {
            AudioSource.PlayClipAtPoint(sfxNging, mainCam.transform.position, volumeSfx);
        }

        // POCONG LEWAT
        if (pocongJendela != null && titikAwal != null && titikAkhir != null)
        {
            pocongJendela.transform.position = titikAwal.position;
            pocongJendela.SetActive(true);

            float jarak = Vector3.Distance(titikAwal.position, titikAkhir.position);
            float durasi = jarak / kecepatanPocong;
            float timer = 0;

            while (timer < durasi)
            {
                timer += Time.deltaTime;
                pocongJendela.transform.position = Vector3.Lerp(titikAwal.position, titikAkhir.position, timer / durasi);
                yield return null;
            }

            pocongJendela.SetActive(false);
        }

        // KEMBALIKAN KENDALI PEMAIN
        if (player != null)
        {
            player.enabled = true;
            player.canMove = true;
        }
    }
}