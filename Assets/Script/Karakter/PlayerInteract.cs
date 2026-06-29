using UnityEngine;
using UnityEngine.InputSystem; 
using TMPro; 
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events; 
using UnityEngine.EventSystems; 

public class PlayerInteract : MonoBehaviour
{
    [Header("Pengaturan Interaksi")]
    public float interactDistance = 2.5f;
    public PlayerController playerController; 

    [Header("UI References (Teks & Menu)")]
    public TextMeshProUGUI hoverText;        
    public GameObject actionMenuContainer;   
    public TextMeshProUGUI subtitleText;     

    [Header("Crosshair Settings")]
    public Image crosshair;                  
    public Color normalColor = new Color(1f, 1f, 1f, 0.6f); 
    public Color hoverColor = Color.red;                    

    [Header("Sistem Tombol Dinamis")]
    public GameObject buttonPrefab; 
    public Transform dynamicButtonParent; 

    [Header("Tombol Universal (Tetap)")]
    public GameObject btnPeriksa;            
    public GameObject btnKomentar;           

    private InteractableObject currentObj;
    private bool isMenuOpen = false;
    private Coroutine komentarCoroutine;
    
    private List<GameObject> spawnedButtons = new List<GameObject>();

    void Start()
    {
        if (hoverText != null) hoverText.gameObject.SetActive(false);
        if (actionMenuContainer != null) actionMenuContainer.SetActive(false);
        if (subtitleText != null) subtitleText.text = "";
        if (crosshair != null) crosshair.color = normalColor; 
    }

