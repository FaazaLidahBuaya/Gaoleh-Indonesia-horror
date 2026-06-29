using UnityEngine;

public class PocongAI : MonoBehaviour
{
    [Header("Referensi Komponen")]
    public Transform kameraPemain;
    public Animator animPocong;
    public AudioSource suaraMenghilang; 
    
    [Header("Pengaturan Mata")]
    [Tooltip("Batas sudut pandang (dalam derajat) sampai pocong sadar dilihat")]
    public float batasSudutPandang = 80f; 
    public LayerMask layerHalangan; 
    
    [Header("Pengaturan Jeda & Jarak")]
    public float durasiJedaHilang = 2f; 
    public float jarakHilangOtomatis = 3f; 
    
    [Header("Pengaturan Gerak Tubuh")]
    public float kecepatanNengok = 3f;

    [Header("Koreksi Model 3D")]
    [Tooltip("Ubah sumbu Y (misal: 90, -90, atau 180) jika pocong menghadap samping/belakang")]
    public Vector3 koreksiRotasi = new Vector3(0, 0, 0);

    private bool sedangNgintip = false;
    private TipePose poseAktif;

    void Update()
    {
        if (sedangNgintip)
        {
            float jarak = Vector3.Distance(transform.position, kameraPemain.position);
            if (jarak < jarakHilangOtomatis)
            {
                sedangNgintip = false;
                CancelInvoke("Menghilang"); 
                Menghilang(); 
                return;
            }

            CekDitatapPemain();

            if (poseAktif == TipePose.BerdiriPenuh)
            {
                TatapPemainTerus();
            }
        }
    }

    public void MulaiNgintip(WaypointPocong waypoint)
    {
        transform.position = waypoint.transform.position;
        transform.rotation = waypoint.transform.rotation * Quaternion.Euler(koreksiRotasi);
        
        int indexPose = (int)waypoint.posePocong;
        poseAktif = waypoint.posePocong; 

        // 1. BANGUNKAN POCONGNYA DULU!
        gameObject.SetActive(true);
        sedangNgintip = true;

        // 2. SETELAH BANGUN, BARU TERIAKIN PERINTAHNYA!
        if(animPocong != null)
        {
            animPocong.SetInteger("IndexPose", indexPose);
        }

        Invoke("Menghilang", 20f);
    }

    void CekDitatapPemain()
    {
        Vector3 targetPocong = transform.position + Vector3.up * 1.5f; 
        Vector3 arahKePocong = targetPocong - kameraPemain.position;
        
        float sudut = Vector3.Angle(kameraPemain.forward, arahKePocong.normalized);
        Debug.DrawRay(kameraPemain.position, arahKePocong.normalized * 20f, Color.red);

        if (sudut < batasSudutPandang)
        {
            RaycastHit hit;
            if (Physics.Raycast(kameraPemain.position, arahKePocong.normalized, out hit, 20f, layerHalangan))
            {
                if (hit.collider.CompareTag("Pocong") || hit.collider.gameObject == this.gameObject)
                {
                    if (!sedangNgintip) return; 
                    sedangNgintip = false; 
                    Invoke("Menghilang", durasiJedaHilang);
                }
            }
        }
    }

    void TatapPemainTerus()
    {
        Vector3 arahPemain = kameraPemain.position - transform.position;
        arahPemain.y = 0; 

        if (arahPemain != Vector3.zero)
        {
            // 1. Dapatkan arah hadap normal ke pemain
            Quaternion rotasiDasar = Quaternion.LookRotation(arahPemain);
            
            // 2. [MODIFIKASI]: Tambahkan kemiringan 90 derajat milik modelmu
            Quaternion rotasiTarget = rotasiDasar * Quaternion.Euler(koreksiRotasi);
            
            // 3. Putar pocong menuju rotasi yang sudah dikoreksi
            transform.rotation = Quaternion.Slerp(transform.rotation, rotasiTarget, Time.deltaTime * kecepatanNengok);
        }
    }

    // Tambahkan variabel ini di atas (bareng variabel lain)
    public AudioSource audioSourceGlobal; 

    void Menghilang()
    {
        CancelInvoke("Menghilang");
        // Pindahkan suara ke objek yang tidak akan mati
        if(audioSourceGlobal != null && suaraMenghilang != null)
        {
            audioSourceGlobal.clip = suaraMenghilang.clip;
            audioSourceGlobal.Play();
        }

        gameObject.SetActive(false); // Pocong mati, tapi suaranya tetap hidup di PocongAudioManager!
    }
}