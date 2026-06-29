using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AksiSompral : MonoBehaviour
{
    [Header("Pengaturan Dialog (Subtitle & Suara)")]
    [TextArea] public string teksDialogPemain = "...";
    public float durasiDialog = 3f;
    public AudioClip suaraDialog;

    [Header("Pengaturan Dosa (Sistem Sompral)")]
    public int poinDiberikan = 1;
    [TextArea] public string catatanDosaPemain = "...";

    [Header("Efek Jumpscare Instan (Opsional)")]
    public bool picuJumpscareKecil = false;
    public UnityEvent eventJumpscareLokal;

    private bool sudahDilakukan = false; 

    public void LakukanTindakanSompral()
    {
        if (sudahDilakukan) return; 
        sudahDilakukan = true;

        if (suaraDialog != null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                AudioSource playerAudio = player.GetComponent<AudioSource>();
                if (playerAudio != null) playerAudio.PlayOneShot(suaraDialog);
            }
        }

        PlayerInteract playerInteract = FindAnyObjectByType<PlayerInteract>();
        if (playerInteract != null && !string.IsNullOrEmpty(teksDialogPemain))
        {
            playerInteract.MunculkanSubtitleKustom(teksDialogPemain, durasiDialog);
        }

        // FORMAT BARU: Menggabungkan Nama Objek + Pesan Dosa
        InteractableObject io = GetComponent<InteractableObject>();
        string namaBenda = io != null ? io.namaObjek : gameObject.name;
        string catatanFormatLengkap = $"{namaBenda}: {catatanDosaPemain}";

        if (SompralManager.instance != null)
        {
            SompralManager.instance.TambahSompral(poinDiberikan, catatanFormatLengkap);
        }

        if (picuJumpscareKecil)
        {
            eventJumpscareLokal.Invoke();
        }

        this.enabled = false; 
    }
}