    void Update()
    {
        if (isMenuOpen) return; 

        // Pertahanan Anti Klik Tembus UI
        if (PhoneSystem.instance != null && PhoneSystem.instance.isPhoneOpen) 
        { 
            BersihkanHover(); 
            return; 
        }

        if (InventorySystem.instance != null && InventorySystem.instance.inventoryUI != null && InventorySystem.instance.inventoryUI.activeInHierarchy) 
        { 
            BersihkanHover(); 
            return; 
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            BersihkanHover();
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, interactDistance))
        {
            InteractableObject obj = hitInfo.collider.GetComponent<InteractableObject>();
            if (obj != null)
            {
                currentObj = obj;
                if (crosshair != null) crosshair.color = hoverColor;
                if (hoverText != null)
                {
                    // --- CEGAT NAMA OBJEK (HOVER) DI SINI ---
                    hoverText.text = TranslationManager.instance != null ? 
                                     TranslationManager.instance.Terjemahkan(obj.namaObjek) : obj.namaObjek;
                    hoverText.gameObject.SetActive(true);
                }

                if (Mouse.current != null)
                {
                    // === LOGIKA KLIK KIRI ===
                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        // Cek apakah objek ini punya script Noda
                        AksiBersihkanNoda komponenNoda = currentObj.GetComponent<AksiBersihkanNoda>();
                        
                        if (komponenNoda != null && InventorySystem.instance != null && SistemBersihBersih.instance != null)
                        {
                            // JIKA BELUM PUNYA SAPU/PEL
                            if (!InventorySystem.instance.CekPunyaBarang(SistemBersihBersih.instance.namaAlatPembersih))
                            {
                                // JANGAN BUKA PANEL MENU! Langsung paksa Arya komentar lewat subtitle
                                MunculkanSubtitleKustom(currentObj.komentarText, currentObj.durasiKomentar);
                            }
                            else
                            {
                                // KALAU SUDAH PUNYA, baru boleh buka panel menu interaksi
                                BukaMenuDinamis(currentObj.daftarInteraksi, true);
                            }
                        }
                        else
                        {
                            // Objek normal selain noda akan membuka menu seperti biasa
                            BukaMenuDinamis(currentObj.daftarInteraksi, true);
                        }
                    }
                    // === LOGIKA KLIK KANAN (DIALOG) ===
                    else if (Mouse.current.rightButton.wasPressedThisFrame && currentObj.daftarDialog.Count > 0)
                    {
                        BukaMenuDinamis(currentObj.daftarDialog, false);
                    }
                }
            }
            else BersihkanHover();
        }
        else BersihkanHover();
    }

    private void BersihkanHover()
    {
        currentObj = null;
        if (hoverText != null) hoverText.gameObject.SetActive(false);
        if (crosshair != null) crosshair.color = normalColor;
    }

    private void BukaMenuDinamis(List<InteractionOption> daftarMenu, bool tampilkanTombolTetap)
    {
        if (actionMenuContainer == null) return; 
        
        isMenuOpen = true;
        if (crosshair != null) crosshair.gameObject.SetActive(false);
        if (hoverText != null) hoverText.gameObject.SetActive(false); 
        
        actionMenuContainer.SetActive(true);

        if (playerController != null) playerController.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        HancurkanTombolLama(); 

        if (currentObj != null && buttonPrefab != null && dynamicButtonParent != null)
        {
            foreach (InteractionOption opsi in daftarMenu)
            {
                if (opsi.sudahDipakai) continue; 

                // Pengaman tambahan dari tas
                if (!string.IsNullOrEmpty(opsi.butuhItemBiarMuncul))
                {
                    if (InventorySystem.instance != null && !InventorySystem.instance.CekPunyaBarang(opsi.butuhItemBiarMuncul))
                    {
                        continue; 
                    }
                }

                GameObject objekTombolBaru = Instantiate(buttonPrefab, dynamicButtonParent);
                spawnedButtons.Add(objekTombolBaru);

                TextMeshProUGUI teksTombol = objekTombolBaru.GetComponentInChildren<TextMeshProUGUI>();
                if (teksTombol != null) 
                {
                    // --- CEGAT DAN TERJEMAHKAN TEKS TOMBOL DINAMIS DI SINI ---
                    teksTombol.text = TranslationManager.instance != null ? 
                                      TranslationManager.instance.Terjemahkan(opsi.teksTombol) : opsi.teksTombol;
                }

                Button komponenButton = objekTombolBaru.GetComponent<Button>();
                if (komponenButton != null)
                {
                    InteractionOption opsiLokal = opsi; 
                    
                    komponenButton.onClick.AddListener(() => {
                        opsiLokal.aksi.Invoke(); 

                        if (opsiLokal.hilangSetelahDiklik)
                        {
                            opsiLokal.sudahDipakai = true; 
                        }

                        KlikBatal();           
                    });
                }
            }
        }

        if (btnKomentar != null) btnKomentar.SetActive(tampilkanTombolTetap); 
        if (btnPeriksa != null) btnPeriksa.SetActive(tampilkanTombolTetap && currentObj.hasTombolPeriksa);
    }

    public void KlikBatal()
    {
        isMenuOpen = false;
        if (actionMenuContainer != null) actionMenuContainer.SetActive(false);

        if (crosshair != null) crosshair.gameObject.SetActive(true);

        if (playerController != null) playerController.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        HancurkanTombolLama(); 
    }

    private void HancurkanTombolLama()
    {
        foreach (GameObject tombol in spawnedButtons)
        {
            Destroy(tombol);
        }
        spawnedButtons.Clear();
    }

    public void KlikKomentar()
    {
        if (currentObj != null && subtitleText != null)
        {
            if (komentarCoroutine != null) StopCoroutine(komentarCoroutine);
            
            // --- CEGAT TEKS KOMENTAR (TOMBOL TETAP) DI SINI ---
            string teksFinal = TranslationManager.instance != null ? 
                               TranslationManager.instance.Terjemahkan(currentObj.komentarText) : currentObj.komentarText;

            komentarCoroutine = StartCoroutine(KomentarSequence(teksFinal, currentObj.durasiKomentar));
        }
    }

    public void MunculkanSubtitleKustom(string teks, float durasi = 3f)
    {
        if (komentarCoroutine != null) StopCoroutine(komentarCoroutine);
        
        // --- CEGAT SUBTITLE KUSTOM DI SINI ---
        string teksFinal = TranslationManager.instance != null ? 
                           TranslationManager.instance.Terjemahkan(teks) : teks;

        komentarCoroutine = StartCoroutine(KomentarSequence(teksFinal, durasi));
    }

    private IEnumerator KomentarSequence(string teks, float durasi)
    {
        KlikBatal(); 
        if (subtitleText != null) subtitleText.text = teks; 
        yield return new WaitForSeconds(durasi); 
        if (subtitleText != null) subtitleText.text = ""; 
    }

    public void KlikPeriksa()
    {
        if (currentObj != null && currentObj.hasTombolPeriksa)
        {
            if (actionMenuContainer != null) actionMenuContainer.SetActive(false);
            if (ExaminationSystem.instance != null) ExaminationSystem.instance.MulaiPeriksa(currentObj);
            HancurkanTombolLama();
        }
    }
}