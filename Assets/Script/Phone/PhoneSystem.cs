using UnityEngine;
using UnityEngine.InputSystem;
using TMPro; 
using UnityEngine.UI;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine.Events; 

[System.Serializable]
public class PesanChat
{
    public string pengirim; 
    [TextArea(2, 5)] 
    public string isiPesan;
}

[System.Serializable]
public class DataKontak
{
    public string namaKontak; 
    public List<PesanChat> riwayatChat; 
}

public class PhoneSystem : MonoBehaviour
{
    public static PhoneSystem instance; 

    [Header("Panel Utama HP")]
    public GameObject phonePanel;
    public bool isPhoneOpen = false;

    [Header("Layar / Halaman HP")]
    public GameObject panelMenuUtama;
    public GameObject panelKontak;
    public GameObject panelChat;
    public GameObject panelDial;

    [Header("Sistem Spawn Tombol Kontak Otomatis")]
    public GameObject templateTombolKontak; 
    public Transform wadahTombolKontak;     

    [Header("Sistem Telepon Layar Sentuh")]
    public TextMeshProUGUI teksLayarDial; 
    public TextMeshProUGUI teksStatusPanggilan; 

    [Header("Database Rumah Chat (Runtime)")]
    public TextMeshProUGUI teksNamaKontakHeader; 
    public TextMeshProUGUI layarTeksChat;            
    public List<DataKontak> daftarKontak = new List<DataKontak>();            
    public ScrollRect scrollRectChat; 

    [Header("Pengaturan Audio (SFX)")]
    public AudioSource audioSource;
    public AudioClip sfxTombolAngka;
    public AudioClip sfxTeleponBerdering;
    public AudioClip sfxNotifikasiPesan; 

    [Header("Sistem Ending")]
    public UnityEvent eventEndingPolisi; 

