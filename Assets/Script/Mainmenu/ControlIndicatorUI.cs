using UnityEngine;

public class ControlIndicatorUI : MonoBehaviour
{
    public static ControlIndicatorUI instance;

    [Header("UI Objects")]
    public GameObject uiR_Inventory;
    public GameObject uiE_Phone;
    public GameObject uiF_Flashlight; // Khusus malam 3

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // Pastikan saat game mulai, hanya R dan E yang muncul
        if (uiR_Inventory) uiR_Inventory.SetActive(true);
        if (uiE_Phone) uiE_Phone.SetActive(true);
        
        UpdateFlashlightUI();
    }

    // Panggil fungsi ini di NightManager saat ganti malam
    public void UpdateFlashlightUI()
    {
        if (uiF_Flashlight)
        {
            // Munculkan hanya jika malam ke-3
            bool isMalam3 = (NightManager.instance != null && NightManager.instance.malamSekarang == 3);
            uiF_Flashlight.SetActive(isMalam3);
        }
    }
}