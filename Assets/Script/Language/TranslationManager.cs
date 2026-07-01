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
        kamusInggris.Add("Baca", "Read");

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
        kamusInggris.Add("(Lemari ku...)", "(My wardrobe...)");
        kamusInggris.Add("(Kasurku...)", "(My bed...)");
        kamusInggris.Add("(Laptopku)", "(My laptop)");
        kamusInggris.Add("(Lampu tidur...)", "(Bedside lamp...)");
        kamusInggris.Add("(Lemari orang tua...)", "(Parents' wardrobe...)");
        kamusInggris.Add("(Kunci untuk membuka gudang...)", "(Key to open the warehouse...)");
        kamusInggris.Add("(Foto apa ini?)", "(What photo is this?)");
        kamusInggris.Add("(Kasur orang tuaku...)", "(My parents' bed...)");
        kamusInggris.Add("(Tolet milik orang tuaku...)", "(My parents' dressing table...)");
        kamusInggris.Add("(Siapa yang menulis catatan ini?)", "(Who wrote this note?)");
        kamusInggris.Add("(Apa mungkin pintu kamar kakak ya hahaha...)", "(Could it be my sibling's bedroom door hahaha...)");
        kamusInggris.Add("(Apakah aku harus mandi?)", "(Should I take a bath?)");
        kamusInggris.Add("(Kurasa masih bisa digunakan)", "(I think it can still be used)");
        kamusInggris.Add("(Aku akan membuangnya nanti...)", "(I'll throw it away later...)");
        kamusInggris.Add("(Tajam...)", "(Sharp...)");
        kamusInggris.Add("(Gunting...)", "(Scissors...)");
        kamusInggris.Add("(Sudah berkarat...)", "(Already rusted...)");
        kamusInggris.Add("(Piringnya bersih?)", "(Is the plate clean?)");
        kamusInggris.Add("(Masih berfungsi?)", "(Still working?)");
        kamusInggris.Add("(Ya tetangga pasti marah...)", "(Yeah, the neighbors will definitely be angry...)");
        kamusInggris.Add("(Buku harian... Sepertinya sudah usang)", "(Diary... It seems worn out)");
        kamusInggris.Add("(Siapa?)", "(Who?)");
        kamusInggris.Add("(Kakak...)", "(Sibling...)");
        kamusInggris.Add("Ini cara membebaskan kakak", "This is the way to free my sibling");
        kamusInggris.Add("(Kasur kakak)", "(Sibling's bed)");
        kamusInggris.Add("(Lemari kakak...)", "(Sibling's wardrobe...)");
        kamusInggris.Add("(Kertas apa ini...)", "(What paper is this...)");
        kamusInggris.Add("Sepertinya ini cara menyucikan keris kakek", "It seems this is the way to purify grandpa's keris");
        kamusInggris.Add("(Serem banget rasanya...)", "(It feels so creepy...)");
        kamusInggris.Add("(Untuk membersihkan noda)", "(To clean the stain)");

        //Sompral
        kamusInggris.Add("Pohonnya sudah mau tumbang, apa aku potong saja ya?", "The tree is about to fall, should I just cut it down?");
        kamusInggris.Add("Rumah tua mah dijual aja", "An old house should just be sold");
        kamusInggris.Add("Mereka pasti sudah menjadi hantu! hahaha", "They must have become ghosts! hahaha");
        kamusInggris.Add("Kalo emang beneran ada hantu, pindahin kursinya dong!", "If there really is a ghost, move the chair!");
        kamusInggris.Add("Munculah hantu, muncul lah", "Appear, ghost, appear");
        kamusInggris.Add("Bayangin coba, kalau mereka tiba tiba muncul dibelakangku", "Just imagine if they suddenly appeared behind me");
        kamusInggris.Add("Pasti ada penunggunya, coba muncul dong...", "There must be a guardian spirit, come on out...");
        kamusInggris.Add("Ini pasti topi milik hantu! ini bukan milikku", "This must be a ghost's hat! This isn't mine");
        kamusInggris.Add("Orang bunuh diri pakai ini!", "People commit suicide with this!");
        kamusInggris.Add("Lucu ya, semisal lagi masak muncul hantu dijendela", "Funny, isn't it, if a ghost appears at the window while cooking");
        kamusInggris.Add("Mana hantunya? biar aku getok pakai palu ini", "Where's the ghost? Let me hit it with this hammer");
        kamusInggris.Add("Orang yang sudah meninggal di selimuti oleh ini...", "The deceased are covered in this...");
        kamusInggris.Add("Bagaimana ya kalau ada hantu dibelakangku", "What if there is a ghost behind me");
        kamusInggris.Add("Jika dia melompat-lompat, mirip seperti...", "If it hops around, it looks like...");
        kamusInggris.Add("Kenapa ada banyak sekali foto gak jelas!", "Why are there so many unclear photos!");
        kamusInggris.Add("Kakak memang suka mengada-ngada", "Sibling really likes to make things up");
        kamusInggris.Add("Kasur kakak bau banget", "Sibling's bed smells so bad");

        //Riwayar Sompral
        kamusInggris.Add("- Pohon: Jangan buang air kecil sembarangan", "- Tree: Don't pee carelessly");
        kamusInggris.Add("- Pohon: Ada yang menjaga pohon tersebut", "- Tree: Someone is guarding the tree");
        kamusInggris.Add("- Foto: Itu rumah keluargamu", "- Photo: That is your family's house");
        kamusInggris.Add("- Foto Keluarga: Membicarakan seseorang yang sudah tidak ada itu tidak baik", "- Family Photo: It's not good to talk about someone who is gone");
        kamusInggris.Add("- Silet: Membuang penjaga", "- Blade: Throwing away the guardian");
        kamusInggris.Add("- Foto: Jangan menantang sesuatu yang telah tiada", "- Photo: Don't challenge something that is gone");
        kamusInggris.Add("- Jendela: Jangan menyuruh mereka muncul di hadapanmu", "- Window: Don't ask them to appear before you");
        kamusInggris.Add("- Kursi: Menyuruh makhluk lain seenaknya", "- Chair: Bossing around other beings");
        kamusInggris.Add("- Keris: Menantang sang roh", "- Keris: Challenging the spirit");
        kamusInggris.Add("- Topi: Hantu tidak memakai topi", "- Hat: Ghosts don't wear hats");
        kamusInggris.Add("- Baju: Bagaimana jika iya?", "- Clothes: What if they do?");
        kamusInggris.Add("- Pisau: Pemain menantang hantu untuk duduk di sofa.", "- Knife: Player challenges the ghost to sit on the sofa.");
        kamusInggris.Add("- Gunting: Membuang pelindung", "- Scissors: Throwing away the protector");
        kamusInggris.Add("- Wajan: Bagaimana jika mereka beneran muncul di jendela?", "- Wok: What if they really appear at the window?");
        kamusInggris.Add("- Foto: Jangan sompral", "- Photo: Don't be disrespectful");
        kamusInggris.Add("- Foto: Jangan menghina sesuatu", "- Photo: Don't insult something");
        kamusInggris.Add("- Guling: Memanggilnya", "- Bolster: Calling them");
        kamusInggris.Add("- Tolet: Jangan berandai pada sesuatu yang negatif", "- Dressing Table: Don't imagine negative things");
        kamusInggris.Add("- Keran: Jangan mandi malam hari", "- Faucet: Don't take a bath at night");
        kamusInggris.Add("- Palu: Mereka mungkin sedikit marah", "- Hammer: They might be a little angry");
        kamusInggris.Add("- Kain kafan: Kamu sedikit menyinggung seseorang", "- Shroud: You slightly offended someone");
        
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