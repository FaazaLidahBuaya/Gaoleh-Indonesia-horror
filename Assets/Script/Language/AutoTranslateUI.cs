using UnityEngine;
using TMPro; 

public class AutoTranslateUI : MonoBehaviour
{
    private TextMeshProUGUI teksUI;
    private bool sudahDiterjemahkan = false;

    // OnEnable dipanggil otomatis SETIAP KALI tombolnya muncul di layar
    void OnEnable()
    {
        if (teksUI == null) teksUI = GetComponent<TextMeshProUGUI>();
        
        // Menerjemahkan secara instan tanpa perlu menunggu
        if (teksUI != null && TranslationManager.instance != null && !sudahDiterjemahkan)
        {
            teksUI.text = TranslationManager.instance.Terjemahkan(teksUI.text);
            sudahDiterjemahkan = true; // Supaya tidak diterjemahkan berkali-kali setiap menu dibuka
        }
    }
}