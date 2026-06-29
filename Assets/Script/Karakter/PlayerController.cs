using UnityEngine;
// WAJIB: Tambahkan namespace ini untuk New Input System
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3.0f;
    public float gravity = 9.81f;

    [Header("Look Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 0.1f; 
    public float lookXLimit = 85.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Mengunci kursor mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Memastikan Kamera berada di tengah Player (X=0, Z=0) dan set tinggi mata (Y=0.8)
        if (playerCamera != null)
        {
            playerCamera.localPosition = new Vector3(0f, 0.8f, 0f);
        }
    }

    void Update()
    {
        // =================================================================
        // SISTEM PENGAMAN UI (PALANG PINTU)
        // =================================================================
        // Variabel penentu apakah pemain boleh bergerak/menengok di frame ini
        bool isAllowedToMove = canMove;

        // Cek apakah HP sedang terbuka
        if (PhoneSystem.instance != null && PhoneSystem.instance.isPhoneOpen) 
        {
            isAllowedToMove = false;
        }

        // Cek apakah Inventory sedang terbuka (jika kamu punya InventorySystem)
        if (InventorySystem.instance != null && InventorySystem.instance.inventoryUI != null && InventorySystem.instance.inventoryUI.activeInHierarchy)
        {
            isAllowedToMove = false;
        }

        // =================================================================
        // PERGERAKAN (MOVEMENT) - NEW INPUT SYSTEM
        // =================================================================
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector2 movementInput = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) movementInput.y = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) movementInput.y = -1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) movementInput.x = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) movementInput.x = 1;
        }

        // Gunakan isAllowedToMove sebagai ganti canMove
        float curSpeedX = isAllowedToMove ? walkSpeed * movementInput.y : 0;
        float curSpeedY = isAllowedToMove ? walkSpeed * movementInput.x : 0;
        
        float moveDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Gravitasi tetap berjalan meskipun UI terbuka (agar pemain tidak melayang kalau buka HP saat lompat)
        if (!characterController.isGrounded)
        {
            moveDirection.y = moveDirectionY - (gravity * Time.deltaTime);
        }
        else
        {
            moveDirection.y = -0.5f; 
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // =================================================================
        // ROTASI KAMERA (LOOK AROUND) - NEW INPUT SYSTEM
        // =================================================================
        // Rotasi hanya diizinkan jika isAllowedToMove bernilai True
        if (isAllowedToMove && playerCamera != null && Mouse.current != null)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();

            rotationX -= mouseDelta.y * mouseSensitivity;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, mouseDelta.x * mouseSensitivity, 0);
        }
    }
}