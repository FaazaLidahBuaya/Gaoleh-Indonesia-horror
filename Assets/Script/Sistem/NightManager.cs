using UnityEngine;
using TMPro;
using System.Collections;

public class NightManager : MonoBehaviour
{
    public static NightManager instance;

    [Header("Status Malam")]
    [Range(1, 3)] public int malamSekarang = 1;

    [Header("Sistem Transisi Layar")]
    public CanvasGroup panelTransisiHitam; 
    public TextMeshProUGUI teksMalam;      
    public float durasiFade = 1f;          
    public float tahanLayarHitam = 2.5f;   

    [Header("Referensi Objek Lingkungan")]
    public Camera mainCamera;             
    public GameObject boxPenghalangMalam1; 
    public GameObject pintuKamarKakak;    

    [Header("BGM / Musik Latar Game")]
    public AudioSource bgmAudioSource;
    public AudioClip bgmMalam1;
    public AudioClip bgmMalam2; // (Opsional) BGM kalau ganti malam 2
    public AudioClip bgmMalam3; // (Opsional) BGM kalau ganti malam 3

    [Header("Sistem Kelistrikan Rumah")]
    public GameObject[] semuaLampuRumah; 

    [Header("Tema Malam 1: Sore (Kuning/Oranye)")]
    public Color ambientSore = new Color(0.6f, 0.4f, 0.3f);
    public Color warnaFogSore = new Color(0.5f, 0.3f, 0.2f);
    public Color bgKameraSore = new Color(0.5f, 0.3f, 0.2f);
    public float ketebalanFogSore = 0.02f;

    [Header("Tema Malam 2: Malam (Biru Gelap)")]
    public Color ambientMalam = new Color(0.1f, 0.15f, 0.25f);
    public Color warnaFogMalam = new Color(0.05f, 0.08f, 0.15f);
    public Color bgKameraMalam = new Color(0.05f, 0.08f, 0.15f);
    public float ketebalanFogMalam = 0.05f;

    [Header("Tema Malam 3: Puncak Teror (Hitam Pekat)")]
    public Color ambientMalam3 = new Color(0.02f, 0.02f, 0.02f);
    public Color warnaFogMalam3 = new Color(0f, 0f, 0f);
    public Color bgKameraMalam3 = new Color(0f, 0f, 0f);
    public float ketebalanFogMalam3 = 0.08f;

    [Header("Koneksi ke Sistem Teror")]
    public TerrorMessageManager terrorManager;

    private bool sedangTransisi = false; 

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared; 
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat; 

        if (teksMalam != null) teksMalam.text = "";

        if (panelTransisiHitam != null)
        {
            panelTransisiHitam.gameObject.SetActive(true);
            panelTransisiHitam.alpha = 1f; 
        }

