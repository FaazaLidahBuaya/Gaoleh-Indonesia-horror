using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;

public class BedInteract : MonoBehaviour 
{
    [Header("Sistem Tidur Normal")]
    public UnityEvent eventEndingKetiduran; 

    // ---> TAMBAHKAN KODE INI DI SINI <---
    [Header("Sistem Tidur Sompral (Malam 3)")]
    [Tooltip("Picu Ending Pocong jika poin sompral lebih dari batas ini")]
    public int batasSompralTidur = 35;
    public UnityEvent eventEndingTidurPocong;
    // -----------------------------------

    [Header("Sistem Sembunyi (Batas Poin)")]
    [Tooltip("Jika poin sompral DI ATAS angka ini, akan memicu Pocong. Jika DI BAWAH/SAMA DENGAN angka ini, sembunyi aman.")]
    public int batasPoinSompral = 30;

    [Header("CABANG ENDING SEMBUNYI")]
    [Tooltip("Picu Ending Trigger versi Seram (Pocong Jumpscare)")]
    public UnityEvent eventEndingSembunyiSompral;
    [Tooltip("Picu Ending Trigger versi Aman (Tidak terjadi apa-apa, langsung panel)")]
    public UnityEvent eventEndingSembunyiAman;

    [Header("Referensi Sutradara Sembunyi (Wajib diisi untuk kedua rute)")]
    public Transform titikKolongKasur;

    [Header("Referensi Sutradara Khusus Rute Sompral")]
    public SimpleDoor pintuKamar; 
    public GameObject pocongJumpscare;
    public Transform titikPintuKamar;
    public Transform titikDepanMuka;

    [Header("Koreksi Model Pocong")]
    public Vector3 koreksiRotasiPocong = new Vector3(0, 0, 0);

    [Header("Pengaturan Lompatan Pocong")]
    public float durasiSatuLompatan = 1.8f;
    public float tinggiLompatan = 0.25f;
    public float jedaAntarLompatan = 2.5f;

    [Header("Audio")]
    public AudioClip sfxPocongLompat; 

    private bool sudahDiklik = false;

    public void GunakanKasur()
    {
        if (sudahDiklik) return;
        if (NightManager.instance == null) return;
        int malamIni = NightManager.instance.malamSekarang;

        // Ambil poin sompral pemain saat ini
        int poinSekarang = 0;
        if (SompralManager.instance != null) poinSekarang = SompralManager.instance.totalPoinSompral;

        if (malamIni < 3)
        {
            sudahDiklik = true;
            NightManager.instance.MajuMalam(); 
            Invoke("BukaKunciKasur", 5f);
        }
        else if (malamIni == 3)
        {
            sudahDiklik = true;
            InteractableObject interactObj = GetComponent<InteractableObject>();
            if (interactObj != null) interactObj.enabled = false;

            // CEK CABANG ENDING: Apakah pemain sompral?
            if (poinSekarang > batasSompralTidur)
            {
                Debug.Log($"Poin Sompral ({poinSekarang}) > {batasSompralTidur}. Panggil Ending Pocong!");
                if (eventEndingTidurPocong != null) eventEndingTidurPocong.Invoke();
            }
            else
            {
                Debug.Log($"Poin Sompral ({poinSekarang}) aman. Panggil Ending Ketiduran.");
                if (eventEndingKetiduran != null) eventEndingKetiduran.Invoke();
            }
        }
    }

    public void Sembunyi()
    {
        if (sudahDiklik) return;

        int poinSekarang = 0;
        if (SompralManager.instance != null) poinSekarang = SompralManager.instance.totalPoinSompral;

        sudahDiklik = true;
        InteractableObject interactObj = GetComponent<InteractableObject>();
        if (interactObj != null) interactObj.enabled = false;

        // PENGECEKAN CABANG
        if (poinSekarang > batasPoinSompral)
        {
            StartCoroutine(ProsesEndingSembunyiSompral());
        }
        else
        {
            // SEKARANG PAKE COROUTINE JUGA BIAR BISA NGUMPET DULU
            StartCoroutine(ProsesEndingSembunyiAman());
        }
    }

    private void BukaKunciKasur() { sudahDiklik = false; }

    // --- FUNGSI BANTUAN UNTUK PERSIAPAN NGUMPET (Dipakai kedua rute) ---
    private IEnumerator MatikanKontrolDanFadeOut()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        if (player != null) 
        { 
            player.canMove = false; 
            player.enabled = false; 
            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false; 
        }
        if (interact != null) interact.enabled = false;

