using UnityEngine;
using UnityEngine.UI;

public class NoiseScroll : MonoBehaviour
{
    public RawImage noiseImage;

    public float speedX = 3f;
    public float speedY = 5f;

    void Update()
    {
        Rect uv = noiseImage.uvRect;

        uv.x += speedX * Time.deltaTime;
        uv.y += speedY * Time.deltaTime;

        noiseImage.uvRect = uv;
    }
}