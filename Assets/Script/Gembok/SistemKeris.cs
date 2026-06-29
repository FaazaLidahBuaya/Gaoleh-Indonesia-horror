using UnityEngine;

public class SistemKeris : MonoBehaviour
{
    public static SistemKeris instance;

    [Header("Status Quest Keris")]
    [Tooltip("Apakah pemain sudah membaca kertas petunjuk?")]
    public bool kertasRitualDibaca = false; 
    
    [Tooltip("Apakah keris sudah berhasil disucikan di meja?")]
    public bool sudahDiritualkan = false;

    void Awake()
    {
        instance = this;
    }
}