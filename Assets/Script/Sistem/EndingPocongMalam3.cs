using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndingPocongMalam3 : MonoBehaviour
{
    [Header("Referensi Environment & Pocong")]
    public Transform kameraPlayer;
    public Transform titikKasur;
    public GameObject pocongMalam3;
    public Collider colliderPocong;
    [Tooltip("Buat objek kosong di depan wajah pocong")]
    public Transform targetWajahPocong; 

    [Header("Pengaturan Cahaya (Khusus Ending)")]
    public GameObject lampuKhususEnding; 
    public GameObject senterPemain;

    [Header("Referensi UI & Audio")]
    public EndingTrigger triggerEnding; 
    public AudioSource sfxJumpscare;

    private bool sedangMengintai = false;
    private float rotasiX;
    private float rotasiY;

    // Fungsi ini dipanggil dari BedInteract.cs
    public void MulaiEnding()
    {
        StartCoroutine(ProsesAwalEnding());
    }

    private IEnumerator ProsesAwalEnding()
    {
        // 1. Matikan kontrol pemain secara paksa
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        if (interact != null) interact.enabled = false; 
        if (player != null)
        {
            player.canMove = false;
            player.enabled = false; 
        }

        // 2. Fade Out ke Hitam menggunakan sistem NightManager
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            
            float t = 0;
            while (t < 1f) { t += Time.deltaTime * 1.5f; layarHitam.alpha = t; yield return null; }
            layarHitam.alpha = 1f;

            // 3. Jeda saat layar hitam (tidur lebih lama)
            yield return new WaitForSeconds(3f); 

            // 4. Teleport dan Ubah Cahaya
            kameraPlayer.parent.position = titikKasur.position;
            kameraPlayer.parent.rotation = titikKasur.rotation; 
            
            rotasiX = titikKasur.rotation.eulerAngles.x;
            rotasiY = titikKasur.rotation.eulerAngles.y;
            kameraPlayer.localRotation = Quaternion.Euler(rotasiX, 0, 0);

            pocongMalam3.SetActive(true);

            // Nyalakan lampu khusus & matikan senter
            if (lampuKhususEnding != null) lampuKhususEnding.SetActive(true);
            if (senterPemain != null) senterPemain.SetActive(false);

            // 5. Fade In (Melek perlahan)
            t = 0;
            while (t < 1f) { t += Time.deltaTime * 1.0f; layarHitam.alpha = 1f - t; yield return null; }
            layarHitam.alpha = 0f;
            layarHitam.gameObject.SetActive(false);
        }

        sedangMengintai = true;
    }

    private void Update()
    {
        if (sedangMengintai)
        {
            // Kontrol menoleh mandiri
            if (Mouse.current != null)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                rotasiX -= mouseDelta.y * 0.1f; 
                rotasiY += mouseDelta.x * 0.1f;
                rotasiX = Mathf.Clamp(rotasiX, -60f, 60f); 
                kameraPlayer.localRotation = Quaternion.Euler(rotasiX, 0, 0);
                kameraPlayer.parent.rotation = Quaternion.Euler(0, rotasiY, 0);
            }

            // Deteksi tatapan ke pocong
            RaycastHit hit;
            if (Physics.Raycast(kameraPlayer.position, kameraPlayer.forward, out hit, 10f))
            {
                if (hit.collider == colliderPocong)
                {
                    EksekusiJumpscare();
                }
            }
        }
    }

    private void EksekusiJumpscare()
    {
        sedangMengintai = false;
        StartCoroutine(ProsesJumpscare());
    }

    private IEnumerator ProsesJumpscare()
    {
        if (sfxJumpscare != null) sfxJumpscare.Play();

        // 1. Animasi menoleh cepat ke wajah pocong
        Quaternion rotasiAwal = kameraPlayer.rotation;
        Vector3 titikTatapan = targetWajahPocong != null ? targetWajahPocong.position : pocongMalam3.transform.position + Vector3.up * 1.5f;
        Vector3 arahTatapan = titikTatapan - kameraPlayer.position;
        Quaternion rotasiTarget = Quaternion.LookRotation(arahTatapan);

        float waktu = 0;
        float durasiSmooth = 0.15f; 
        while (waktu < durasiSmooth)
        {
            waktu += Time.deltaTime;
            kameraPlayer.rotation = Quaternion.Slerp(rotasiAwal, rotasiTarget, waktu / durasiSmooth);
            yield return null;
        }
        kameraPlayer.rotation = rotasiTarget; 

        // 2. Freeze Frame
        yield return new WaitForSeconds(0.4f);

        // 3. Layar mati instan
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            NightManager.instance.panelTransisiHitam.gameObject.SetActive(true);
            NightManager.instance.panelTransisiHitam.alpha = 1f; 
            NightManager.instance.panelTransisiHitam.blocksRaycasts = true; 
        }

        // 4. Munculkan Panel Ending melalui EndingTrigger
        yield return new WaitForSeconds(1.5f);
        if (triggerEnding != null)
        {
            triggerEnding.PicunyaEnding();
        }
    }
}