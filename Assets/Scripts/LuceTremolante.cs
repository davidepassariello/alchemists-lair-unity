using UnityEngine;

public class LuceTremolante : MonoBehaviour
{
    [Header("Impostazioni Luce")]
    public Light luceLanterna;
    public float intensitaMinima = 0.5f;
    public float intensitaMassima = 2.0f;
    public float velocitaSfarfallio = 0.1f;

    void Update()
    {
        if (luceLanterna != null)
        {
            // Perlin Noise per creare un tremolio naturale e morbido, meno meccanico
            float rumore = Mathf.PerlinNoise(Time.time / velocitaSfarfallio, 0.0f);
            luceLanterna.intensity = Mathf.Lerp(intensitaMinima, intensitaMassima, rumore);
        }
    }
}