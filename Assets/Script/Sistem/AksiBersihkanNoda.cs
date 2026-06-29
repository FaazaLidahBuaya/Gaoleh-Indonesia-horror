using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AksiBersihkanNoda : MonoBehaviour
{
    [Header("Pengaturan Visual")]
    public float durasiPudar = 2f;

    [Header("Efek Audio")]
    public AudioClip sfxMenggosok; 

    [Header("Event Tambahan (Opsional)")]
    public UnityEvent aksiSetelahDibersihkan;

    private bool sudahDibersihkan = false;
    private Renderer nodaRenderer;
    private InteractableObject interactObj;

    void Start()
    {
        nodaRenderer = GetComponent<Renderer>();
        interactObj = GetComponent<InteractableObject>();
    }

    public void CobaBersihkan()
    {
        if (sudahDibersihkan) return;
        sudahDibersihkan = true;
                
        // Matikan tombol interaksinya secara permanen
        if (interactObj != null) interactObj.enabled = false;

        if (SistemBersihBersih.instance != null) SistemBersihBersih.instance.CatatSatuNodaBersih();

        if (sfxMenggosok != null)
        {
            PlayerController player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                AudioSource audioPlayer = player.GetComponent<AudioSource>();
                if (audioPlayer != null) audioPlayer.PlayOneShot(sfxMenggosok);
            }
        }

        if (aksiSetelahDibersihkan != null) aksiSetelahDibersihkan.Invoke();

        if (nodaRenderer != null) StartCoroutine(ProsesMemudar());
        else gameObject.SetActive(false); 
    }

    private IEnumerator ProsesMemudar()
    {
        Material mat = nodaRenderer.material;
        Color warnaAwal = mat.color;
        Color warnaTarget = new Color(warnaAwal.r, warnaAwal.g, warnaAwal.b, 0f);

        float waktuBerjalan = 0f;
        while (waktuBerjalan < 1f)
        {
            waktuBerjalan += Time.deltaTime / durasiPudar;
            mat.color = Color.Lerp(warnaAwal, warnaTarget, waktuBerjalan);
            yield return null; 
        }

        gameObject.SetActive(false);
    }
}