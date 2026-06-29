using UnityEngine;
using System.Collections.Generic;

public class TranslationManager : MonoBehaviour
{
    public static TranslationManager instance;

    // Centang di Inspector untuk mengubah bahasa game ke Inggris
    public bool gunakanBahasaInggris = false; 

    private Dictionary<string, string> kamusInggris = new Dictionary<string, string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
            IsiKamus();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void IsiKamus()
    {
        // === TAMBAHKAN SEMUA TERJEMAHANMU DI BAWAH INI ===
        
        kamusInggris.Add("Mulai Ritual", "Start Ritual");
        kamusInggris.Add("Ambil Sesajen", "Take Offering");
        kamusInggris.Add("Sepertinya ini cara untuk menyucikan keris kakek...", "It seems this is the way to purify grandpa's keris...");
        kamusInggris.Add("Hanya benda biasa, aku tidak membutuhkannya sekarang.", "Just an ordinary object, I don't need it right now.");
        
        // Terjemahan dari error yang tadi:
        kamusInggris.Add("Gembok", "Padlock");
        kamusInggris.Add("Keluar dari sini", "Get out of here");
        kamusInggris.Add("Jual rumah", "Sell a house");
        kamusInggris.Add("Periksa", "Inspect");
        kamusInggris.Add("Komentar", "Comment");
        kamusInggris.Add("Batal", "Cancel");
        kamusInggris.Add("(Apakah aku harus keluar...)", "(Should I leave...)");
        kamusInggris.Add("Pohon", "Tree");
        kamusInggris.Add("Dupa", "Incense");
        kamusInggris.Add("Pipis", "Pee");
        kamusInggris.Add("Ambil Dupa", "Take Incense");
        kamusInggris.Add("(Kenapa mereka meletakkan dupa disini?)", "(Why did they put incense here?)");
        kamusInggris.Add("(Dulu aku sering bermain dibawah pohon...)", "(I used to play under this tree all the time...)");
        kamusInggris.Add("MALAM 1", "NIGHT 1");
        kamusInggris.Add("MALAM 2", "NIGHT 2");
        kamusInggris.Add("MALAM 3", "NIGHT 3");
        kamusInggris.Add("Batal", "Cancel");
        kamusInggris.Add("Periksa", "Examine");
        kamusInggris.Add("Komentar", "Comment");
        
        // Kalau nanti ada teks kuning lagi di Console, tambahkannya seperti ini:
        // kamusInggris.Add("Teks Indonesia", "Teks Inggris");
    }

    public string Terjemahkan(string teksAsli)
    {
        if (!gunakanBahasaInggris) return teksAsli; 

        if (kamusInggris.ContainsKey(teksAsli))
        {
            return kamusInggris[teksAsli];
        }
        
        // Peringatan kuning kalau ada teks yang lupa dimasukkan ke IsiKamus()
        Debug.LogWarning("Teks belum ada di kamus C#: " + teksAsli);
        return teksAsli; 
    }
}