        SetUpLingkunganBerdasarkanMalam();
        StartCoroutine(ProsesMulaiGame());
    }

    public void MajuMalam()
    {
        if (sedangTransisi) return; 

        if (malamSekarang < 3)
        {
            StartCoroutine(ProsesGantiMalam(malamSekarang + 1));
        }
    }

    private IEnumerator ProsesMulaiGame()
    {
        sedangTransisi = true;

        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        if (player != null) 
        {
            player.canMove = false;
            player.enabled = false; 
            AudioSource audioPlayer = player.GetComponent<AudioSource>();
            if (audioPlayer != null) audioPlayer.Stop(); 
        }
        
        if (interact != null)
        {
            interact.KlikBatal(); 
            interact.enabled = false; 
        }

        if (teksMalam != null) teksMalam.text = "MALAM " + malamSekarang;

        // BGM MULAI BERSAMAAN DENGAN MUNCULNYA TEKS "MALAM 1"
        if (bgmAudioSource != null && bgmMalam1 != null)
        {
            bgmAudioSource.clip = bgmMalam1;
            bgmAudioSource.loop = false;
            bgmAudioSource.Play();
        }

        yield return new WaitForSeconds(tahanLayarHitam);
        if (teksMalam != null) teksMalam.text = "";

        if (panelTransisiHitam != null)
        {
            float timer = 0;
            while (timer < durasiFade)
            {
                timer += Time.deltaTime;
                panelTransisiHitam.alpha = Mathf.Lerp(1f, 0f, timer / durasiFade);
                yield return null;
            }
            panelTransisiHitam.alpha = 0f;
            panelTransisiHitam.gameObject.SetActive(false); 
        }

        if (player != null) 
        {
            player.enabled = true;
            player.canMove = true;
        }
        
        if (interact != null) interact.enabled = true;
        if (ControlIndicatorUI.instance != null) ControlIndicatorUI.instance.UpdateFlashlightUI();

        sedangTransisi = false;
    }

    private IEnumerator ProsesGantiMalam(int nomorMalamBerikutnya)
    {
        sedangTransisi = true;

        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        if (player != null) 
        {
            player.canMove = false;
            player.enabled = false; 
            AudioSource audioPlayer = player.GetComponent<AudioSource>();
            if (audioPlayer != null) audioPlayer.Stop(); 
        }
        
        if (interact != null)
        {
            interact.KlikBatal();
            interact.enabled = false; 
        }

        if (panelTransisiHitam != null)
        {
            panelTransisiHitam.gameObject.SetActive(true);
            float timer = 0;
            while (timer < durasiFade)
            {
                timer += Time.deltaTime;
                panelTransisiHitam.alpha = Mathf.Lerp(0f, 1f, timer / durasiFade);
                yield return null;
            }
            panelTransisiHitam.alpha = 1f;
        }

        malamSekarang = nomorMalamBerikutnya;
        SetUpLingkunganBerdasarkanMalam();

        // GANTI BGM SESUAI MALAM (Jika Disediakan)
        if (bgmAudioSource != null)
        {
            if (malamSekarang == 2 && bgmMalam2 != null) { bgmAudioSource.clip = bgmMalam2; bgmAudioSource.Play(); }
            else if (malamSekarang == 3 && bgmMalam3 != null) { bgmAudioSource.clip = bgmMalam3; bgmAudioSource.Play(); }
        }

        if (ControlIndicatorUI.instance != null) ControlIndicatorUI.instance.UpdateFlashlightUI();
        
        if (teksMalam != null) teksMalam.text = "MALAM " + malamSekarang;
        yield return new WaitForSeconds(tahanLayarHitam);
        if (teksMalam != null) teksMalam.text = "";

        if (panelTransisiHitam != null)
        {
            float timer = 0;
            while (timer < durasiFade)
            {
                timer += Time.deltaTime;
                panelTransisiHitam.alpha = Mathf.Lerp(1f, 0f, timer / durasiFade);
                yield return null;
            }
            panelTransisiHitam.alpha = 0f;
            panelTransisiHitam.gameObject.SetActive(false);
        }

        if (player != null) 
        {
            player.enabled = true;
            player.canMove = true;
        }
        
        if (interact != null) interact.enabled = true;

        sedangTransisi = false;
    }

    private void SetUpLingkunganBerdasarkanMalam()
    {
        switch (malamSekarang)
        {
            case 1: AturMalam1(); break;
            case 2: AturMalam2(); break;
            case 3: AturMalam3(); break;
        }
        DynamicGI.UpdateEnvironment(); 
    }

    void AturMalam1()
    {
        RenderSettings.ambientLight = ambientSore; RenderSettings.fogColor = warnaFogSore; RenderSettings.fogDensity = ketebalanFogSore; if (mainCamera != null) mainCamera.backgroundColor = bgKameraSore;
        if (boxPenghalangMalam1 != null) boxPenghalangMalam1.SetActive(true); if (pintuKamarKakak != null) pintuKamarKakak.SetActive(true); 
        if (terrorManager != null) { terrorManager.jedaMinimalPesan = 180f; terrorManager.jedaMaksimalPesan = 360f; }
        if (StoryMessageManager.instance != null) StoryMessageManager.instance.JalankanCeritaMalam(1);
    }

    void AturMalam2()
    {
        RenderSettings.ambientLight = ambientMalam; RenderSettings.fogColor = warnaFogMalam; RenderSettings.fogDensity = ketebalanFogMalam; if (mainCamera != null) mainCamera.backgroundColor = bgKameraMalam;
        if (boxPenghalangMalam1 != null) boxPenghalangMalam1.SetActive(false);
        if (terrorManager != null) { terrorManager.jedaMinimalPesan = 60f; terrorManager.jedaMaksimalPesan = 180f; }
        if (StoryMessageManager.instance != null) StoryMessageManager.instance.JalankanCeritaMalam(2);
    }

    void AturMalam3()
    {
        RenderSettings.ambientLight = ambientMalam3; RenderSettings.fogColor = warnaFogMalam3; RenderSettings.fogDensity = ketebalanFogMalam3; if (mainCamera != null) mainCamera.backgroundColor = bgKameraMalam3;
        if (boxPenghalangMalam1 != null) boxPenghalangMalam1.SetActive(false);
        if (terrorManager != null) { terrorManager.jedaMinimalPesan = 20f; terrorManager.jedaMaksimalPesan = 60f; }
        if (StoryMessageManager.instance != null) StoryMessageManager.instance.JalankanCeritaMalam(3);

        foreach (GameObject lampu in semuaLampuRumah)
        {
            if (lampu != null) lampu.SetActive(false);
        }
    }

    public void MentrikBukaKamarKakak()
    {
        if (malamSekarang == 3)
        {
            if (pintuKamarKakak != null) { pintuKamarKakak.SetActive(false); if (PhoneSystem.instance != null) PhoneSystem.instance.TerimaPesanMasuk("???", "Pintu kamarnya sudah tidak dikunci lagi. Masuklah."); }
        }
    }
}