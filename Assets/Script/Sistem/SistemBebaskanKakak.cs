using UnityEngine;

public class SistemBebaskanKakak : MonoBehaviour
{
    public static SistemBebaskanKakak instance;

    [Header("Status Quest")]
    public bool catatanDibaca = false;
    public bool bukuDiletakkan = false;
    public bool kemenyanDiletakkan = false;
    public bool berasDiletakkan = false;
    public bool kopiDiletakkan = false;

    void Awake()
    {
        instance = this;
    }
}