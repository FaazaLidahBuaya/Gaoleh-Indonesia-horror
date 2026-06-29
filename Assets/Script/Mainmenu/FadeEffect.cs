using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadePanel; 
    
    [Tooltip("Lama layar hitam bertahan (detik) sebelum mulai fade")]
    public float delayBeforeFade = 2.0f; 
    
    [Tooltip("Lama proses fade dari hitam ke terang (detik)")]
    public float fadeDuration = 3.0f; 

    void Start()
    {
        if (fadePanel != null)
        {
            // Pastikan panel aktif saat game dimulai
            fadePanel.gameObject.SetActive(true);
            
            // Jalankan proses coroutine
            StartCoroutine(FadeInRoutine());
        }
        else
        {
            Debug.LogWarning("Fade Panel belum dimasukkan ke Inspector!");
        }
    }

    private IEnumerator FadeInRoutine()
    {
        // 1. PASTIKAN WARNA AWAL HITAM PEKAT
        Color currentColor = fadePanel.color;
        currentColor.a = 1f;
        fadePanel.color = currentColor;

        // 2. JEDA SEBELUM FADE
        // Sistem akan menahan eksekusi di baris ini selama waktu delayBeforeFade (2 detik)
        yield return new WaitForSeconds(delayBeforeFade);

        // 3. PROSES FADE-IN (MEMUDAR)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            
            // Hitung nilai alpha baru (dari 1 ke 0)
            currentColor.a = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            fadePanel.color = currentColor;
            
            // Tunggu frame berikutnya
            yield return null;
        }

        // 4. BERSIHKAN PANEL DI AKHIR
        currentColor.a = 0f;
        fadePanel.color = currentColor;
        fadePanel.gameObject.SetActive(false);
    }
}