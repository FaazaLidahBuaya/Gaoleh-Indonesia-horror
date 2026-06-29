using UnityEngine;
using System.Collections;

[System.Serializable]
public class KategoriAudio
{
    [Tooltip("Hanya untuk penamaan di Inspector agar rapi")]
    public string namaKategori = "Nama Gangguan Baru"; 
    public AudioClip[] daftarSuara;
    [Tooltip("Seberapa jauh jarak suara ini muncul dari pemain?")]
    public float jarakMuncul = 10f;
}

public class SistemGangguan : MonoBehaviour
{
    public static SistemGangguan instance;

    [Header("Batas Poin Sompral (Penentu Tingkat Gangguan)")]
    public int batasLevel1 = 15;
    public int batasLevel2 = 30;
    public int batasLevel3 = 50;

    [Header("Frekuensi Audio (Suara & Benda Jatuh)")]
    public float jedaMinLevel0 = 50f; public float jedaMaxLevel0 = 70f; 
    public float jedaMinLevel1 = 30f; public float jedaMaxLevel1 = 45f; 
    public float jedaMinLevel2 = 15f; public float jedaMaxLevel2 = 25f; 
    public float jedaMinLevel3 = 5f;  public float jedaMaxLevel3 = 12f; 

    [Header("Frekuensi Visual (Lampu/Senter Kedip)")]
    public float visualMinMalam1 = 40f; public float visualMaxMalam1 = 60f;
    public float visualMinMalam2 = 25f; public float visualMaxMalam2 = 45f;
    public float visualMinMalam3 = 10f; public float visualMaxMalam3 = 20f;

    [Header("Referensi Objek")]
    public Transform kameraPlayer;
    public GameObject senterPlayer;
    public Light[] lampuRumah;
    public GameObject pocongDepanMuka;

    [Header("Pengaturan Posisi Jumpscare (Pocong Muka)")]
    [Tooltip("Karena pivot pocong di kaki, turunkan posisinya agar wajah pocong pas di depan kamera")]
    public float koreksiTinggiPocong = 1.6f;
    [Tooltip("Ubah rotasi jika pocong menghadap samping/belakang")]
    public Vector3 koreksiRotasiPocong = new Vector3(0, -90f, 0);

    [Header("Pengaturan Audio")]
    public KategoriAudio[] audioMalam1;
    public KategoriAudio[] audioMalam2;
    public KategoriAudio[] audioMalam3;
    public AudioClip sfxJumpscareMuka;

