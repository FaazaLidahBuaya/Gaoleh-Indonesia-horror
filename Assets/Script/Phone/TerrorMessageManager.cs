using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TemplatePesanTeror
{
    public string namaPengirim; 
    [TextArea(2, 5)]
    public string isiPesan;     
}

public class TerrorMessageManager : MonoBehaviour
{
    [Header("Daftar Pesan Teror")]
    public List<TemplatePesanTeror> daftarPesanTeror;

    [Header("Pengaturan Waktu Acak (Dalam Detik)")]
    public float jedaMinimalPesan = 120f; // Minimal 2 menit
    public float jedaMaksimalPesan = 300f; // Maksimal 5 menit

    private List<TemplatePesanTeror> sisaPesanTeror;

    void Start()
    {
        sisaPesanTeror = new List<TemplatePesanTeror>(daftarPesanTeror);
        StartCoroutine(SistemPemicuTerorOtomatis());
    }

    private IEnumerator SistemPemicuTerorOtomatis()
    {
        while (sisaPesanTeror.Count > 0)
        {
            // 1. HITUNG WAKTU ACAK DULU SEBELUM PESAN MUNCUL (Termasuk pesan pertama)
            float waktuTunggu = Random.Range(jedaMinimalPesan, jedaMaksimalPesan);
            Debug.Log($"[TEROR] Pesan akan masuk dalam {waktuTunggu / 60f:F1} menit.");
            
            yield return new WaitForSeconds(waktuTunggu);

            // 2. KIRIM PESAN
            if (PhoneSystem.instance != null)
            {
                int indeksAcak = Random.Range(0, sisaPesanTeror.Count);
                TemplatePesanTeror pesanTerpilih = sisaPesanTeror[indeksAcak];

                PhoneSystem.instance.TerimaPesanMasuk(pesanTerpilih.namaPengirim, pesanTerpilih.isiPesan);
                sisaPesanTeror.RemoveAt(indeksAcak);
            }
        }
    }
}