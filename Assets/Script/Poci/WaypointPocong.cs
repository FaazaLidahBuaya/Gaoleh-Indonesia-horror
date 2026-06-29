using UnityEngine;

public enum TipePose { NgintipKiri, NgintipKanan, BalikJendela, BerdiriPenuh }

public class WaypointPocong : MonoBehaviour
{
    [Header("Pengaturan Pose")]
    public TipePose posePocong;
    
    [Header("Identitas Ruangan (Untuk Room System)")]
    [Tooltip("Tulis nama ruangan tempat titik ini berada. Misal: Dapur, KamarArya, RuangTamu")]
    public string namaRuangan = "RuangTamu";
    
    [Header("Logika Pintu (Integrasi SimpleDoor)")]
    [Tooltip("Centang ini jika titik ngintip ada di balik pintu")]
    public bool dibalikPintu = false;
    
    [Tooltip("Tarik objek pintu (yang punya script SimpleDoor) ke kolom ini")]
    public SimpleDoor pintuTerkait; 

    // Fungsi ini dipanggil oleh PocongManager untuk mengecek apakah titik ini aman dipakai
    public bool ApakahBisaDipakai()
    {
        // Jika titik ini ditandai ada di balik pintu
        if (dibalikPintu)
        {
            if (pintuTerkait != null)
            {
                // Jika pintu terbuka (isOpen == true), pocong BISA muncul di sini.
                // Jika pintu tertutup (isOpen == false), pocong TIDAK AKAN muncul di sini.
                return pintuTerkait.isOpen; 
            }
            else
            {
                Debug.LogWarning("Waypoint ditandai 'Dibalik Pintu' tapi Pintu Terkait belum diisi!");
                return false; 
            }
        }
        
        // Jika bukan di balik pintu, bebas dipakai kapan saja
        return true; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawSphere(transform.position, 0.3f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 1f);
    }
}