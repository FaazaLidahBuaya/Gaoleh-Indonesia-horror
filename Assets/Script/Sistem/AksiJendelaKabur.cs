using UnityEngine;
using System.Collections;

public class AksiJendelaKabur : MonoBehaviour
{
    [Header("Titik Tujuan (Bolak-Balik)")]
    [Tooltip("Titik penanda saat pemain KELUAR ke lorong")]
    public Transform titikLuarLorong; 
    [Tooltip("Titik penanda saat pemain MASUK KEMBALI ke dalam kamar")]
    public Transform titikDalamKamar; 

    [Header("Efek Suara (Opsional)")]
    public AudioClip sfxBukaJendela;
    public AudioClip sfxAnginLorong; 

    private bool sedangProses = false;
    
    // Pelacak posisi: False = Di Dalam Kamar, True = Di Lorong Luar
    private bool posisiDiLuar = false; 

    public void EksekusiKeluarMasukJendela()
    {
        if (sedangProses) return;
        sedangProses = true;
        StartCoroutine(ProsesKaburAtauMasuk());
    }

    private IEnumerator ProsesKaburAtauMasuk()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        // 1. KUNCI PERGERAKAN PEMAIN
        if (player != null) player.canMove = false;
        if (interact != null) interact.KlikBatal();

        if (sfxBukaJendela != null) AudioSource.PlayClipAtPoint(sfxBukaJendela, transform.position);

        // 2. FADE OUT KE HITAM
        CanvasGroup layarHitam = null;
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            
            float fadeTimer = 0f;
            while (fadeTimer < 1.5f) // Durasi Fade Out 1.5 detik
            {
                fadeTimer += Time.deltaTime;
                layarHitam.alpha = Mathf.Clamp01(fadeTimer / 1.5f);
                yield return null;
            }
            layarHitam.alpha = 1f;
        }

        yield return new WaitForSeconds(0.5f); 

        // 3. LOGIKA TELEPORTASI BOLAK-BALIK
        if (player != null)
        {
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; 

            if (!posisiDiLuar && titikLuarLorong != null)
            {
                // DARI KAMAR -> MAU KE LORONG LUAR
                player.transform.position = titikLuarLorong.position;
                player.transform.rotation = titikLuarLorong.rotation;
                if (Camera.main != null) Camera.main.transform.rotation = titikLuarLorong.rotation;
                
                posisiDiLuar = true; // Update status Arya sekarang di luar
                if (sfxAnginLorong != null) AudioSource.PlayClipAtPoint(sfxAnginLorong, titikLuarLorong.position);
            }
            else if (posisiDiLuar && titikDalamKamar != null)
            {
                // DARI LORONG LUAR -> MAU KEMBALI KE KAMAR
                player.transform.position = titikDalamKamar.position;
                player.transform.rotation = titikDalamKamar.rotation;
                if (Camera.main != null) Camera.main.transform.rotation = titikDalamKamar.rotation;
                
                posisiDiLuar = false; // Update status Arya sekarang di dalam
            }

            if (cc != null) cc.enabled = true; // Nyalakan lagi fisikanya
        }

        yield return new WaitForSeconds(1f);

        // 4. FADE IN (MATA TERBUKA)
        if (layarHitam != null)
        {
            float fadeTimer = 0f;
            while (fadeTimer < 1.5f)
            {
                fadeTimer += Time.deltaTime;
                layarHitam.alpha = Mathf.Clamp01(1f - (fadeTimer / 1.5f));
                yield return null;
            }
            layarHitam.alpha = 0f;
            layarHitam.gameObject.SetActive(false);
        }

        // 5. KEMBALIKAN KONTROL PEMAIN BEBAS BERGERAK
        if (player != null) player.canMove = true;
        sedangProses = false;
    }
}