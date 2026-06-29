using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AksiPipisSembarangan : MonoBehaviour
{
    [Header("Pengaturan Aksi")]
    public float durasiPipis = 5f;
    public AudioClip sfxPipis;

    [Header("Sistem Sompral")]
    public int poinSompral = 10;
    [TextArea]
    public string catatanDosa = "Pemain nekat pipis sembarangan di pohon keramat.";

    [Header("Efek Jumpscare (Pocong & Pintu)")]
    public GameObject pocongIntip;
    public SimpleDoor pintuDepanRumah;
    [Tooltip("Masukkan objek Trigger Banting Pintu di sini")]
    public GameObject jebakanBantingPintu; 

    [Header("Timing Layar Hitam")]
    [Tooltip("Berapa detik jeda dari layar HITAM TOTAL sampai pintu terbuka sendiri?")]
    public float jedaPintuTerbuka = 1.5f;

    private bool sudahPipis = false;

    public void MulaiPipis()
    {
        if (sudahPipis) return;
        sudahPipis = true;

        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;

        StartCoroutine(ProsesPipis());
    }

    private IEnumerator ProsesPipis()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) 
        {
            player.canMove = false;
            player.enabled = false;
            AudioSource audioPlayer = player.GetComponent<AudioSource>();
            if (audioPlayer != null) audioPlayer.Stop();
        }

        CanvasGroup layarHitam = null;
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            float fadeTimer = 0f;
            while (fadeTimer < 1f)
            {
                fadeTimer += Time.deltaTime;
                layarHitam.alpha = Mathf.Clamp01(fadeTimer);
                yield return null;
            }
            layarHitam.alpha = 1f;
        }

        if (sfxPipis != null) AudioSource.PlayClipAtPoint(sfxPipis, Camera.main.transform.position);

        yield return new WaitForSeconds(jedaPintuTerbuka);

        if (pintuDepanRumah != null) pintuDepanRumah.BukaPaksa();

        float sisaDurasiPipis = Mathf.Max(0.5f, durasiPipis - jedaPintuTerbuka);
        yield return new WaitForSeconds(sisaDurasiPipis);

        // --- UPDATE PENCATATAN DOSA FORMAT BARU ---
        InteractableObject io = GetComponent<InteractableObject>();
        string namaBenda = io != null ? io.namaObjek : "Pohon";
        string catatanFormatLengkap = $"{namaBenda}: {catatanDosa}";

        if (SompralManager.instance != null) 
        {
            SompralManager.instance.TambahSompral(poinSompral, catatanFormatLengkap);
        }

        if (pocongIntip != null) pocongIntip.SetActive(true);

        if (layarHitam != null)
        {
            float fadeTimer = 0f;
            while (fadeTimer < 1f)
            {
                fadeTimer += Time.deltaTime;
                layarHitam.alpha = Mathf.Clamp01(1f - fadeTimer);
                yield return null;
            }
            layarHitam.alpha = 0f;
            layarHitam.gameObject.SetActive(false);
        }

        if (jebakanBantingPintu != null) jebakanBantingPintu.SetActive(true);

        if (player != null) 
        {
            player.enabled = true;
            player.canMove = true;
        }
    }
}