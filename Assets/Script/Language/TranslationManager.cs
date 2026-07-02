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
        kamusInggris.Add("Foto Keluarga", "Family Photo");
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
        kamusInggris.Add("Surat rumah", "Certificate of Ownership");
        kamusInggris.Add("Dokumen PBB", "Property Tax Document");
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
        kamusInggris.Add("Pintu", "Door");
        kamusInggris.Add("Gemboknya masih terkunci. Aku butuh kuncinya.", "The padlock is still locked. I need the key.");

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
        kamusInggris.Add("(Surat penting)", "(Important Letter)");
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

        //Phone
        kamusInggris.Add("Memanggil...", "Call...");
        kamusInggris.Add("Tersambung...", "Connected...");
        kamusInggris.Add("Nomor yang Anda tuju salah.", "The number you dialed is incorrect.");
        kamusInggris.Add("Panggilan terputus...", "Call disconnected...");
        kamusInggris.Add("Kontak tidak dikenal", "Unknown Contact");
        kamusInggris.Add("Pergi dari sini sebelum terlambat...", "Get out of here before it's too late...");
        kamusInggris.Add("Pembeli", "Buyer");
        kamusInggris.Add("Istrikuu", "My Wifee");
        kamusInggris.Add("Halo sayang? entah kenapa peraasaan ku sedang tidak enak... kamu baik baik sajakan disana? semoga iya, aku merindukanmu disini.", "Hey, honey? I don’t know why, but I’m feeling a little down… Are you doing okay over there? I hope so—I miss you here.");
        kamusInggris.Add("Rumah sakit", "Hospital");
        kamusInggris.Add("Hilang", "Missing");
        kamusInggris.Add("Berhati-hatilah", "Be careful");
        kamusInggris.Add("Pengirim", "Sender");
        kamusInggris.Add("Aku selalu mengawasimu", "I'm always watching you");
        kamusInggris.Add("Disembunyikan", "Hidden");
        kamusInggris.Add("Bebaskan aku dari penderitaan ini", "Free me from this suffering");
        kamusInggris.Add("Info desa", "Village Information");
        kamusInggris.Add("Mohon maaf karena desa sedang mengalami pemadaman listrik untuk sementara waktu dan akan berfungsi kembali besok pagi", "We apologize, but the village is currently experiencing a temporary power outage; service will resume tomorrow morning.");
        kamusInggris.Add("Untuk dokumennya sendiri harus ada:\n1. Sertifikat hak milik\n2. KK\n3. Dokumen wajib pajak", "The required documents are as follows:\n1. Certificate of ownership\n2. Family Card (KK)\n3. Taxpayer registration documents");
        kamusInggris.Add("Halo arya, katanya kamu ingin menjual rumah ya? saya bisa bantu buat mengurus beberapa hal... ", "Hi Arya, I heard you want to sell your house, right? I can help you take care of a few things... ");
        kamusInggris.Add("Halo pak, untuk tagihan perawatan rumah sakit harus segera dibayarkan paling lama 3 hari ya... jika lebih, pihak rumah sakit tidak bisa menerima pasien lagi...", "Hello, sir. Hospital bills must be paid within 3 days at the latest... If payment is delayed beyond that, the hospital will no longer be able to admit patients...");
        kamusInggris.Add("Halo lagi Arya, kamu sudah mengumpulkan semua dokumennya? kalau iya sekalian dong, bersihin rumahnya. saya mah bakal kasih bonus kalo rumahnya bersih...", "Hi again, Arya. Have you gathered all the documents yet? If so, go ahead and clean the house while you’re at it. I’ll give you a bonus if the house is clean...");

        //UI Main menu
        kamusInggris.Add("Jawa, Indonesia\n\nArya membutuhkan uang untuk biaya operasi istrinya. Dia berencana menjual rumah lama keluarganya.\n\nTapi, Rumah lama itu menyimpan hal lain yang masih misteri.","Java, Indonesia\n\nArya needs money for his wife's surgery. He plans to sell his old family house.\n\nBut, that old house holds another mystery.");
        kamusInggris.Add("MULAI", "START");
        kamusInggris.Add("SETELAN", "SETTINGS");
        kamusInggris.Add("KELUAR", "QUIT");
        kamusInggris.Add("Lanjutkan", "Continue");
        kamusInggris.Add("Paham", "Understood");
        kamusInggris.Add("Mengerti", "Understand");
        kamusInggris.Add("Kembali", "Back");
        kamusInggris.Add("Buka Dokumen", "Open Document");
        kamusInggris.Add("Hasil Dokumen", "Document Result");
        kamusInggris.Add("KONTROL", "CONTROL");
        // === UI TUTORIAL KONTROL ===

        kamusInggris.Add("Kamu bisa berbicara buruk mengenai object (Hanya beberapa), Klik kanan dan pilih dialog. PENTING, klik pada bagian tengah tombol dialog. Dan kamu akan mulai berbicara. Ambil konsekuensimu sendiri!", "You can speak ill of an object (only some). Right-click and select dialogue. IMPORTANT: click on the center of the dialogue button, and you will start speaking. Face your own consequences!");
        kamusInggris.Add("Kamu bisa melakukan berbagai interaksi pada object. Periksa untuk memeriksa object. Komentar untuk membicarakan barang tersebut. Dan interaksi(Pada barang tertentu), tombol ini bisa punya 2 interaksi.", "You can interact with objects in various ways. 'Inspect' to examine the object. 'Comment' to talk about it. And 'Interact' (for specific items); this button can have 2 different interactions.");
        kamusInggris.Add("Saat memeriksa suatu object, kamu bisa menekan Klik kanan untuk merotate object. Kamu juga bisa Zoom In/Out Object tersebut dengan Mouse Wheel", "While examining an object, you can hold Right-click to rotate it. You can also Zoom In/Out on the object using the Mouse Wheel.");
        kamusInggris.Add("DISCLAIMER\n\nGame ini adalah fan game yang terinspirasi dari Pamali. Game ini tidak berafiliasi dengan pengembang resminya. Seluruh hak atas merek dan aset asli tetap menjadi milik pemiliknya. Sebagian tekstur dalam game ini dibuat dengan bantuan AI dan telah disesuaikan untuk kebutuhan pengembangan.","DISCLAIMER\n\nThis game is a fan game inspired by Pamali. This game is not affiliated with the official developer. All rights to the brand and original assets remain the property of their respective owners. Some textures in this game were created with the help of AI and have been adjusted for development needs.");
        kamusInggris.Add("WARNING\n\nGame ini mengandung jumpscare, suara keras, cahaya berkedip, dan konten horor yang dapat mengganggu sebagian pemain. Game ini mungkin tidak cocok bagi pemain yang sensitif terhadap cahaya berkedip, suara keras, atau adegan menakutkan, dan pemain yang memiliki riwayat epilepsi fotosensitif disarankan untuk berhati-hati. Harap bermain dengan bijak.","WARNING\n\nThis game contains jumpscares, loud noises, flashing lights, and horror content that may disturb some players. This game may not be suitable for players who are sensitive to flashing lights, loud noises, or scary scenes, and players with a history of photosensitive epilepsy are advised to be cautious. Please play responsibly.");

        //Sompral
        kamusInggris.Add("\"Pohonnya sudah mau tumbang, apa aku potong saja ya?\"", "\"The tree is about to fall, should I just cut it down?\"");
        kamusInggris.Add("\"Rumah tua mah dijual aja\"", "\"An old house should just be sold\"");
        kamusInggris.Add("\"Mereka pasti sudah menjadi hantu! hahaha\"", "\"They must have become ghosts! hahaha\"");
        kamusInggris.Add("\"Kalo emang beneran ada hantu, pindahin kursinya dong!\"", "\"If there really is a ghost, move the chair!\"");
        kamusInggris.Add("\"Munculah hantu, muncul lah\"", "\"Appear, ghost, appear\"");
        kamusInggris.Add("\"Bayangin coba, kalau mereka tiba tiba muncul dibelakangku\"", "\"Just imagine if they suddenly appeared behind me\"");
        kamusInggris.Add("\"Pasti ada penunggunya, coba muncul dong...\"", "\"There must be a guardian spirit, come on out...\"");
        kamusInggris.Add("\"Ini pasti topi milik hantu! ini bukan milikku\"", "\"This must be a ghost's hat! This isn't mine\"");
        kamusInggris.Add("\"Orang bunuh diri pakai ini!\"", "\"People commit suicide with this!\"");
        kamusInggris.Add("\"Lucu ya, semisal lagi masak muncul hantu dijendela\"", "\"Funny, isn't it, if a ghost appears at the window while cooking\"");
        kamusInggris.Add("\"Mana hantunya? biar aku getok pakai palu ini\"", "\"Where's the ghost? Let me hit it with this hammer\"");
        kamusInggris.Add("\"Orang yang sudah meninggal di selimuti oleh ini...\"", "\"The deceased are covered in this...\"");
        kamusInggris.Add("\"Bagaimana ya kalau ada hantu dibelakangku\"", "\"What if there is a ghost behind me\"");
        kamusInggris.Add("\"Jika dia melompat-lompat, mirip seperti...\"", "\"If it hops around, it looks like...\"");
        kamusInggris.Add("\"Kenapa ada banyak sekali foto gak jelas!\"", "\"Why are there so many unclear photos!\"");
        kamusInggris.Add("\"Kakak memang suka mengada-ngada\"", "\"Sibling really likes to make things up\"");
        kamusInggris.Add("\"Kasur kakak bau banget\"", "\"Sibling's bed smells so bad\"");

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

        //Ending
        kamusInggris.Add("Laporan Sompral", "Sompral Report");
        kamusInggris.Add("Riwayat Sompral:", "Sompral History:");
        kamusInggris.Add("Ending 1: Melarikan Diri", "Ending 1: Running Away");
        kamusInggris.Add("Arya memutuskan untuk pergi tanpa pikir panjang dari rumah tersebut, ia merasa menjual rumah keluarganya adalah hal yang buruk. Arya pun memutuskan untuk mencari uang dengan cara lain. Terlepas dari misteri rumah lama yang sudah ditinggalkan tersebut.", "Arya decided to leave the house without a second thought; he felt that selling his family's home was a bad idea. Arya then decided to find money in another way, regardless of the mystery of the abandoned old house.");
        kamusInggris.Add("Ending 2: Terjual Begitu Murah", "Ending 2: Sold So Cheaply");
        kamusInggris.Add("Arya menemukan orang untuk menjual rumah tersebut, tetapi.. karena ia tidak membawa beberapa dokumen terkait rumah itu. Rumah tersebut dihargai begitu murah. Meski begitu, uang tersebut masih dapat digunakan untuk mengobati istrinya yang sedang sakit.", "Arya found someone to sell the house to, but... because he didn't bring some documents related to the house, it was valued very cheaply. Even so, the money could still be used to treat his sick wife.");
        kamusInggris.Add("Ending 3: Berhasil Terjual", "Ending 3: Successfully Sold");
        kamusInggris.Add("Arya berhasil menjual rumah lama itu dengan harga yang cukup mahal, memang sangat menyakitkan. Melihat rumah lamanya terjual dan dihuni oleh keluarga berikutnya, tetapi ia harus melupakan masalalunya itu dan fokus dengan apa yang sedang dihadapinya.", "Arya successfully sold the old house at a fairly high price. It was indeed very painful seeing his old house sold and occupied by the next family, but he had to forget his past and focus on what he was currently facing.");
        kamusInggris.Add("Ending 4: Menjual pusaka", "Ending 4: Selling the Heirloom");
        kamusInggris.Add("Arya melakukan metode yang tepat untuk membersihkan keris tersebut. Tetapi Arya memutuskan untuk menjual keris tersebut. Tidak disangka-sangka, harga keris tersebut ternyata lebih mahal dari rumahnya dan Arya pun mendapatkan uang yang begitu banyak.", "Arya performed the right method to cleanse the keris. However, Arya decided to sell it. Unexpectedly, the price of the keris turned out to be higher than his house, and Arya received a lot of money.");
        kamusInggris.Add("Ending 5: Pemilik pusaka", "Ending 5: The Heirloom Owner");
        kamusInggris.Add("Arya melakukan ritual penyucian pada keris tersebut, ia melakukan metode yang tepat. Roh yang ada di keris tersebut mengakui bahwa arya adalah pemilik barunya dan akan memberikan berbagai keuntungan bagi Arya kedepannya.", "Arya performed a purification ritual on the keris using the correct method. The spirit inside the keris acknowledged that Arya is its new owner and will provide various benefits for him in the future.");
        kamusInggris.Add("Ending 6: Diikuti oleh ketidakberuntungan", "Ending 6: Followed by Bad Luck");
        kamusInggris.Add("Arya mencoba membawa kabur keris peninggalan kakeknya, tetapi ia tidak memahami prosedur untuk membersihkan keris tersebut. Arya sering mendapat masalah semenjak mengambil keris tersebut. Setiap kali Arya membuang keris tersebut, dia kembali kepada Arya. Arya sudah membuat roh keris itu marah.", "Arya tried to run away with his grandfather's heirloom keris, but he didn't understand the procedure to cleanse it. Arya often encountered problems since taking the keris. Every time Arya threw it away, it returned to him. Arya had made the keris spirit angry.");
        kamusInggris.Add("Ending 7: Terjual dengan sempurna", "Ending 7: Sold Perfectly");
        kamusInggris.Add("Arya berhasil menemukan semua dokumen yang dibutuhkan serta membersihkan rumah tersebut. Dia berhasil menjual rumah tersebut dengan harga tinggi. Meskipun ada misteri yang masih tertinggal...", "Arya successfully found all the required documents and cleaned the house. He managed to sell the house at a high price. Although there is a mystery that still remains...");
        kamusInggris.Add("Ending 8: Pembuat masalah", "Ending 8: Troublemaker");
        kamusInggris.Add("Arya membuat keributan didalam rumah itu. Sehingga sosok yang menemaninya didalam sana menjadi marah. Sejak saat itu arya sering melihat sosok menyeramkan menyerupai guling yang mengikutinya kemana saja.", "Arya caused a commotion inside the house, so the figure accompanying him in there became angry. Since then, Arya often saw a terrifying figure resembling a bolster following him everywhere.");
        kamusInggris.Add("Ending 9: Merawat dengan baik", "Ending 9: Taking Good Care");
        kamusInggris.Add("Arya pergi kerumah lamanya hanya untuk membersihkan rumah tersebut. memang, ia sedang membutuhkan uang hasil menjual rumah. tapi, setidaknya makhluk yang ada disana bisa lebih senang kan?", "Arya went to his old house just to clean it. Indeed, he was in need of money from selling the house. But, at least the beings there can be happier, right?");
        kamusInggris.Add("Ending 10: Gagal terjual", "Ending 10: Failed to Sell");
        kamusInggris.Add("Rumah siap dijual, tapi konsekuensi atas hal yang telah dia perbuat karena telah membuat keributan di dalam rumah tersebut. Arya gagal menjualnya karena pembeli mengaku mendengar suara aneh dan beberapa benda berpindah tempat. Arya pun memutuskan tinggal dirumah tersebut sendirian. tapi...", "The house was ready to be sold, but there were consequences for what he had done by making a commotion inside the house. Arya failed to sell it because the buyer claimed to hear strange noises and saw some objects changing places. Arya then decided to live in the house alone. But...");
        kamusInggris.Add("Ending 11: Ketiduran", "Ending 11: Overslept");
        kamusInggris.Add("Arya melupakan tujuannya untuk menjual rumah, membuatnya telat membayarkan biaya rumah sakit. entah dengan cara apa Arya harus mencari uang. dia tetap harus menjaga istrinya.", "Arya forgot his goal of selling the house, causing him to be late in paying the hospital bills. Somehow, Arya has to find money. He still has to take care of his wife.");
        kamusInggris.Add("Ending 12: Polisi", "Ending 12: Police");
        kamusInggris.Add("Arya menelepon polisi dan mengatakan bahwa dia melihat pocong, tetapi polisi mengira Arya berbohong dan langsung menutup telepon.", "Arya called the police and said that he saw a pocong, but the police thought Arya was lying and immediately hung up the phone.");
        kamusInggris.Add("Ending 13: Terjual dengan masalah", "Ending 13: Sold with Problems");
        kamusInggris.Add("Arya menjadi orang yang rajin, mengumpulkan semua dokumen serta memberishkan rumah. Tetapi, Hal yang seharusnya tidak dia usik malah membuatnya keluar. pembeli selanjutnya mengaku bahwa mereka sering melihat sosok putih dibawah pohon dan di depan rumah.", "Arya became a diligent person, gathering all the documents and cleaning the house. However, the thing he shouldn't have disturbed made him leave instead. The next buyer claimed that they often saw a white figure under the tree and in front of the house.");
        kamusInggris.Add("Ending 14: Gagal bersembunyi", "Ending 14: Failed to Hide");
        kamusInggris.Add("Arya dipenuhi kegelisahan, gangguan yang ada dirumah ini sudah mencapai puncaknya, dia sudah tidak kuat dan memutuskan untuk bersembunyi dibawah kasur sampai pagi. tetapi, entitas itu berhasil menemukannya. Arya ditemukan dalam keadaan pingsan dan mengalami trauma berat.", "Arya was filled with anxiety, the disturbances in this house had reached their peak. He couldn't take it anymore and decided to hide under the bed until morning. However, the entity managed to find him. Arya was found unconscious and suffered severe trauma.");
        kamusInggris.Add("Ending 15: Sembunyi", "Ending 15: Hide");
        kamusInggris.Add("Arya memilih sembunyi setelah menerima berbagai gangguan dirumah tersebut. Arya berhasil bertahan di pagi hari dan langsung menjual rumah tersebut tanpa pikir panjang.", "Arya chose to hide after receiving various disturbances in the house. Arya managed to survive until morning and immediately sold the house without a second thought.");
        kamusInggris.Add("Ending 16: Diikuti", "Ending 16: Followed");
        kamusInggris.Add("Arya secara sembrono membawa pergi buku catatan kakaknya. Secara tidak langsung juga membawa arwah gentayangan bersamanya. Sekarang Arya sering melihat sosok putih berdiri di pojok ruangan kamarnya.", "Arya recklessly took his sibling's notebook away. Indirectly, he also brought the wandering spirit with him. Now Arya often sees a white figure standing in the corner of his room.");
        kamusInggris.Add("Ending 17: Rest in peace", "Ending 17: Rest in Peace");
        kamusInggris.Add("Arya berhasil membebaskan jiwa sang kakak. Dia adalah orang yang bertanggung jawab. Arwah sang kakak yang terjerat didunia pun telah tenang pergi ke alam sana.", "Arya successfully freed his sibling's soul. He is a responsible person. The sibling's spirit that was entangled in this world has peacefully gone to the afterlife.");
        kamusInggris.Add("Ending 18: Ketindihan", "Ending 18: Sleep Paralysis");
        kamusInggris.Add("Setelah membuat begitu banyak keributan, Arya akhirnya mendapat kehadirannya. dia berdiri di pojok kamarnya, menatap. Arya tidak bisa bergerak. Bau busuk itu sangat menyengat. Paginya, Arya ditemukan dalam keadaan tak sadarkan diri.", "After making so much commotion, Arya finally got its presence. It stood in the corner of his room, staring. Arya couldn't move. The foul stench was very pungent. In the morning, Arya was found unconscious.");
        // Kalau nanti ada teks kuning lagi di Console, tambahkannya seperti ini:
        // kamusInggris.Add("Teks Indonesia", "Teks Inggris");
        
        // Kalau nanti ada teks kuning lagi di Console, tambahkannya seperti ini:
        // kamusInggris.Add("Teks Indonesia", "Teks Inggris");
    }

    public string Terjemahkan(string teksAsli)
    {
        if (string.IsNullOrWhiteSpace(teksAsli)) return teksAsli;
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