        // Fade Out pelan
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            float timer = 0;
            while (timer < 1f) { timer += Time.deltaTime; layarHitam.alpha = timer; yield return null; }
            layarHitam.alpha = 1f;
        }
    }

    private void PindahkanKeKolong()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null && titikKolongKasur != null)
        {
            // Trik Y-minus agar kamera pas di lantai
            player.transform.position = titikKolongKasur.position;
            player.transform.rotation = titikKolongKasur.rotation;
            if (Camera.main != null) Camera.main.transform.localRotation = Quaternion.identity;
        }
    }

    private IEnumerator FaseWaspada(float durasi)
    {
        // Fade In di kolong
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
            float timer = 0;
            while (timer < 1f) { timer += Time.deltaTime; layarHitam.alpha = 1f - timer; yield return null; }
            layarHitam.alpha = 0f;
        }

        // Cek menoleh kanan kiri
        float lookTimer = 0f;
        float currentRotY = 0f;
        while (lookTimer < durasi)
        {
            lookTimer += Time.deltaTime;
            if (Mouse.current != null && Camera.main != null)
            {
                float mouseX = Mouse.current.delta.ReadValue().x * 0.05f; 
                currentRotY += mouseX;
                currentRotY = Mathf.Clamp(currentRotY, -60f, 60f); 
                Camera.main.transform.localRotation = Quaternion.Euler(0, currentRotY, 0);
            }
            yield return null;
        }
    }
    // ------------------------------------------------------------------


    // ==============================================================
    // RUTE 1: CUTSCENE SOMPRAL (POCONG) - Sedikit dirapikan
    // ==============================================================
    private IEnumerator ProsesEndingSembunyiSompral()
    {
        yield return StartCoroutine(MatikanKontrolDanFadeOut());
        PindahkanKeKolong();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FaseWaspada(4f)); // Waspada 4 detik

        // Mulai Horror
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
            float timer = 0;
            while (timer < 0.5f) { timer += Time.deltaTime * 2f; layarHitam.alpha = timer; yield return null; }
            layarHitam.alpha = 1f;
        }

        if (Camera.main != null) Camera.main.transform.localRotation = Quaternion.identity;
        yield return new WaitForSeconds(1f);

        if (pintuKamar != null) pintuKamar.BukaPaksa();
        if (pocongJumpscare != null && titikPintuKamar != null)
        {
            pocongJumpscare.transform.position = titikPintuKamar.position;
            ArahkanPocongKePemain();
            pocongJumpscare.SetActive(true);
        }
        
        yield return new WaitForSeconds(1.5f);

        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
            float timer = 0;
            while (timer < 1f) { timer += Time.deltaTime; layarHitam.alpha = 1f - timer; yield return null; }
            layarHitam.alpha = 0f;
            layarHitam.gameObject.SetActive(false); 
        }

        yield return new WaitForSeconds(1f); 

        if (pocongJumpscare != null && titikPintuKamar != null && titikDepanMuka != null)
        {
            Vector3 titikTengah = Vector3.Lerp(titikPintuKamar.position, titikDepanMuka.position, 0.5f);
            yield return StartCoroutine(SatuLompatan(titikPintuKamar.position, titikTengah));
            yield return new WaitForSeconds(jedaAntarLompatan);
            yield return StartCoroutine(SatuLompatan(titikTengah, titikDepanMuka.position));
        }

        yield return new WaitForSeconds(2.0f);
        if (eventEndingSembunyiSompral != null) eventEndingSembunyiSompral.Invoke();
    }

    // ==============================================================
    // RUTE 2: AMAN / TIDAK SOMPRAL (REVISI: NGUMPET DULU)
    // ==============================================================
    private IEnumerator ProsesEndingSembunyiAman()
    {
        // 1. Matikan kontrol, layar hitam pelan
        yield return StartCoroutine(MatikanKontrolDanFadeOut());

        // 2. Pindahkan Arya ke bawah kasur (Y-minus tanah)
        PindahkanKeKolong();

        yield return new WaitForSeconds(0.5f); // Jeda suspense di kegelapan

        // 3. Layar menyala, fase waspada menoleh kanan-kiri (Beri durasi agak lama biar tegang)
        yield return StartCoroutine(FaseWaspada(6f)); // Waspada 6 detik menanti hantu

        // 4. Setelah 6 detik menengok dan ternyata tetap aman...
        // Mata Arya dipaksa lurus kedepan lagi
        if (Camera.main != null) Camera.main.transform.localRotation = Quaternion.identity;

        yield return new WaitForSeconds(1f); // Jeda tenang sebelum pingsan/tidur

        // 5. Fade out layar ke hitam (Arya ketiduran/aman)
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            float timer = 0;
            while (timer < 2f) { timer += Time.deltaTime; layarHitam.alpha = timer; yield return null; } // Fade out lambat (2 detik)
            layarHitam.alpha = 1f;
        }

        yield return new WaitForSeconds(1.5f); // Jeda tenang di kegelapan akhir

        // 6. Picu Ending Trigger Aman!
        if (eventEndingSembunyiAman != null)
        {
            eventEndingSembunyiAman.Invoke();
        }
    }

    private IEnumerator SatuLompatan(Vector3 startPos, Vector3 endPos)
    {
        float timer = 0f;
        while (timer < durasiSatuLompatan)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / durasiSatuLompatan);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Vector3 basePos = Vector3.Lerp(startPos, endPos, smoothT);
            
            float jumpCurve = Mathf.Abs(Mathf.Sin(t * Mathf.PI));
            float height = Mathf.Pow(jumpCurve, 1.2f) * tinggiLompatan;
            
            pocongJumpscare.transform.position = basePos + new Vector3(0, height, 0);
            ArahkanPocongKePemain();

            yield return null;
        }

        pocongJumpscare.transform.position = endPos;
        ArahkanPocongKePemain();

        if (sfxPocongLompat != null) 
            AudioSource.PlayClipAtPoint(sfxPocongLompat, pocongJumpscare.transform.position);
    }

    private void ArahkanPocongKePemain()
    {
        if (Camera.main != null && pocongJumpscare != null)
        {
            Vector3 dir = (Camera.main.transform.position - pocongJumpscare.transform.position).normalized;
            dir.y = 0; 
            if (dir != Vector3.zero) 
            {
                Quaternion rotasiDasar = Quaternion.LookRotation(dir);
                pocongJumpscare.transform.rotation = rotasiDasar * Quaternion.Euler(koreksiRotasiPocong);
            }
        }
    }
}