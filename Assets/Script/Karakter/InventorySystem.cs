using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    [Header("UI References")]
    public GameObject inventoryUI; 
    public Transform slotContainer; 
    public GameObject slotPrefab; 

    [Header("Penyimpanan Fisik & Animasi")]
    public Transform stashFolder; 
    public float kecepatanTerbang = 8f; 
    public AudioClip sfxAmbil; 
    private AudioSource audioSource;

    private List<InteractableObject> itemList = new List<InteractableObject>();
    private bool isInventoryOpen = false;

    void Awake() 
    { 
        instance = this; 
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        if (inventoryUI != null) inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (ExaminationSystem.instance != null && ExaminationSystem.instance.examinationUI.activeInHierarchy) return;

            if (isInventoryOpen) TutupInventory();
            else BukaInventory();
        }
    }

    public void AmbilBarang(InteractableObject item)
    {
        // Langsung daftarkan ke memori tas
        if (!itemList.Contains(item))
        {
            itemList.Add(item);
            item.isDiInventory = true;
        }

        PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();
        if (interact != null) interact.KlikBatal();

        // Jalankan animasi terbang
        StartCoroutine(AnimasiTerbangMasukTas(item));
    }

    private IEnumerator AnimasiTerbangMasukTas(InteractableObject item)
    {
        if (item.GetComponent<Collider>()) item.GetComponent<Collider>().enabled = false;
        if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().isKinematic = true;

        Transform kamera = Camera.main.transform;
        float jarak = Vector3.Distance(item.transform.position, kamera.position);

        while (jarak > 0.5f)
        {
            item.transform.position = Vector3.Lerp(item.transform.position, kamera.position, Time.deltaTime * kecepatanTerbang);
            item.transform.Rotate(Vector3.up * 360 * Time.deltaTime); 
            
            jarak = Vector3.Distance(item.transform.position, kamera.position);
            yield return null; 
        }

        if (sfxAmbil != null) audioSource.PlayOneShot(sfxAmbil);

        item.transform.SetParent(stashFolder);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity; 
        item.gameObject.SetActive(false);
    }

    public void BukaInventory()
    {
        isInventoryOpen = true;
        inventoryUI.SetActive(true);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) player.canMove = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        RefreshUI();
    }

    public void TutupInventory()
    {
        isInventoryOpen = false;
        inventoryUI.SetActive(false);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null) player.canMove = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void RefreshUI()
    {
        foreach (Transform child in slotContainer) Destroy(child.gameObject);

        foreach (InteractableObject item in itemList)
        {
            GameObject tombol = Instantiate(slotPrefab, slotContainer);
            
            Transform iconTransform = tombol.transform.Find("Icon");
            if (iconTransform != null)
            {
                Image img = iconTransform.GetComponent<Image>();
                if (item.ikonItem != null) img.sprite = item.ikonItem;
            }

            Button btn = tombol.GetComponent<Button>();
            if (btn != null)
            {
                InteractableObject itemSpesifik = item; 
                btn.onClick.AddListener(() => {
                    PeriksaDariInventory(itemSpesifik);
                });
            }
        }
    }

    private void PeriksaDariInventory(InteractableObject item)
    {
        inventoryUI.SetActive(false); 
        item.gameObject.SetActive(true); 
        
        if (ExaminationSystem.instance != null)
        {
            ExaminationSystem.instance.MulaiPeriksa(item);
        }
    }

    // Fungsi pengecekan yang sudah kebal huruf besar/kecil (Case Insensitive)
    public bool CekPunyaBarang(string namaBarangDicari)
    {
        foreach (InteractableObject item in itemList)
        {
            if (item.namaObjek.ToLower() == namaBarangDicari.ToLower()) 
            {
                return true;
            }
        }
        return false; 
    }
    // FUNGSI BARU: Untuk menghapus barang dari tas saat diletakkan di meja
    public InteractableObject HapusBarang(string namaBarangDicari)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].namaObjek.ToLower() == namaBarangDicari.ToLower()) 
            {
                InteractableObject barangDihapus = itemList[i];
                itemList.RemoveAt(i);
                barangDihapus.isDiInventory = false;
                RefreshUI();
                return barangDihapus; // Mengembalikan data barang aslinya
            }
        }
        return null; 
    }
}