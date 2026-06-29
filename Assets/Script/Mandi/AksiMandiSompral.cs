using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AksiMandiSompral : MonoBehaviour
{
    [Header("Pengaturan Mandi")]
    public float durasiMandi = 4f;
    public AudioClip sfxMandi;
    
    [Header("Sistem Sompral (Pamali Mandi Malam)")]
    public int poinSompral = 2;
    [TextArea]
    public string catatanDosa = "Pemain nekat mandi di tengah malam.";

    [Header("Sistem Jumpscare")]
    [Tooltip("Dieksekusi TEPAT SAAT LAYAR SEDANG GELAP")]
    public UnityEvent aksiSaatLayarGelap;
    [Tooltip("Dieksekusi SAAT LAYAR KEMBALI TERANG")]
    public UnityEvent aksiSetelahMandi;

    private bool sudahMandi = false;

    public void MulaiMandi()
    {
        if (sudahMandi) return;
        sudahMandi = true;

        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;

        StartCoroutine(ProsesMandi());
    }

    private IEnumerator ProsesMandi()
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

        if (aksiSaatLayarGelap != null) aksiSaatLayarGelap.Invoke();

        if (sfxMandi != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(sfxMandi, Camera.main.transform.position);
        }

        yield return new WaitForSeconds(durasiMandi);

        // --- UPDATE PENCATATAN DOSA FORMAT BARU ---
        InteractableObject io = GetComponent<InteractableObject>();
        string namaBenda = io != null ? io.namaObjek : "Bak Mandi";
        string catatanFormatLengkap = $"{namaBenda}: {catatanDosa}";

        if (SompralManager.instance != null)
        {
            SompralManager.instance.TambahSompral(poinSompral, catatanFormatLengkap);
        }

        if (aksiSetelahMandi != null) aksiSetelahMandi.Invoke();

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

        if (player != null) 
        {
            player.enabled = true;
            player.canMove = true;
        }
    }
}