using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UrutanPesanCerita
{
    public string namaPengirim;
    [TextArea(2, 4)]
    public string isiPesan;
    
    [Tooltip("Berapa detik jeda SEBELUM pesan ini dikirim?")]
    public float waktuTungguSebelumDikirim;
}

public class StoryMessageManager : MonoBehaviour
{
    public static StoryMessageManager instance;

    [Header("Daftar Pesan Per Malam")]
    public List<UrutanPesanCerita> pesanMalam1;
    public List<UrutanPesanCerita> pesanMalam2;
    public List<UrutanPesanCerita> pesanMalam3;

    void Awake()
    {
        instance = this;
    }

    // Fungsi Start() dihapus karena sekarang yang memberi aba-aba adalah NightManager

    public void JalankanCeritaMalam(int nomorMalam)
    {
        // Bersihkan sisa pesan dari malam sebelumnya agar tidak tumpang tindih
        StopAllCoroutines(); 
        
        Debug.Log("[STORY] Memulai antrean pesan untuk Malam " + nomorMalam);

        switch (nomorMalam)
        {
            case 1: StartCoroutine(ProsesAntreanPesan(pesanMalam1)); break;
            case 2: StartCoroutine(ProsesAntreanPesan(pesanMalam2)); break;
            case 3: StartCoroutine(ProsesAntreanPesan(pesanMalam3)); break;
        }
    }

    private IEnumerator ProsesAntreanPesan(List<UrutanPesanCerita> daftarPesan)
    {
        foreach (UrutanPesanCerita pesan in daftarPesan)
        {
            yield return new WaitForSeconds(pesan.waktuTungguSebelumDikirim);

            if (PhoneSystem.instance != null)
            {
                PhoneSystem.instance.TerimaPesanMasuk(pesan.namaPengirim, pesan.isiPesan);
                Debug.Log("[PESAN CERITA] Pesan masuk dari: " + pesan.namaPengirim);
            }
        }
    }
}