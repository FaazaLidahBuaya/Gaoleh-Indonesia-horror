using UnityEngine;
using System.Collections;

public class MejaRitualKakak : MonoBehaviour
{
    [Header("Visual Barang di Meja (Tarik objek 3D di meja ke sini)")]
    public GameObject visualBuku;
    public GameObject visualKemenyan;
    public GameObject visualBeras;
    public GameObject visualKopi;

    [Header("Status Pengumpulan (Biarkan False)")]
    public bool bukuTerkumpul = false;
    public bool kemenyanTerkumpul = false;
    public bool berasTerkumpul = false;
    public bool kopiTerkumpul = false;

    [Header("Pengaturan Ending Ritual")]
    public string teksDoaArya = "Semoga ini berhasil membebaskanmu, Kak...";
    public float durasiDoa = 4f;
    public AudioClip sfxAryaDoa;
    public EndingTrigger triggerEnding;

    // Dipanggil dari SyaratSesajenKakak saat barang diambil
    public void OtomatisLetakkanBenda(SyaratSesajenKakak.JenisItemKakak jenis)
    {
        if (jenis == SyaratSesajenKakak.JenisItemKakak.Buku) { bukuTerkumpul = true; if(visualBuku) visualBuku.SetActive(true); }
        else if (jenis == SyaratSesajenKakak.JenisItemKakak.Kemenyan) { kemenyanTerkumpul = true; if(visualKemenyan) visualKemenyan.SetActive(true); }
        else if (jenis == SyaratSesajenKakak.JenisItemKakak.BerasKuning) { berasTerkumpul = true; if(visualBeras) visualBeras.SetActive(true); }
        else if (jenis == SyaratSesajenKakak.JenisItemKakak.KopiHitam) { kopiTerkumpul = true; if(visualKopi) visualKopi.SetActive(true); }

        CekSemuaTerkumpul();
    }

    private void CekSemuaTerkumpul()
    {
        // Jika keempat barang sudah terkumpul
        if (bukuTerkumpul && kemenyanTerkumpul && berasTerkumpul && kopiTerkumpul)
        {
            InteractableObject io = GetComponent<InteractableObject>();
            if (io != null)
            {
                InteractionOption opsiRitual = new InteractionOption();
                opsiRitual.teksTombol = "Mulai Ritual";
                opsiRitual.hilangSetelahDiklik = true;
                opsiRitual.aksi = new UnityEngine.Events.UnityEvent();
                opsiRitual.aksi.AddListener(MulaiRitual);
                io.daftarInteraksi.Add(opsiRitual);
            }
        }
    }

    public void MulaiRitual()
    {
        StartCoroutine(ProsesRitualEnding());
    }

    private IEnumerator ProsesRitualEnding()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();

        // 1. Tutup UI
        if (interact != null) interact.KlikBatal();

        // --- JEDA SUPER PENTING ---
        yield return new WaitForSeconds(0.1f);

        // 2. Kunci Pergerakan secara Paksa (Permanen untuk Ending)
        if (player != null) 
        {
            player.canMove = false;
            player.enabled = false; // Matikan script movement total!
        }
        
        Collider mejaCollider = GetComponent<Collider>();
        if (mejaCollider != null) mejaCollider.enabled = false;

        // 3. Fade out ke Hitam 
        CanvasGroup layarHitam = null;
        if (NightManager.instance != null && NightManager.instance.panelTransisiHitam != null)
        {
            layarHitam = NightManager.instance.panelTransisiHitam;
            layarHitam.gameObject.SetActive(true);
            layarHitam.blocksRaycasts = true; 

            float t = 0;
            while (t < 1f) { t += Time.deltaTime; layarHitam.alpha = t; yield return null; }
            layarHitam.alpha = 1f;
        }

        // 4. Munculkan Subtitle Doa Arya
        if (interact != null && !string.IsNullOrEmpty(teksDoaArya))
        {
            interact.MunculkanSubtitleKustom(teksDoaArya, durasiDoa);
        }

        // 5. Putar Suara Doa Arya di kegelapan
        if (sfxAryaDoa != null)
        {
            AudioSource.PlayClipAtPoint(sfxAryaDoa, Camera.main.transform.position);
            float waktuTunggu = Mathf.Max(sfxAryaDoa.length, durasiDoa);
            yield return new WaitForSeconds(waktuTunggu + 0.5f);
        }
        else 
        {
            yield return new WaitForSeconds(durasiDoa + 0.5f); 
        }

        // 6. Panggil Ending
        if (triggerEnding != null)
        {
            triggerEnding.PicunyaEnding();
        }
        else
        {
            Debug.LogWarning("Ending Trigger belum dimasukkan ke script MejaRitualKakak!");
        }
    }
}