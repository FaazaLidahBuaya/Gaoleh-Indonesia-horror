using UnityEngine;

public class JebakanKamarKakak : MonoBehaviour
{
    [Header("Pengaturan Pintu")]
    [Tooltip("Tarik objek Pintu Kamar Kakak ke sini")]
    public SimpleDoor pintuKamar; 

    [Header("Pengaturan Poin Sompral")]
    [Tooltip("Batas poin untuk memicu status terkunci")]
    public int batasPoinSompral = 20;

    [Header("Suara Jebakan")]
    [Tooltip("Suara Pintu Dibanting (BAM!)")]
    public AudioSource sfxPintuBanting;

    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang menyentuh kubus ini adalah Player
        if (other.CompareTag("Player"))
        {
            // Efek suara bantingan selalu berbunyi untuk semua kondisi
            if (sfxPintuBanting != null)
            {
                sfxPintuBanting.Play();
            }

            // AMBIL DATA POIN ASLI DARI SOMPRAL MANAGER
            int poinSekarang = 0;
            if (SompralManager.instance != null)
            {
                poinSekarang = SompralManager.instance.totalPoinSompral;
            }

            // ATURAN SOMPRAL
            if (poinSekarang >= batasPoinSompral)
            {
                Debug.Log($"💀 Poin Sompral Asli ({poinSekarang}) >= {batasPoinSompral}: Pintu dibanting dan STATUS TERKUNCI!");
                
                if (pintuKamar != null)
                {
                    // true = Tutup pintu sekaligus aktifkan status terkunci
                    pintuKamar.TutupPaksa(true); 
                }
            }
            else
            {
                Debug.Log($"😇 Poin Sompral Asli ({poinSekarang}) < {batasPoinSompral}: Pintu dibanting tapi TIDAK TERKUNCI (Bisa dibuka lagi).");
                
                if (pintuKamar != null)
                {
                    // false = Cuma tutup saja, pintu TIDAK terkunci
                    pintuKamar.TutupPaksa(false); 
                }
            }

            // Matikan trigger ini agar tidak tereksekusi berulang kali
            gameObject.SetActive(false);
        }
    }
}