    private bool jumpscareMukaSelesai = false; 
    private AudioClip klipTerakhirDiputar;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SiklusAudio());
        StartCoroutine(SiklusVisual());
    }

    // ====================================================================
    // MESIN 1: SIKLUS TEROR AUDIO
    // ====================================================================
    private float DapatkanWaktuAcak(int poin)
    {
        if (poin >= batasLevel3) return Random.Range(jedaMinLevel3, jedaMaxLevel3);
        if (poin >= batasLevel2) return Random.Range(jedaMinLevel2, jedaMaxLevel2);
        if (poin >= batasLevel1) return Random.Range(jedaMinLevel1, jedaMaxLevel1);
        return Random.Range(jedaMinLevel0, jedaMaxLevel0);
    }

    private IEnumerator SiklusAudio()
    {
        yield return new WaitForSeconds(10f);

        while (true)
        {
            if (SompralManager.instance == null || NightManager.instance == null) 
            {
                yield return new WaitForSeconds(5f);
                continue;
            }

            int poinTracker = SompralManager.instance.totalPoinSompral;
            float sisaWaktu = DapatkanWaktuAcak(poinTracker);

            while (sisaWaktu > 0)
            {
                int poinSekarang = SompralManager.instance.totalPoinSompral;
                if (poinSekarang > poinTracker)
                {
                    poinTracker = poinSekarang;
                    float jedaBaru = DapatkanWaktuAcak(poinTracker);
                    if (sisaWaktu > jedaBaru) sisaWaktu = jedaBaru;
                }

                sisaWaktu -= Time.deltaTime;
                yield return null;
            }

            int malam = NightManager.instance.malamSekarang;
            switch (malam)
            {
                case 1: if (audioMalam1.Length > 0) MainkanAudioAcak(audioMalam1[Random.Range(0, audioMalam1.Length)]); break;
                case 2: if (audioMalam2.Length > 0) MainkanAudioAcak(audioMalam2[Random.Range(0, audioMalam2.Length)]); break;
                case 3: 
                    // Jumpscare dipicu di sini
                    if (poinTracker >= batasLevel3 && !jumpscareMukaSelesai) { StartCoroutine(JumpscareDepanMuka()); }
                    else if (audioMalam3.Length > 0) MainkanAudioAcak(audioMalam3[Random.Range(0, audioMalam3.Length)]); 
                    break;
            }
        }
    }

    // ====================================================================
    // MESIN 2: SIKLUS TEROR VISUAL (LAMPU & SENTER) 
    // ====================================================================
    private IEnumerator SiklusVisual()
    {
        yield return new WaitForSeconds(15f);

        while (true)
        {
            if (SompralManager.instance == null || NightManager.instance == null) 
            {
                yield return new WaitForSeconds(5f);
                continue; 
            }

            int poinTracker = SompralManager.instance.totalPoinSompral;
            int malam = NightManager.instance.malamSekarang;
            
            float jedaMax = visualMaxMalam1;
            float jedaMin = visualMinMalam1;
            
            if (malam == 2) { jedaMin = visualMinMalam2; jedaMax = visualMaxMalam2; }
            else if (malam == 3) { jedaMin = visualMinMalam3; jedaMax = visualMaxMalam3; }
            
            if (poinTracker >= batasLevel3) { jedaMin *= 0.5f; jedaMax *= 0.5f; }
            else if (poinTracker >= batasLevel2) { jedaMin *= 0.75f; jedaMax *= 0.75f; }

            float sisaWaktu = Random.Range(jedaMin, jedaMax);

            while (sisaWaktu > 0)
            {
                sisaWaktu -= Time.deltaTime;
                yield return null;
            }

            if (malam == 1)
            {
                StartCoroutine(KedipkanLampuRumah(Random.Range(2, 4), 0.2f)); 
            }
            else if (malam == 2)
            {
                StartCoroutine(KedipkanLampuRumah(Random.Range(3, 7), 0.15f)); 
            }
            else if (malam == 3)
            {
                StartCoroutine(KedipkanSenter(Random.Range(8, 15), 0.05f)); 
            }
        }
    }

    // ====================================================================
    // FUNGSI PEMBANTU
    // ====================================================================

    private Light DapatkanLampuTerdekat()
    {
        if (lampuRumah.Length == 0 || kameraPlayer == null) return null;

        Light lampuTerdekat = null;
        float jarakTerdekat = Mathf.Infinity;

        foreach (Light lampu in lampuRumah)
        {
            if (lampu == null || !lampu.gameObject.activeInHierarchy) continue;

            float jarak = Vector3.Distance(lampu.transform.position, kameraPlayer.position);
            
            if (jarak < jarakTerdekat)
            {
                jarakTerdekat = jarak;
                lampuTerdekat = lampu;
            }
        }

        return lampuTerdekat;
    }

    private IEnumerator KedipkanLampuRumah(int jumlahKedip, float kecepatan)
    {
        Light lampuTarget = DapatkanLampuTerdekat();
        
        if (lampuTarget == null) yield break;

        bool statusAwal = lampuTarget.enabled;

        for (int i = 0; i < jumlahKedip; i++)
        {
            lampuTarget.enabled = !lampuTarget.enabled;
            yield return new WaitForSeconds(kecepatan);
        }

        lampuTarget.enabled = statusAwal; 
    }

    private IEnumerator KedipkanSenter(int jumlahKedip, float kecepatan)
    {
        if (senterPlayer == null || !senterPlayer.activeSelf) yield break;

        for (int i = 0; i < jumlahKedip; i++)
        {
            senterPlayer.SetActive(!senterPlayer.activeSelf);
            yield return new WaitForSeconds(kecepatan + Random.Range(0f, 0.05f)); 
        }
        
        senterPlayer.SetActive(true); 
    }

    private void MainkanAudioAcak(KategoriAudio kat)
    {
        if (kat.daftarSuara == null || kat.daftarSuara.Length == 0 || kameraPlayer == null) return;

        AudioClip klipDipilih = kat.daftarSuara[0];

        if (kat.daftarSuara.Length > 1)
        {
            do
            {
                int indexAcak = Random.Range(0, kat.daftarSuara.Length);
                klipDipilih = kat.daftarSuara[indexAcak];
            } 
            while (klipDipilih == klipTerakhirDiputar); 
        }

        klipTerakhirDiputar = klipDipilih;

        Vector2 lingkaranAcak = Random.insideUnitCircle.normalized * kat.jarakMuncul;
        Vector3 titikSuara = kameraPlayer.position + new Vector3(lingkaranAcak.x, 0, lingkaranAcak.y);

        GameObject tempAudio = new GameObject("Teror_3D_" + kat.namaKategori);
        tempAudio.transform.position = titikSuara;
        AudioSource src = tempAudio.AddComponent<AudioSource>();
        
        src.clip = klipDipilih;
        src.spatialBlend = 1.0f; 
        src.rolloffMode = AudioRolloffMode.Logarithmic; 
        src.minDistance = 1f;
        src.maxDistance = kat.jarakMuncul + 15f; 
        
        src.Play();
        Destroy(tempAudio, klipDipilih.length);
    }

    private IEnumerator JumpscareDepanMuka()
    {
        jumpscareMukaSelesai = true;
        
        if (pocongDepanMuka != null && kameraPlayer != null)
        {
            // 1. Tentukan posisi dan rotasi pocong
            Vector3 posisiMuka = kameraPlayer.position + kameraPlayer.forward * 1.2f;
            posisiMuka.y -= koreksiTinggiPocong;
            pocongDepanMuka.transform.position = posisiMuka;
            
            Vector3 arahTatapan = kameraPlayer.position - pocongDepanMuka.transform.position;
            arahTatapan.y = 0; 
            if (arahTatapan != Vector3.zero)
            {
                Quaternion rotasiDasar = Quaternion.LookRotation(arahTatapan);
                pocongDepanMuka.transform.rotation = rotasiDasar * Quaternion.Euler(koreksiRotasiPocong);
            }

            // 2. MUNCULKAN POCONG & PUTAR SUARA DULU!
            pocongDepanMuka.SetActive(true);
            if (sfxJumpscareMuka != null)
            {
                AudioSource.PlayClipAtPoint(sfxJumpscareMuka, kameraPlayer.position);
            }

            // 3. JEDA UNTUK PEMAIN MELIHAT POCONG (0.4 Detik)
            // Di sinilah pemain akan kaget melihat pocong sebelum layar mati
            yield return new WaitForSeconds(0.4f);

            // 4. LAYAR MATI INSTAN
            if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
            {
                NightManager.instance.panelTransisiHitam.gameObject.SetActive(true);
                NightManager.instance.panelTransisiHitam.alpha = 1f;
            }

            // 5. HILANGKAN POCONG SAAT LAYAR SUDAH GELAP
            pocongDepanMuka.SetActive(false);
            
            // 6. HENING SEJENAK DI KEGELAPAN (1.5 detik)
            yield return new WaitForSeconds(1.5f);

            // 7. LAYAR KEMBALI TERANG PERLAHAN (FADE IN)
            if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
            {
                float fadeTimer = 0f;
                while (fadeTimer < 1f)
                {
                    fadeTimer += Time.deltaTime;
                    NightManager.instance.panelTransisiHitam.alpha = 1f - fadeTimer;
                    yield return null;
                }
                NightManager.instance.panelTransisiHitam.alpha = 0f;
                NightManager.instance.panelTransisiHitam.gameObject.SetActive(false);
            }
        }
    }
}