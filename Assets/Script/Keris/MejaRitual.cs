using UnityEngine;
using System.Collections;

public class MejaRitual : MonoBehaviour
{
    [Header("Visual Benda di Atas Meja")]
    [Tooltip("Tarik objek 3D pajangan di atas meja ke kolom ini")]
    public GameObject visualKeris;
    public GameObject visualDupa;
    public GameObject visualBunga;

    [Header("Audio Ritual")]
    public AudioClip sfxRitualKresek;

    private InteractableObject kerisAsli; // Untuk mengingat keris agar bisa diambil lagi
    private bool kerisAda = false, dupaAda = false, bungaAda = false;

    void Start()
    {
        // Matikan pajangan di awal game
        if (visualKeris != null) visualKeris.SetActive(false);
        if (visualDupa != null) visualDupa.SetActive(false);
        if (visualBunga != null) visualBunga.SetActive(false);
    }

    public void LetakkanKeris()
    {
        if (InventorySystem.instance != null) {
            kerisAsli = InventorySystem.instance.HapusBarang("Keris");
            kerisAda = true;
            if (visualKeris != null) visualKeris.SetActive(true);
            CekSiapRitual();
        }
    }

    public void LetakkanDupa()
    {
        if (InventorySystem.instance != null) {
            InventorySystem.instance.HapusBarang("Dupa");
            dupaAda = true;
            if (visualDupa != null) visualDupa.SetActive(true);
            CekSiapRitual();
        }
    }

    public void LetakkanBunga()
    {
        if (InventorySystem.instance != null) {
            InventorySystem.instance.HapusBarang("Bunga");
            bungaAda = true;
            if (visualBunga != null) visualBunga.SetActive(true);
            CekSiapRitual();
        }
    }

    private void CekSiapRitual()
    {
        // Jika 3 barang sudah kumpul, munculkan tombol "Mulai Ritual"
        if (kerisAda && dupaAda && bungaAda)
        {
            InteractableObject io = GetComponent<InteractableObject>();
            InteractionOption opsiRitual = new InteractionOption();
            opsiRitual.teksTombol = "Mulai Ritual";
            opsiRitual.hilangSetelahDiklik = true;
            opsiRitual.aksi = new UnityEngine.Events.UnityEvent();
            opsiRitual.aksi.AddListener(EksekusiRitual);
            io.daftarInteraksi.Add(opsiRitual);
        }
    }

    public void EksekusiRitual()
    {
        StartCoroutine(ProsesMeredupRitual());
    }

    private IEnumerator ProsesMeredupRitual()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) player.canMove = false;

        // 1. Fade out Layar Hitam
        CanvasGroup layarHitam = NightManager.instance.panelTransisiHitam;
        if (layarHitam != null)
        {
            layarHitam.gameObject.SetActive(true);
            float t = 0;
            while (t < 1f) { t += Time.deltaTime; layarHitam.alpha = t; yield return null; }
        }

        // 2. Putar Suara Kresek-Kresek Ritual
        if (sfxRitualKresek != null)
        {
            AudioSource.PlayClipAtPoint(sfxRitualKresek, Camera.main.transform.position);
            yield return new WaitForSeconds(sfxRitualKresek.length);
        }
        else yield return new WaitForSeconds(4f);

        // 3. RITUAL BERHASIL! Ubah status keris.
        if (SistemKeris.instance != null) SistemKeris.instance.sudahDiritualkan = true;

        // 4. Fade In Layar Kembali Terang
        if (layarHitam != null)
        {
            float t = 0;
            while (t < 1f) { t += Time.deltaTime; layarHitam.alpha = 1f - t; yield return null; }
            layarHitam.gameObject.SetActive(false);
        }

        if (player != null) player.canMove = true;
        
        // 5. Ubah meja menjadi "Ambil Keris Suci"
        InteractableObject io = GetComponent<InteractableObject>();
        io.daftarInteraksi.Clear(); // Hapus sisa-sisa tombol lama
        
        InteractionOption opsiAmbil = new InteractionOption();
        opsiAmbil.teksTombol = "Ambil Keris Suci";
        opsiAmbil.hilangSetelahDiklik = true;
        opsiAmbil.aksi = new UnityEngine.Events.UnityEvent();
        opsiAmbil.aksi.AddListener(AmbilKerisSuci);
        io.daftarInteraksi.Add(opsiAmbil);
    }

    public void AmbilKerisSuci()
    {
        if (visualKeris != null) visualKeris.SetActive(false); // Hilangkan dari meja
        
        // Masukkan kembali keris aslinya ke dalam tas!
        if (kerisAsli != null && InventorySystem.instance != null)
        {
            InventorySystem.instance.AmbilBarang(kerisAsli);
        }
        
        PlayerInteract inter = FindAnyObjectByType<PlayerInteract>();
        if (inter != null) inter.MunculkanSubtitleKustom("Hawa panas dari keris ini telah menghilang. Keris ini sudah suci.", 4f);
    }
}