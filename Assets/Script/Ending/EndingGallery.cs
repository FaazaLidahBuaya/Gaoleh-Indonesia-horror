using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndingGallery : MonoBehaviour
{
    [Header("Daftar Kotak Ending (Wajib Urut 1-18)")]
    public GameObject[] semuaKotakEnding;

    [Header("Pengaturan Visual")]
    public Color warnaTerkunci = new Color(0.2f, 0.2f, 0.2f, 1f); 
    public Color warnaTerbuka = Color.white; 

    [Header("Sistem Pop-up Detail Ending")]
    public GameObject panelPopupDetail;
    public TextMeshProUGUI teksDetailJudul;
    public TextMeshProUGUI teksDetailNarasi;

    private void OnEnable()
    {
        TutupDetail(); 
        RefreshGaleri();
    }

    public void RefreshGaleri()
    {
        for (int i = 0; i < semuaKotakEnding.Length; i++)
        {
            if (semuaKotakEnding[i] == null) continue;

            int idEnding = i + 1; 
            bool sudahTerbuka = PlayerPrefs.GetInt("UnlockEnding_" + idEnding, 0) == 1;

            Image kotakGambar = semuaKotakEnding[i].GetComponent<Image>();
            if (kotakGambar != null)
            {
                kotakGambar.color = sudahTerbuka ? warnaTerbuka : warnaTerkunci;
            }

            TextMeshProUGUI teksDiDalam = semuaKotakEnding[i].GetComponentInChildren<TextMeshProUGUI>();
            if (teksDiDalam != null)
            {
                teksDiDalam.text = sudahTerbuka ? "Ending " + idEnding : "???";
            }

            // Memasang fungsi klik untuk membuka teks
            Button tombol = semuaKotakEnding[i].GetComponent<Button>();
            if (tombol != null)
            {
                tombol.onClick.RemoveAllListeners(); 
                if (sudahTerbuka)
                {
                    // Tarik data judul dan narasi dari memori Unity
                    string judul = PlayerPrefs.GetString("JudulEnding_" + idEnding, "Ending " + idEnding);
                    string narasi = PlayerPrefs.GetString("NarasiEnding_" + idEnding, "Teks narasi belum terekam. Dapatkan ending ini lagi di dalam game.");
                    
                    // Hubungkan tombol ke fungsi BukaDetail
                    tombol.onClick.AddListener(() => BukaDetail(judul, narasi));
                }
            }
        }
    }
    
    // FUNGSI UNTUK MEMBUKA POP-UP
    public void BukaDetail(string judul, string narasi)
    {
        if (teksDetailJudul != null) teksDetailJudul.text = judul;
        if (teksDetailNarasi != null) teksDetailNarasi.text = narasi;
        if (panelPopupDetail != null) panelPopupDetail.SetActive(true);
    }

    // FUNGSI UNTUK MENUTUP POP-UP
    public void TutupDetail()
    {
        if (panelPopupDetail != null) panelPopupDetail.SetActive(false);
    }

    public void ResetSemuaEnding()
    {
        for (int i = 1; i <= 18; i++)
        {
            PlayerPrefs.SetInt("UnlockEnding_" + i, 0);
            PlayerPrefs.DeleteKey("JudulEnding_" + i);
            PlayerPrefs.DeleteKey("NarasiEnding_" + i);
        }
        PlayerPrefs.Save();
        RefreshGaleri();
        Debug.Log("Seluruh data ending telah direset!");
    }
}