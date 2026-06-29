using UnityEngine;

[RequireComponent(typeof(AudioSource))] // Otomatis menambahkan AudioSource jika belum ada
public class LightSwitch : MonoBehaviour
{
    [Header("Pengaturan Saklar")]
    public Light targetLight;

    [Header("Pengaturan Suara (SFX)")]
    public AudioClip soundOn;  // Seret file suara lampu NYALA ke sini
    public AudioClip soundOff; // Seret file suara lampu MATI ke sini

    private AudioSource audioSource;
    private bool isLightOn;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (targetLight != null)
        {
            targetLight.enabled = false;
            isLightOn = false;
        }
    }

    public void ToggleLight()
    {
        if (targetLight != null)
        {
            isLightOn = !isLightOn;
            targetLight.enabled = isLightOn;

            // Panggil fungsi untuk membunyikan suara
            PlaySwitchSound();
        }
    }

    private void PlaySwitchSound()
    {
        if (audioSource != null)
        {
            // Pilih suara berdasarkan status lampu (nyala atau mati)
            AudioClip clipToPlay = isLightOn ? soundOn : soundOff;

            if (clipToPlay != null)
            {
                // PlayOneShot digunakan agar suara tidak terpotong jika tombol ditekan cepat
                audioSource.PlayOneShot(clipToPlay); 
            }
        }
    }
}