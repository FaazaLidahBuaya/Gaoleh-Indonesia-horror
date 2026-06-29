using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [Header("Identitas Ending")]
    [Tooltip("PENTING: Masukkan angka 1 sampai 18 sesuai urutan ending kotak Main Menu")]
    public int idEnding; 

    [Header("Data Ending")]
    public string judulEnding;
    [TextArea]
    public string narasiEnding;
    public AudioClip sfxEnding;

    [Header("Pengaturan Transisi")]
    [Tooltip("Centang untuk memunculkan panel secara perlahan (Fade). Hilangkan centang untuk layar mati instan (Jumpscare).")]
    public bool gunakanFade = true;

    public void PicunyaEnding()
    {
        if (EndingManager.instance != null)
        {
            // Sekarang kita mengirim 4 parameter yang diminta oleh EndingManager
            // 1. ID, 2. Judul, 3. Narasi, 4. SFX, 5. Fade
            EndingManager.instance.TriggerEnding(idEnding, judulEnding, narasiEnding, sfxEnding, gunakanFade);
        }
    }
}