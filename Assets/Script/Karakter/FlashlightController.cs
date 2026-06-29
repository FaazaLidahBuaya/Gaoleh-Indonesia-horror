using UnityEngine;
using UnityEngine.InputSystem; // Wajib karena kita pakai New Input System seperti di HP

public class FlashlightController : MonoBehaviour
{
    [Header("Referensi Komponen Senter")]
    [Tooltip("Tarik objek Spotlight Senter_Player yang ada di bawah Main Camera ke sini")]
    public GameObject senterObject; 

    [Header("Audio SFX")]
    public AudioSource audioSource;
    public AudioClip sfxKlikSenter; // Suara 'Ceklek!' saklar senter

    private bool isSenterOn = false;

    void Start()
    {
        // Pastikan senter mati total saat game baru dimulai (Malam 1)
        if (senterObject != null)
        {
            senterObject.SetActive(false);
        }
    }

    void Update()
    {
        // ====================================================================
        // SYARAT MUTLAK: Senter HANYA bisa hidup jika game sudah masuk MALAM 3
        // ====================================================================
        if (NightManager.instance == null || NightManager.instance.malamSekarang != 3)
        {
            // Jika belum Malam 3, paksa senter tetap mati (mencegah cheat tombol F)
            if (senterObject != null && senterObject.activeSelf)
            {
                senterObject.SetActive(false);
                isSenterOn = false;
            }
            return; // Berhenti di sini, kode di bawah tidak akan dieksekusi
        }

        // Jika sudah Malam 3, izinkan player menekan tombol F
        if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
        {
            ToggleSenter();
        }
    }

    void ToggleSenter()
    {
        if (senterObject == null) return;

        // Balikkan status on/off
        isSenterOn = !isSenterOn;
        
        // Hidupkan/matikan lampu di Unity
        senterObject.SetActive(isSenterOn);

        // Bunyikan suara saklar klik senter
        if (audioSource != null && sfxKlikSenter != null)
        {
            audioSource.PlayOneShot(sfxKlikSenter);
        }
    }
}