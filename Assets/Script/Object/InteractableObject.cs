using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InteractionOption
{
    public string teksTombol; 
    public UnityEvent aksi;   

    [Tooltip("Centang jika tombol ini akan dihapus permanen setelah 1x diklik (Cocok untuk Dialog)")]
    public bool hilangSetelahDiklik = false;

    [Tooltip("Tulis nama barang yang WAJIB ADA di tas agar tombol ini muncul. (Kosongkan jika tombol selalu muncul)")]
    public string butuhItemBiarMuncul = ""; 

    [HideInInspector] 
    public bool sudahDipakai = false; 
}

public class InteractableObject : MonoBehaviour
{
    [Header("Identitas Benda")]
    public string namaObjek = "Nama Benda";

    [Header("Daftar Interaksi Utama (KLIK KIRI)")]
    public List<InteractionOption> daftarInteraksi = new List<InteractionOption>();

    [Header("Daftar Dialog / Sompral (KLIK KANAN)")]
    [Tooltip("Muncul saat pemain klik kanan. Kosongkan jika benda ini tidak bisa diajak bicara.")]
    public List<InteractionOption> daftarDialog = new List<InteractionOption>();

    [Header("Sistem Periksa & Komentar")]
    public bool hasTombolPeriksa = false;
    [TextArea(2, 5)] 
    public string komentarText = "...";
    public float durasiKomentar = 3f;

    [Header("Sistem Inventory (Opsional)")]
    public Sprite ikonItem; 
    
    [HideInInspector] 
    public bool isDiInventory = false;

    public void AmbilMasukInventory()
    {
        if (InventorySystem.instance != null) InventorySystem.instance.AmbilBarang(this);
    }
}