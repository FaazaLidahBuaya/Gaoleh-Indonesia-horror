using UnityEngine;

public class TriggerRuangan : MonoBehaviour
{
    [Tooltip("Nama ruangan ini. Harus sama persis dengan yang ditulis di Waypoint!")]
    public string namaRuangan = "Dapur";

    private void OnTriggerStay(Collider other)
    {
        // Jika pemain menginjak kotak ini, lapor ke Manager!
        if (other.CompareTag("Player"))
        {
            if (PocongManager.instance != null)
            {
                PocongManager.instance.ruanganPemainSekarang = namaRuangan;
            }
        }
    }
}