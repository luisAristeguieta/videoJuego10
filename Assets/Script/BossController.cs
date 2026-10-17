using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [Header("1. Salud y Referencias UI")]
    public int maxHealth = 10;
    private int currentHealth;
    public Slider barraVidaUI;
    public GameObject panelVictoriaUI;

    [Header("Configuración de Movimiento e IA")]
    public Transform player;
    public float velocidad = 2.5f;
    public int danoAlJugador = 1;

    [Header("Estado de Fase 2 (Furia)")]
    private bool enFuria = false;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 2. Configurar barraVidaUI.maxValue y value con la salud máxima
        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = maxHealth;
            barraVidaUI.value = maxHealth;
        }

        // Busca al Player automáticamente si no se arrastró en el Inspector
        if (player == null)
        {
            GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
            if (objPlayer == null) objPlayer = GameObject.Find("Player");
            if (objPlayer != null) player = objPlayer.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 3. Perseguir al jugador en el eje X usando Rigidbody2D.MovePosition
        Vector2 posicionObjetivo = new Vector2(player.position.x, rb.position.y);
        Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, posicionObjetivo, velocidad * Time.deltaTime);
        rb.MovePosition(nuevaPosicion);

        // Voltear el sprite hacia el jugador manteniendo la escala 2.5
        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 1f);
        }
    }

    // 4. Función pública TakeDamage(int damage) que resta vida y actualiza el Slider de UI
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (barraVidaUI != null)
        {
            barraVidaUI.value = currentHealth;
        }

        // 5. Al bajar del 50% de vida (<= maxHealth / 2), entra en Fase de Furia
        if (!enFuria && currentHealth <= maxHealth / 2)
        {
            enFuria = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red; // Cambia el SpriteRenderer a color rojo
            }
            velocidad *= 1.8f; // Aumenta la velocidad de movimiento un 80%
        }

        // 6. Si llega a 0 de vida, activa Victoria, pausa el tiempo y destruye al Boss
        if (currentHealth <= 0)
        {
            if (panelVictoriaUI != null)
            {
                panelVictoriaUI.SetActive(true);
            }
            Time.timeScale = 0f;
            Destroy(gameObject);
        }
    }

    // Daño al jugador al chocar con el Boss
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name == "Player")
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(transform.position, danoAlJugador);
            }
        }
    }
}