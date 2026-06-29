using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; 

[RequireComponent(typeof(AudioSource))]
public class EndingManager : MonoBehaviour
{
    public static EndingManager instance;

    [Header("Referensi UI Ending")]
    public CanvasGroup endingPanel;       
    public TextMeshProUGUI textEndingJudul;  
    public TextMeshProUGUI textEndingNarasi; 
    public GameObject tombolLanjutkan; 

    [Header("Referensi UI Laporan Sompral")]
    public CanvasGroup panelLaporanSompral; 
    public TextMeshProUGUI textLaporanSompral; 
    public GameObject tombolMainMenu; 

    [Header("Pengaturan Scene")]
    public string namaSceneMainMenu = "MainMenu"; 

    private AudioSource audioSource;
    private bool isEndingTriggered = false;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1f; 
        if (Camera.main != null) Camera.main.enabled = true; 
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (endingPanel != null) { endingPanel.alpha = 0f; endingPanel.gameObject.SetActive(false); }
        if (panelLaporanSompral != null) { panelLaporanSompral.alpha = 0f; panelLaporanSompral.gameObject.SetActive(false); }
        if (textEndingJudul != null) textEndingJudul.text = "";
        if (textEndingNarasi != null) textEndingNarasi.text = "";
        if (textLaporanSompral != null) textLaporanSompral.text = "";
        if (tombolLanjutkan != null) tombolLanjutkan.SetActive(false);
        if (tombolMainMenu != null) tombolMainMenu.SetActive(false);
    }

    public void TriggerEnding(int idEnding, string judul, string narasi, AudioClip sfxEnding, bool gunakanFade = true)
    {
        if (isEndingTriggered) return; 
        isEndingTriggered = true;

        // ========================================================
        // SIMPAN STATUS, JUDUL, DAN NARASI KE MEMORI PERMANEN!
        // ========================================================
        PlayerPrefs.SetInt("UnlockEnding_" + idEnding, 1);
        PlayerPrefs.SetString("JudulEnding_" + idEnding, judul);
        PlayerPrefs.SetString("NarasiEnding_" + idEnding, narasi);
        PlayerPrefs.Save();
        
        Debug.Log("🔓 Ending " + idEnding + " beserta teksnya berhasil disimpan ke memori!");

        StartCoroutine(EndingSequence(judul, narasi, sfxEnding, gunakanFade));
    }

    private IEnumerator EndingSequence(string judul, string narasi, AudioClip sfx, bool gunakanFade)
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) { player.canMove = false; player.enabled = false; AudioSource audioPlayer = player.GetComponent<AudioSource>(); if (audioPlayer != null) audioPlayer.Stop(); }

        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();
        if (interact != null) interact.enabled = false; 

        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            NightManager.instance.panelTransisiHitam.alpha = 0f;
            NightManager.instance.panelTransisiHitam.gameObject.SetActive(false);
        }

        endingPanel.gameObject.SetActive(true);

        if (gunakanFade)
        {
            endingPanel.alpha = 0f;
            float fadeTimer = 0f;
            while (fadeTimer < 1f) { fadeTimer += Time.deltaTime * 1.5f; endingPanel.alpha = Mathf.Clamp01(fadeTimer); yield return null; }
        }
        
        endingPanel.alpha = 1f;

        Time.timeScale = 0f; 
        Application.targetFrameRate = 60; 

        if (Camera.main != null) { Camera.main.cullingMask = 0; Camera.main.clearFlags = CameraClearFlags.SolidColor; Camera.main.backgroundColor = Color.black; }

        if (audioSource != null && sfx != null) { audioSource.PlayOneShot(sfx); yield return new WaitForSecondsRealtime(sfx.length + 0.5f); }
        else { yield return new WaitForSecondsRealtime(1f); }

        textEndingJudul.text = judul;
        textEndingNarasi.text = narasi;

        yield return new WaitForSecondsRealtime(1.5f);

        if (tombolLanjutkan != null) tombolLanjutkan.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void KlikLanjutkanKeSompral()
    {
        if (tombolLanjutkan != null) tombolLanjutkan.SetActive(false);
        if (endingPanel != null) endingPanel.gameObject.SetActive(false);

        if (textLaporanSompral != null && SompralManager.instance != null) { textLaporanSompral.text = SompralManager.instance.DapatkanLaporanSompral(); }
        if (panelLaporanSompral != null) { panelLaporanSompral.alpha = 1f; panelLaporanSompral.gameObject.SetActive(true); }
        if (tombolMainMenu != null) tombolMainMenu.SetActive(true);
    }

    public void KlikMainMenu()
    {
        Time.timeScale = 1f; Application.targetFrameRate = -1; SceneManager.LoadScene(namaSceneMainMenu);
    }
}