    private Coroutine prosesTelepon; 

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (phonePanel != null) phonePanel.SetActive(false);
        BukaMenuUtama();          
        if (templateTombolKontak != null) templateTombolKontak.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            TogglePhone();
        }
    }

    public void TogglePhone()
    {
        if (InventorySystem.instance != null && InventorySystem.instance.inventoryUI != null && InventorySystem.instance.inventoryUI.activeInHierarchy) return;
        
        isPhoneOpen = !isPhoneOpen;
        if (phonePanel != null) phonePanel.SetActive(isPhoneOpen);
        
        PlayerController player = FindAnyObjectByType<PlayerController>();
        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();
        
        if (isPhoneOpen)
        {
            if (player != null) player.canMove = false;
            if (interact != null) interact.KlikBatal(); 
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;                      
            BukaMenuUtama(); 
        }
        else
        {
            if (player != null) player.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;       
            Cursor.visible = false;                         
            if (audioSource != null && audioSource.isPlaying) audioSource.Stop();
            if (prosesTelepon != null) StopCoroutine(prosesTelepon);
        }
    }

    public void BuatSatuTombolKontak(string namaKontak)
    {
        if (templateTombolKontak == null || wadahTombolKontak == null) return;
        
        GameObject tombolBaru = Instantiate(templateTombolKontak, wadahTombolKontak);
        tombolBaru.SetActive(true);
        
        TextMeshProUGUI teksTombol = tombolBaru.GetComponentInChildren<TextMeshProUGUI>();
        if (teksTombol != null) 
        {
            // === TRANSLATE NAMA KONTAK DI TOMBOL ===
            teksTombol.text = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan(namaKontak) : namaKontak;
        }
        
        Button btn = tombolBaru.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            string namaLokal = namaKontak; // Simpan nama asli untuk pencarian data
            btn.onClick.AddListener(() => BukaLayarChat(namaLokal));
        }
    }

    public void BukaMenuUtama() { if (panelMenuUtama != null) panelMenuUtama.SetActive(true); if (panelKontak != null) panelKontak.SetActive(false); if (panelChat != null) panelChat.SetActive(false); if (panelDial != null) panelDial.SetActive(false); }
    public void BukaLayarKontak() { if (panelMenuUtama != null) panelMenuUtama.SetActive(false); if (panelChat != null) panelChat.SetActive(false); if (panelDial != null) panelDial.SetActive(false); if (panelKontak != null) panelKontak.SetActive(true); }
    public void BukaLayarDial() { if (panelMenuUtama != null) panelMenuUtama.SetActive(false); if (panelKontak != null) panelKontak.SetActive(false); if (panelChat != null) panelChat.SetActive(false); if (panelDial != null) panelDial.SetActive(true); if (teksLayarDial != null) teksLayarDial.text = ""; if (teksStatusPanggilan != null) teksStatusPanggilan.text = ""; }
    public void TombolHome() { BukaMenuUtama(); }
    public void TombolKembali() { if (panelChat != null && panelChat.activeSelf) BukaLayarKontak(); else if ((panelKontak != null && panelKontak.activeSelf) || (panelDial != null && panelDial.activeSelf)) BukaMenuUtama(); else if (panelMenuUtama != null && panelMenuUtama.activeSelf) TogglePhone(); }
    
    public void KetikAngka(string angka) { if (teksLayarDial != null) teksLayarDial.text += angka; if (audioSource != null && sfxTombolAngka != null) audioSource.PlayOneShot(sfxTombolAngka); }
    public void HapusAngka() { if (teksLayarDial != null && teksLayarDial.text.Length > 0) { teksLayarDial.text = teksLayarDial.text.Substring(0, teksLayarDial.text.Length - 1); } if (audioSource != null && sfxTombolAngka != null) audioSource.PlayOneShot(sfxTombolAngka); }
    
    public void PanggilNomorManual()
    {
        if (teksLayarDial == null || string.IsNullOrEmpty(teksLayarDial.text)) return;
        if (prosesTelepon != null) StopCoroutine(prosesTelepon);
        string nomorDitekan = teksLayarDial.text;
        prosesTelepon = StartCoroutine(ProsesDeringTelepon(nomorDitekan));
    }

    private IEnumerator ProcessDeringTelepon(string nomor)
    {
        return ProsesDeringTelepon(nomor);
    }

    private IEnumerator ProsesDeringTelepon(string nomor)
    {
        if (teksStatusPanggilan != null) teksStatusPanggilan.text = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan("Memanggil...") : "Memanggil...";
        if (audioSource != null && sfxTeleponBerdering != null) { audioSource.clip = sfxTeleponBerdering; audioSource.loop = true; audioSource.Play(); }
        
        yield return new WaitForSeconds(4f);
        
        if (audioSource != null) { audioSource.Stop(); audioSource.loop = false; audioSource.clip = null; }
        
        if (nomor == "911" || nomor == "112")
        {
            if (teksStatusPanggilan != null) teksStatusPanggilan.text = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan("Tersambung...") : "Tersambung...";
            yield return new WaitForSeconds(1f);
            if (eventEndingPolisi != null) eventEndingPolisi.Invoke();
        }
        else if (nomor == "666") 
        { 
            if (teksStatusPanggilan != null) teksStatusPanggilan.text = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan("Panggilan terputus...") : "Panggilan terputus..."; 
        }
        else 
        { 
            if (teksStatusPanggilan != null) teksStatusPanggilan.text = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan("Nomor yang Anda tuju salah.") : "Nomor yang Anda tuju salah."; 
        }
    }

    public void BukaLayarChat(string namaKontakDicari)
    {
        if (panelKontak != null) panelKontak.SetActive(false);
        if (panelChat != null) panelChat.SetActive(true);
        if (layarTeksChat != null) layarTeksChat.text = ""; 
        
        // === TRANSLATE HEADER NAMA KONTAK ===
        if (teksNamaKontakHeader != null) 
        {
            teksNamaKontakHeader.text = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan(namaKontakDicari) : namaKontakDicari;
        }

        foreach (DataKontak data in daftarKontak)
        {
            if (data.namaKontak == namaKontakDicari)
            {
                string fullChat = "";
                foreach (PesanChat pesan in data.riwayatChat)
                {
                    // === TRANSLATE PENGIRIM DAN ISI PESAN ===
                    string pengirimT = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan(pesan.pengirim) : pesan.pengirim;
                    string pesanT = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan(pesan.isiPesan) : pesan.isiPesan;

                    if (pesan.pengirim == "Arya") 
                    {
                        fullChat += "<align=right><color=#A8E6CF><b>" + pengirimT + "</b>\n" + pesanT + "</color></align>\n\n";
                    }
                    else 
                    {
                        fullChat += "<align=left><b>" + pengirimT + "</b>\n" + pesanT + "</align>\n\n";
                    }
                }
                
                if (layarTeksChat != null) layarTeksChat.text = fullChat;                 
                Canvas.ForceUpdateCanvases();
                if (scrollRectChat != null) scrollRectChat.verticalNormalizedPosition = 1f; 
                return; 
            }
        }
        
        // Jika belum ada chat
        string teksKosong = "Belum ada pesan dengan " + namaKontakDicari;
        string teksKosongT = TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan(teksKosong) : teksKosong;
        if (layarTeksChat != null) layarTeksChat.text = "<i>" + teksKosongT + "</i>";
        
        Canvas.ForceUpdateCanvases();
        if (scrollRectChat != null) scrollRectChat.verticalNormalizedPosition = 1f;
    }

    public void TerimaPesanMasuk(string namaPengirim, string isiPesan)
    {
        if (audioSource != null && sfxNotifikasiPesan != null) audioSource.PlayOneShot(sfxNotifikasiPesan);
        
        DataKontak kontak = daftarKontak.Find(k => k.namaKontak == namaPengirim);
        
        if (kontak == null)
        {
            kontak = new DataKontak();
            kontak.namaKontak = namaPengirim;
            kontak.riwayatChat = new List<PesanChat>();
            daftarKontak.Add(kontak);
            
            BuatSatuTombolKontak(namaPengirim);
        }

        PesanChat pesanBaru = new PesanChat();
        pesanBaru.pengirim = namaPengirim;
        pesanBaru.isiPesan = isiPesan;
        kontak.riwayatChat.Add(pesanBaru);

        if (panelChat != null && panelChat.activeSelf && teksNamaKontakHeader != null && teksNamaKontakHeader.text == (TranslationManager.instance != null ? TranslationManager.instance.Terjemahkan(namaPengirim) : namaPengirim))
        {
            BukaLayarChat(namaPengirim);
        }
    }
}