using System.Collections;
using UnityEngine;

public class TimedSpikeTrap : MonoBehaviour
{
    [Header("Ajustes del Tiempo")]
    public float waitTime = 2f;
    public float activeTime = 1f;

    [Header("Daño de la trampa")]
    public int damageAmount = 1; // Añadido para pasarlo al PlayerHealth

    [Header("Estado Trampa")]
    public bool isDangerous = false;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(TrapRoutine());
    }

    // Detecta cuando el jugador entra a la trampa
    private void OnTriggerEnter2D(Collider2D other)
    {
        ProcesarDanio(other);
    }

    // Detecta si el jugador se queda parado sobre la trampa
    private void OnTriggerStay2D(Collider2D other)
    {
        ProcesarDanio(other);
    }

    IEnumerator TrapRoutine()
    {
        while (true)
        {
            isDangerous = false;
            yield return new WaitForSeconds(waitTime);

            if (animator != null)
            {
                animator.SetTrigger("Active");
            }

            isDangerous = true;
            yield return new WaitForSeconds(activeTime);

            isDangerous = false;
        }
    }

    void ProcesarDanio(Collider2D other)
    {
        if (isDangerous)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Llamamos a TakeDamage pasando la posición de la trampa y el daño
                playerHealth.TakeDamage(transform.position, damageAmount);
            }
        }
    }
}