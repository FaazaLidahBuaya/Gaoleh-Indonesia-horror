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
        
        // Terjemahan interaksi dan item:
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
        kamusInggris.Add("Silet", "Blade");
        kamusInggris.Add("Kopi hitam", "Black Coffee");
        kamusInggris.Add("Noda", "Stain");
        kamusInggris.Add("Sofa", "Sofa");
        kamusInggris.Add("Piring", "Plate");
        kamusInggris.Add("Apel", "Apple");
        kamusInggris.Add("Foto", "Photo");
        kamusInggris.Add("Foto keluarga", "Family Photo");
        kamusInggris.Add("Saklar", "Switch");
        kamusInggris.Add("Kunci gerbang", "Gate Key");
        kamusInggris.Add("Keris", "Keris");
        kamusInggris.Add("Topi", "Hat");
        kamusInggris.Add("Kalender", "Calendar");
        kamusInggris.Add("Wajan", "Wok");
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
        kamusInggris.Add("Meja", "Table");
        kamusInggris.Add("Kumpulan bunga", "Bouquet");
        kamusInggris.Add("Ambil", "Take");
        kamusInggris.Add("Kursi", "Chair");
        kamusInggris.Add("Laci", "Drawer");
        kamusInggris.Add("Kartu keluarga", "Family Card");
        kamusInggris.Add("Sertifikat hak milik", "Certificate of Ownership");
        kamusInggris.Add("Dokumen pajak pbb", "Property Tax Document");
        kamusInggris.Add("Catatan", "Note");
        kamusInggris.Add("Kertas", "Paper");
        kamusInggris.Add("Lampu", "Lamp");
        kamusInggris.Add("Laptop", "Laptop");
        kamusInggris.Add("Komik", "Comic");
        kamusInggris.Add("Kulkas", "Refrigerator");
        kamusInggris.Add("Kardus", "Cardboard Box");
        kamusInggris.Add("Tumpukan box", "Stack of Boxes");
        kamusInggris.Add("Lemari", "Wardrobe");
        kamusInggris.Add("Keran", "Faucet");
        kamusInggris.Add("Mandi", "Bath");
        kamusInggris.Add("Kasur orang tua", "Parents' Bed"); 
        kamusInggris.Add("Tolet", "Dressing Table");
        kamusInggris.Add("Baju", "Clothes");
        kamusInggris.Add("Kain kafan", "Shroud");
        kamusInggris.Add("Buku harian kakak", "Sibling's Diary");
        kamusInggris.Add("Gelas", "Glass");
        kamusInggris.Add("Bersihkan", "Clean");
        kamusInggris.Add("Perbaiki posisi", "Adjust Position");
        kamusInggris.Add("Beras kuning", "Yellow Rice");
        kamusInggris.Add("Lewati", "Skip");
        kamusInggris.Add("Buang", "Discard");
        kamusInggris.Add("Televisi", "Television");
        kamusInggris.Add("Denah rumah", "House Plan");
        kamusInggris.Add("Nomer Rumah", "House Number");

        // === KOMENTAR ===
        kamusInggris.Add("(Kotor sekali...)", "(Very dirty...)");
        kamusInggris.Add("(Sudah kadaluarsa)", "(Already expired)");
        kamusInggris.Add("(Nomer 12)", "(Number 12)");
        kamusInggris.Add("(Rumah ini...)", "(This house...)");
        kamusInggris.Add("(Hanya pintu)", "(Just a door)");
        kamusInggris.Add("(Sepertinya masih berfungsi...)", "(Seems like it still works...)");
        kamusInggris.Add("(Foto keluarga... Aku, Ayah, Ibu, Kakak, dan siapa yang dipojok kanan itu...)", "(Family photo... Me, Dad, Mom, my sibling, and who is that in the right corner...)");
        kamusInggris.Add("(Tajam Sekali...)", "(Very sharp...)");
        kamusInggris.Add("(Kenapa tidak membusuk?)", "(Why isn't it rotting?)");
        kamusInggris.Add("(Piring-piring kesukaan ibu...)", "(Mom's favorite plates...)");
        kamusInggris.Add("(Kami biasanya sering duduk disini bersama)", "(We used to sit here together a lot)");
        kamusInggris.Add("(Kami sering menonton TV disini)", "(We used to watch TV here often)");
        kamusInggris.Add("(Aku bisa jadi kaya dengan ini!)", "(I could be rich with this!)");
        kamusInggris.Add("(Denah rumah ini)", "(This house plan)");
        kamusInggris.Add("(Majalah...)", "(Magazine...)");
        kamusInggris.Add("(Bukan milikku...)", "(Not mine...)");
        kamusInggris.Add("(Sepertinya sudah tidak menyala)", "(Seems like it doesn't turn on anymore)");
        kamusInggris.Add("(Foto apa ini? aku akan membuangnya nanti)", "(What photo is this? I'll throw it away later)");
        kamusInggris.Add("(Dokumen penting)", "(Important document)");
        kamusInggris.Add("(Bisa dibuka...)", "(It can be opened...)");
        kamusInggris.Add("(Surat Penting)", "(Important Letter)");
        kamusInggris.Add("(Koleksi buku..)", "(Book collection...)");
        kamusInggris.Add("(Koleksiku dulu hahaha)", "(My old collection hahaha)");
        kamusInggris.Add("(Kursi)", "(Chair)");
        kamusInggris.Add("(Foto Ibu dan Kakek dulu...)", "(An old photo of Mom and Grandpa...)");
        kamusInggris.Add("(Siapa yang dimaksud...)", "(Who is meant by this...)");
        kamusInggris.Add("(Keris peninggalan Kakek...)", "(Grandpa's heirloom keris...)");
        kamusInggris.Add("(Aku akan membersihkannya nanti deh)", "(I'll clean it later)");
        kamusInggris.Add("(Buku belajar...)", "(Study book...)");
        kamusInggris.Add("(Baju Ayah...)", "(Dad's clothes...)");
        kamusInggris.Add("(Apa-apaan ini, aku akan membersihkannya besok)", "(What is this, I'll clean it tomorrow)");

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