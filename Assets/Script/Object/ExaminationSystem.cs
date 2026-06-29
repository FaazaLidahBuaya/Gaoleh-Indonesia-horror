using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using System.Collections; // WAJIB ada untuk menggunakan Coroutine animasi

public class ExaminationSystem : MonoBehaviour
{
    public static ExaminationSystem instance;

    [Header("Referensi")]
    public Transform examineSpot; 
    public Volume globalVolume;   
    public GameObject examinationUI; 

    [Header("Pengaturan Kontrol & Animasi")]
    public float rotationSpeed = 0.5f; 
    public float zoomSpeed = 0.002f;   
    public float minZoom = 0.5f;
    public float maxZoom = 2f;
    public float smoothSpeed = 8f; 
    [Tooltip("Durasi animasi barang melayang kembali (dalam detik)")]
    public float durasiAnimasiKembali = 0.4f;

    [Header("Pengaturan Mata Pemain (Mode Normal)")]
    public float jarakMulaiBlurNormal = 4f;

    private DepthOfField dof;
    private InteractableObject currentItem;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    
    private bool isExamining = false;
    private bool isReturning = false; // Penanda apakah barang sedang proses dikembalikan
    private float targetZoom = 1f;

    void Awake() { instance = this; }

    void Start()
    {
        if (globalVolume != null) globalVolume.profile.TryGet(out dof);
        if (examinationUI != null) examinationUI.SetActive(false);
        
        if (dof != null) dof.mode.value = DepthOfFieldMode.Gaussian;
    }

    public void MulaiPeriksa(InteractableObject item)
    {
        // Cegah spam klik
        if (isReturning) return; 

        isExamining = true;
        currentItem = item;

        originalPosition = item.transform.position;
        originalRotation = item.transform.rotation;
        originalParent = item.transform.parent;

        if (item.GetComponent<Rigidbody>()) item.GetComponent<Rigidbody>().isKinematic = true;

        item.transform.SetParent(examineSpot);
        targetZoom = 1f;
        examineSpot.localPosition = new Vector3(0, 0, targetZoom);
        
        if (examinationUI != null) examinationUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        // ===== MODE JALAN-JALAN NORMAL =====
        if (!isExamining)
        {
            if (dof != null)
            {
                dof.gaussianStart.value = jarakMulaiBlurNormal;
                dof.gaussianEnd.value = jarakMulaiBlurNormal + 4f; 
            }
            return; 
        }

        // Kalau lagi proses kembali, stop logika putar-putar dan zoom
        if (isReturning) return;

        // ===== MODE PERIKSA BARANG =====
        if (currentItem == null || Mouse.current == null) return;

        // Sistem Smooth Zoom
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (scroll != 0)
        {
            targetZoom = Mathf.Clamp(targetZoom + (scroll * zoomSpeed), minZoom, maxZoom);
        }

        Vector3 targetSpotPos = new Vector3(0, 0, targetZoom);
        examineSpot.localPosition = Vector3.Lerp(examineSpot.localPosition, targetSpotPos, Time.deltaTime * smoothSpeed);
        currentItem.transform.localPosition = Vector3.Lerp(currentItem.transform.localPosition, Vector3.zero, Time.deltaTime * smoothSpeed);

        if (dof != null)
        {
            dof.gaussianStart.value = examineSpot.localPosition.z + 0.3f;
            dof.gaussianEnd.value = examineSpot.localPosition.z + 1.5f; 
        }

        // Sistem Rotasi
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            float rotX = mouseDelta.x * rotationSpeed;
            float rotY = mouseDelta.y * rotationSpeed;
            
            currentItem.transform.Rotate(examineSpot.up, -rotX, Space.World);
            currentItem.transform.Rotate(examineSpot.right, rotY, Space.World);
        }
    }

    public void SelesaiPeriksa()
    {
        // Cegah dobel eksekusi
        if (isReturning || currentItem == null) return; 
        
        // Panggil Coroutine animasi
        StartCoroutine(ProsesAnimasiKembali());
    }

    private IEnumerator ProsesAnimasiKembali()
    {
        isReturning = true;

        // 1. Langsung sembunyikan UI agar terkesan bersih saat animasi berjalan
        if (examinationUI != null) examinationUI.SetActive(false);

        // 2. Lepas dari posisi kamera (examineSpot) agar bisa bergerak bebas
        currentItem.transform.SetParent(null);

        Vector3 posisiAwal = currentItem.transform.position;
        Quaternion rotasiAwal = currentItem.transform.rotation;

        // 3. Proses animasi melayang (Lerp)
        float timer = 0f;
        while (timer < durasiAnimasiKembali)
        {
            timer += Time.deltaTime;
            float rasio = Mathf.Clamp01(timer / durasiAnimasiKembali);
            
            // Membuat pergerakannya lambat di awal, cepat di tengah, lambat di akhir (SmoothStep)
            rasio = rasio * rasio * (3f - 2f * rasio); 

            currentItem.transform.position = Vector3.Lerp(posisiAwal, originalPosition, rasio);
            currentItem.transform.rotation = Quaternion.Slerp(rotasiAwal, originalRotation, rasio);
            
            yield return null; // Tunggu satu frame
        }

        // 4. Set posisinya pas 100% dan kembalikan ke Parent aslinya
        currentItem.transform.position = originalPosition;
        currentItem.transform.rotation = originalRotation;
        currentItem.transform.SetParent(originalParent);

        if (currentItem.GetComponent<Rigidbody>()) currentItem.GetComponent<Rigidbody>().isKinematic = false;

        // 5. Cek dari mana barang ini berasal
        if (currentItem.isDiInventory)
        {
            currentItem.gameObject.SetActive(false);
            if (InventorySystem.instance != null) InventorySystem.instance.BukaInventory();
        }
        else 
        {
            PlayerInteract interact = FindAnyObjectByType<PlayerInteract>();
            if (interact != null) interact.KlikBatal(); 
        }

        // 6. Reset semuanya
        isExamining = false;
        isReturning = false;
        currentItem = null;
    }
}