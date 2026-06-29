using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PauseMenuManager : MonoBehaviour
{
    public static PauseMenuManager instance;

    [Header("Referensi UI Utama")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    [Header("Referensi UI Settings")]
    public Slider sliderAudio;
    public Slider sliderSensitivitas;
    public TMP_Dropdown dropdownResolusi;
    [Tooltip("Masukkan UI Toggle untuk Fullscreen di sini")]
    public Toggle toggleFullscreen; 

    [Header("Pengaturan Scene")]
    public string namaSceneMainMenu = "MainMenu";

    [HideInInspector]
    public bool isPaused = false;

    private Resolution[] daftarResolusi;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        isPaused = false;

        InisialisasiSetting();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused)
            {
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    TutupSetting();
                }
                else
                {
                    LanjutkanGame();
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    // =========================================================
    // SISTEM PAUSE & RESUME
    // =========================================================
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) { player.canMove = false; player.enabled = false; }

        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        AudioListener.pause = true; 
    }

    public void LanjutkanGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 
        
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) { player.canMove = true; player.enabled = true; }

        bool hpTerbuka = PhoneSystem.instance != null && PhoneSystem.instance.isPhoneOpen;
        bool tasTerbuka = InventorySystem.instance != null && InventorySystem.instance.inventoryUI != null && InventorySystem.instance.inventoryUI.activeInHierarchy;
        bool periksaTerbuka = ExaminationSystem.instance != null && ExaminationSystem.instance.examinationUI != null && ExaminationSystem.instance.examinationUI.activeInHierarchy;

        if (!hpTerbuka && !tasTerbuka && !periksaTerbuka)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        AudioListener.pause = false;
    }

    public void BukaSetting()
    {
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void TutupSetting()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pauseMenuPanel != null) pauseMenuPanel.SetActive(true);
    }

    public void KeluarKeMainMenu()
    {
        Time.timeScale = 1f; 
        AudioListener.pause = false;
        isPaused = false;
        
        SceneManager.LoadScene(namaSceneMainMenu);
    }

    // =========================================================
    // SISTEM PENGATURAN (SETTINGS)
    // =========================================================
    private void InisialisasiSetting()
    {
        // 1. Muat Data Audio
        float savedAudio = PlayerPrefs.GetFloat("Set_Audio", 1f); 
        AudioListener.volume = savedAudio;
        if (sliderAudio != null) sliderAudio.value = savedAudio;

        // 2. Muat Data Sensitivitas
        float savedSens = PlayerPrefs.GetFloat("Set_Sensitivitas", 0.1f); 
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) player.mouseSensitivity = savedSens;
        if (sliderSensitivitas != null) sliderSensitivitas.value = savedSens;

        // 3. Muat Data Resolusi Layar
        if (dropdownResolusi != null)
        {
            daftarResolusi = Screen.resolutions;
            dropdownResolusi.ClearOptions();

            List<string> opsiResolusi = new List<string>();
            int resolusiSekarangIndeks = 0;

            for (int i = 0; i < daftarResolusi.Length; i++)
            {
                string opsi = daftarResolusi[i].width + " x " + daftarResolusi[i].height;
                opsiResolusi.Add(opsi);

                if (daftarResolusi[i].width == Screen.currentResolution.width &&
                    daftarResolusi[i].height == Screen.currentResolution.height)
                {
                    resolusiSekarangIndeks = i;
                }
            }

            dropdownResolusi.AddOptions(opsiResolusi);

            int savedResIndex = PlayerPrefs.GetInt("Set_Resolusi", resolusiSekarangIndeks);
            dropdownResolusi.value = savedResIndex;
            dropdownResolusi.RefreshShownValue();
        }

        // 4. Muat Data Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt("Set_Fullscreen", 1) == 1; // Default 1 (artinya True/Fullscreen)
        Screen.fullScreen = isFullscreen;
        if (toggleFullscreen != null) toggleFullscreen.isOn = isFullscreen;
    }

    public void UbahVolumeAudio(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Set_Audio", volume);
        PlayerPrefs.Save();
        Debug.Log("[SETTING] Volume diubah jadi: " + volume);
    }

    public void UbahSensitivitasMouse(float nilaiSensitivitas)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.mouseSensitivity = nilaiSensitivitas;
        }
        PlayerPrefs.SetFloat("Set_Sensitivitas", nilaiSensitivitas);
        PlayerPrefs.Save();
        Debug.Log("[SETTING] Sensitivitas diubah jadi: " + nilaiSensitivitas);
    }

    public void UbahResolusiLayar(int indeksResolusi)
    {
        if (daftarResolusi == null || daftarResolusi.Length == 0) return;

        Resolution resolusiTerpilih = daftarResolusi[indeksResolusi];
        Screen.SetResolution(resolusiTerpilih.width, resolusiTerpilih.height, Screen.fullScreen);
        
        PlayerPrefs.SetInt("Set_Resolusi", indeksResolusi);
        PlayerPrefs.Save();
        Debug.Log("[SETTING] Resolusi diubah jadi: " + resolusiTerpilih.width + "x" + resolusiTerpilih.height);
    }

    // FUNGSI BARU UNTUK FULLSCREEN
    public void UbahModeLayar(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Set_Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("[SETTING] Fullscreen mode: " + isFullscreen);
    }
}