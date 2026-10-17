using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    private Vector3 posicionOriginal;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Sacudir(float duracion, float fuerza)
    {
        StartCoroutine(RutinaSacudida(duracion, fuerza));
    }

    IEnumerator RutinaSacudida(float duracion, float fuerza)
    {
        posicionOriginal = transform.localPosition;

        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < duracion)
        {
            float offsetX = Random.Range(-1f, 1f) * fuerza;
            float offsetY = Random.Range(-1f, 1f) * fuerza;

            // Corregido: 'offset' cambiado por 'offsetX'
            transform.localPosition = new Vector3(posicionOriginal.x + offsetX, posicionOriginal.y + offsetY, posicionOriginal.z);

            tiempoTranscurrido += Time.deltaTime;

            yield return null;
        }

        // Vuelve a su posición original al terminar
        transform.localPosition = posicionOriginal;
    }

    void Start()
    {
        
    }
}