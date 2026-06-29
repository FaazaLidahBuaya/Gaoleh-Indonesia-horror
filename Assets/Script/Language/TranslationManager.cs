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
        kamusInggris.Add("Silet", "Blade");
        kamusInggris.Add("Kopi hitam", "Black Coffee"); // Saya tambahkan 'e' agar ejaan bahasa Inggrisnya tepat
        kamusInggris.Add("Noda", "Stain");
        kamusInggris.Add("Sofa", "Sofa");
        kamusInggris.Add("Piring", "Plate");
        kamusInggris.Add("Apel", "Apple");
        kamusInggris.Add("Foto", "Photo");
        kamusInggris.Add("Foto keluarga", "Family Photo");
        kamusInggris.Add("Saklar", "Switch");
        kamusInggris.Add("Kunci gerbang", "Gate Key");
        kamusInggris.Add("Keris", "Keris"); // Bisa juga "Dagger", tapi "Keris" sudah diserap ke bahasa Inggris
        kamusInggris.Add("Topi", "Hat");
        kamusInggris.Add("Kalender", "Calendar");
        kamusInggris.Add("Wajan", "Wok"); // Atau "Frying Pan"
        kamusInggris.Add("Pisau", "Knife");
        kamusInggris.Add("Gunting", "Scissors");
        kamusInggris.Add("Kasur", "Bed");
        kamusInggris.Add("Guling", "Bolster");
        kamusInggris.Add("Kunci gudang", "Warehouse Key");
        kamusInggris.Add("Majalah", "Magazine");
        kamusInggris.Add("Sapu", "Broom");
        kamusInggris.Add("Palu", "Hammer");
        kamusInggris.Add("Jendela", "Window");
        kamusInggris.Add("Nyala/Mati", "On/Off"); 
        kamusInggris.Add("Buka/Tutup", "Open/Close");
        kamusInggris.Add("Meja", "Table"); // Gunakan "Desk" jika ini meja belajar/kerja
        kamusInggris.Add("Kumpulan bunga", "Bouquet"); // Bisa juga "Bunch of Flowers"
        kamusInggris.Add("Ambil", "Take"); // Bisa juga "Grab" atau "Pick Up"
        kamusInggris.Add("Kursi", "Chair");
        kamusInggris.Add("Laci", "Drawer");
        kamusInggris.Add("Kartu keluarga", "Family Card");
        kamusInggris.Add("Sertifikat hak milik", "Certificate of Ownership"); // Atau "Freehold Title"
        kamusInggris.Add("Dokumen pajak pbb", "Property Tax Document");
        kamusInggris.Add("Catatan", "Note");
        kamusInggris.Add("Kertas", "Paper");
        kamusInggris.Add("Lampu", "Lamp"); // Gunakan "Light" jika merujuk ke cahayanya
        kamusInggris.Add("Laptop", "Laptop");
        kamusInggris.Add("Komik", "Comic");
        kamusInggris.Add("Kulkas", "Refrigerator"); // Atau versi singkatnya "Fridge"
        kamusInggris.Add("Kardus", "Cardboard Box");
        kamusInggris.Add("Tumpukan box", "Stack of Boxes");
        kamusInggris.Add("Lemari", "Wardrobe"); // Gunakan "Cabinet" jika lemari barang/pajangan
        kamusInggris.Add("Keran", "Faucet"); // American English, atau "Tap" untuk British
        kamusInggris.Add("Mandi", "Bath"); // Atau "Take a Bath" / "Shower" jika berupa aksi
        kamusInggris.Add("Kasur orang tua", "Parents' Bed"); 
        kamusInggris.Add("Tolet", "Dressing Table"); // Tolet (meja rias) bisa juga "Vanity"
        
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