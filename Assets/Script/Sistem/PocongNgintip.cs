using UnityEngine;

public class PocongNgintip : MonoBehaviour
{
    [Header("Model Pocong Yang Mau Dihilangkan")]
    public GameObject modelPocong;

    [Header("Suara Saat Pocong Hilang (Opsional)")]
    public AudioSource sfxPocongHilang;

    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang menyentuh area ini adalah Player
        if (other.CompareTag("Player"))
        {
            if (modelPocong != null)
            {
                // Sembunyikan pocong
                modelPocong.SetActive(false);
            }

            if (sfxPocongHilang != null)
            {
                // Mainkan suara wush/kuntilanak ketawa/dll
                sfxPocongHilang.Play();
            }

            // Matikan trigger ini agar tidak berulang kali tereksekusi
            gameObject.SetActive(false);
        }
    }
}