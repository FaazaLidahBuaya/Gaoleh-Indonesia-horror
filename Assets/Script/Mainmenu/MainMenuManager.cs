using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MainMenuManager : MonoBehaviour
{
    [Header("Daftar Panel Utama")]
    public GameObject panelMainMenu;
    public GameObject panelSettings;
    public GameObject panelDisclaimer;
    public GameObject panelUpdateInfo;
    public GameObject panelCredits;
    public GameObject panelKontrol;
    public GameObject panelPilihBahasa;

    [Header("Daftar Panel Permainan")]
    public GameObject panelPilihEpisode;
    public GameObject panelAksiEpisode;
    public GameObject panelHasilEnding;

    [Header("Sistem Cerita")]
    public GameObject introPanel;
    public GameObject teksCerita;
    public GameObject tombolLanjutkanCerita;
    public float delayBeforeStory = 2.5f;

    [Header("Sistem Loading (Memuat Game)")]
    public GameObject panelLoading;
    public Slider sliderLoading;
    public TextMeshProUGUI teksProgressLoading;

    [Header("Referensi UI Settings")]
    public Slider sliderAudio;
    public Slider sliderSensitivitas;
    public TMP_Dropdown dropdownResolusi;
    public Toggle toggleFullscreen;

    [Header("Sistem Audio / BGM")]
    public AudioSource audioSourceBGM;
    public AudioClip bgmUtama;

    private int episodeTerpilih = 0;
    private Resolution[] daftarResolusi;
    private static bool sesiPertamaKaliBuka = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InisialisasiSetting();

        // CEK MEMORI: Apakah pemain belum pernah memilih bahasa?
        if (PlayerPrefs.GetInt("SudahPilihBahasa", 0) == 0)
        {
            TutupSemuaPanel();
            if (panelPilihBahasa != null) panelPilihBahasa.SetActive(true);
        }
        else
        {
            // Jika sudah pernah, muat bahasa yang tersimpan dan lanjut ke game
            if (TranslationManager.instance != null)
            {
                TranslationManager.instance.gunakanBahasaInggris = PlayerPrefs.GetInt("Set_Bahasa_Inggris", 0) == 1;
            }
            LanjutKeFlowAwal();
        }
    }

    // Pindahkan logika asli Start() ke fungsi baru ini
    private void LanjutKeFlowAwal()
    {
        if (sesiPertamaKaliBuka)
        {
            sesiPertamaKaliBuka = false;
            BukaDisclaimer();
        }
        else
        {
            KembaliKeMainMenu();
        }
    }

    private void GantiBGM(AudioClip laguBaru)
    {
        if (audioSourceBGM == null || laguBaru == null) return;
        if (audioSourceBGM.clip == laguBaru && audioSourceBGM.isPlaying) return;
        
        audioSourceBGM.clip = laguBaru;
        audioSourceBGM.Play();
    }

    public void BukaDisclaimer()
    {
        TutupSemuaPanel();
        if (panelDisclaimer != null) panelDisclaimer.SetActive(true);
    }

    public void LanjutDariDisclaimer()
    {
        TutupSemuaPanel();
        if (panelUpdateInfo != null) panelUpdateInfo.SetActive(true);
    }

    public void SelesaiBacaUpdateInfo()
    {
        KembaliKeMainMenu();
    }

    public void KembaliKeMainMenu()
    {
        TutupSemuaPanel();
        if (panelMainMenu != null) panelMainMenu.SetActive(true);
        GantiBGM(bgmUtama);
    }

    public void BukaSettings()
    {
        TutupSemuaPanel();
        if (panelSettings != null) panelSettings.SetActive(true);
        GantiBGM(bgmUtama);
    }

    public void BukaUpdateInfo()
    {
        TutupSemuaPanel();
        if (panelUpdateInfo != null) panelUpdateInfo.SetActive(true);
        GantiBGM(bgmUtama);
    }

    // =======================================================
    // FUNGSI BARU: Untuk Membuka Panel Credits
    // =======================================================
    public void BukaCredits()
    {
        TutupSemuaPanel();
        if (panelCredits != null) panelCredits.SetActive(true);
        GantiBGM(bgmUtama);
    }

    public void BukaKontrol()
    {
        TutupSemuaPanel();
        if (panelKontrol != null) panelKontrol.SetActive(true);
        GantiBGM(bgmUtama);
    }

    private void InisialisasiSetting()
    {
        float savedAudio = PlayerPrefs.GetFloat("Set_Audio", 1f);
        AudioListener.volume = savedAudio;
        if (sliderAudio != null) sliderAudio.value = savedAudio;
        
        float savedSens = PlayerPrefs.GetFloat("Set_Sensitivitas", 0.1f);
        if (sliderSensitivitas != null) sliderSensitivitas.value = savedSens;
        
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
        
        bool isFullscreen = PlayerPrefs.GetInt("Set_Fullscreen", 1) == 1;
        Screen.fullScreen = isFullscreen;
        if (toggleFullscreen != null) toggleFullscreen.isOn = isFullscreen;
    }

    public void UbahVolumeAudio(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("Set_Audio", volume);
        PlayerPrefs.Save();
    }

    public void UbahSensitivitasMouse(float nilaiSensitivitas)
    {
        PlayerPrefs.SetFloat("Set_Sensitivitas", nilaiSensitivitas);
        PlayerPrefs.Save();
    }

    public void UbahResolusiLayar(int indeksResolusi)
    {
        if (daftarResolusi == null || daftarResolusi.Length == 0) return;
        Resolution resolusiTerpilih = daftarResolusi[indeksResolusi];
        Screen.SetResolution(resolusiTerpilih.width, resolusiTerpilih.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Set_Resolusi", indeksResolusi);
        PlayerPrefs.Save();
    }

    public void UbahModeLayar(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Set_Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void TombolPlayDitekan()
    {
        TutupSemuaPanel();
        if (panelPilihEpisode != null) panelPilihEpisode.SetActive(true);
        GantiBGM(bgmUtama);
    }

    public void PilihEpisode(int nomorEps)
    {
        episodeTerpilih = nomorEps;
        TutupSemuaPanel();
        if (panelAksiEpisode != null) panelAksiEpisode.SetActive(true);
        GantiBGM(bgmUtama);
    }

    public void BukaDokumen()
    {
        TutupSemuaPanel();
        if (audioSourceBGM != null) audioSourceBGM.Stop();
        StartCoroutine(StorySequence());
    }

    private IEnumerator StorySequence()
    {
        if (introPanel != null) introPanel.SetActive(true);
        if (teksCerita != null) teksCerita.SetActive(false);
        if (tombolLanjutkanCerita != null) tombolLanjutkanCerita.SetActive(false);
        
        yield return new WaitForSeconds(delayBeforeStory);
        
        if (teksCerita != null) teksCerita.SetActive(true);
        if (tombolLanjutkanCerita != null) tombolLanjutkanCerita.SetActive(true);
    }

    // =======================================================
    // SISTEM LOADING BARU (PRE-LOAD ASSET & SHADER)
    // =======================================================
    public void StartGame()
    {
        StartCoroutine(ProsesLoadingScene());
    }

    private IEnumerator ProsesLoadingScene()
    {
        // 1. Matikan panel cerita dan munculkan panel loading
        TutupSemuaPanel();
        if (panelLoading != null) panelLoading.SetActive(true);
        
        // Pastikan audio menu benar-benar mati
        if (audioSourceBGM != null) audioSourceBGM.Stop();
        string namaScene = "Episode_" + episodeTerpilih;
        
        // 2. Mulai memuat dunia di latar belakang (RAM & VRAM)
        AsyncOperation operasiLoading = SceneManager.LoadSceneAsync(namaScene);
        operasiLoading.allowSceneActivation = false; // KUNCI! Tahan jangan masuk game dulu
        
        while (!operasiLoading.isDone)
        {
            // Unity membatasi loading background sampai angka 0.9 (90%)
            float progress = Mathf.Clamp01(operasiLoading.progress / 0.9f);
            
            if (sliderLoading != null) sliderLoading.value = progress;
            if (teksProgressLoading != null) teksProgressLoading.text = "Memuat Aset... " + (progress * 100f).ToString("F0") + "%";
            
            // 3. Jika aset dan 3D model sudah 100% tersusun di RAM
            if (operasiLoading.progress >= 0.9f)
            {
                if (teksProgressLoading != null) teksProgressLoading.text = "Mempersiapkan Shader & Cahaya...";
                
                // Beri waktu 1.5 detik agar CPU bisa melakukan kompilasi Shader dan Lightmap dengan tenang
                // Ini akan menghilangkan lag / stuttering saat game baru pertama kali render!
                yield return new WaitForSeconds(1.5f);
                
                // 4. Buka pintu! Izinkan Unity masuk ke scene Episode_1
                operasiLoading.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void BukaHasil()
    {
        TutupSemuaPanel();
        if (panelHasilEnding != null) panelHasilEnding.SetActive(true);
        GantiBGM(bgmUtama);
    }

    private void TutupSemuaPanel()
    {
        if (panelMainMenu != null) panelMainMenu.SetActive(false);
        if (panelSettings != null) panelSettings.SetActive(false);
        if (panelDisclaimer != null) panelDisclaimer.SetActive(false);
        if (panelUpdateInfo != null) panelUpdateInfo.SetActive(false);
        if (panelPilihEpisode != null) panelPilihEpisode.SetActive(false);
        if (panelAksiEpisode != null) panelAksiEpisode.SetActive(false);
        if (panelHasilEnding != null) panelHasilEnding.SetActive(false);
        if (introPanel != null) introPanel.SetActive(false);
        if (panelLoading != null) panelLoading.SetActive(false);
        if (panelKontrol != null) panelKontrol.SetActive(false);
        if (panelCredits != null) panelCredits.SetActive(false);
        if (panelPilihBahasa != null) panelPilihBahasa.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GantiBahasaIndonesia()
    {
        SimpanBahasaLaluReload(false);
    }

    public void GantiBahasaInggris()
    {
        SimpanBahasaLaluReload(true);
    }

    private void SimpanBahasaLaluReload(bool isInggris)
    {
        if (TranslationManager.instance != null)
        {
            TranslationManager.instance.gunakanBahasaInggris = isInggris;
        }
        
        // Simpan pilihan ke memori permanen
        PlayerPrefs.SetInt("Set_Bahasa_Inggris", isInggris ? 1 : 0);
        PlayerPrefs.SetInt("SudahPilihBahasa", 1); // Mengunci agar panel awal tidak muncul lagi
        PlayerPrefs.Save();

        // Memuat ulang (Reload) scene agar seluruh teks UI langsung berubah bahasanya secara instan
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}