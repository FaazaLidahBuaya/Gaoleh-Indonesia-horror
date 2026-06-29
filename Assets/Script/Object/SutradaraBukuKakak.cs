using UnityEngine;
using System.Collections;

public class SutradaraBukuKakak : MonoBehaviour
{
    [Header("Objek Yang Terlibat")]
    public SimpleDoor pintuKamar;
    [Tooltip("Buat objek kosong di depan pintu agar kamera tahu harus nengok ke mana")]
    public Transform titikLihatPintu; 
    
    [Header("Jebakan Lampu Tidur")]
    public Rigidbody lampuTidur;
    public Collider colliderLampu;
    public AudioSource sfxLampuJatuh;
    
    [Header("Audio (Opsional)")]
    public AudioClip sfxPintuBanting;

    private bool eventDimulai = false;

    public void EksekusiJumpscareBuku()
    {
        if (eventDimulai) return;
        eventDimulai = true;
        StartCoroutine(ProsesCinematicBuku());
    }

    private IEnumerator ProsesCinematicBuku()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        // 1. LANGSUNG KUNCI PEMAIN SAAT ITU JUGA (Tidak bisa bergerak selama 4 detik ke depan)
        if (player != null) { player.canMove = false; player.enabled = false; }
        if (interact != null) interact.KlikBatal();

        // 2. JEDA 4 DETIK HENING (Pemain kaku di tempat)
        yield return new WaitForSeconds(4f);

        // 3. BANTING PINTU
        if (pintuKamar != null) pintuKamar.TutupPaksa(true); // LANGSUNG DIKUNCI!
        if (sfxPintuBanting != null) AudioSource.PlayClipAtPoint(sfxPintuBanting, pintuKamar.transform.position);

        // 4. PAKSA KAMERA NENGOK KE PINTU (Sinematik)
        if (Camera.main != null && titikLihatPintu != null)
        {
            Quaternion rotasiAwal = Camera.main.transform.rotation;
            Vector3 arahPintu = titikLihatPintu.position - Camera.main.transform.position;
            Quaternion rotasiTarget = Quaternion.LookRotation(arahPintu);

            float timerCam = 0;
            while (timerCam < 0.4f)
            {
                timerCam += Time.deltaTime;
                Camera.main.transform.rotation = Quaternion.Slerp(rotasiAwal, rotasiTarget, timerCam / 0.4f);
                yield return null;
            }
            Camera.main.transform.rotation = rotasiTarget;
        }

        // Beri waktu sebentar agar pemain memproses pintu yang tertutup
        yield return new WaitForSeconds(1.5f);

        // 5. KEMBALIKAN KONTROL PEMAIN BEBAS BERGERAK (Setelah nengok selesai)
        if (player != null) { player.enabled = true; player.canMove = true; }
        if (interact != null) interact.MunculkanSubtitleKustom("Ha?", 4f);

        // 6. LAMPU TIDUR JATUH (Untuk memancing pemain sadar akan jendela)
        yield return new WaitForSeconds(1.5f); 
        if (lampuTidur != null)
        {
            if (colliderLampu != null) colliderLampu.enabled = true;
            lampuTidur.isKinematic = false;
            // Memberikan sedikit dorongan fisik agar lampunya terjungkal
            lampuTidur.AddForce(Vector3.forward * 1.5f, ForceMode.Impulse); 
            if (sfxLampuJatuh != null) sfxLampuJatuh.Play();
        }
    }
}   