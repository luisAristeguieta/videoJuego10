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

        if (barraVidaUI != null)
        {
            barraVidaUI.maxValue = maxHealth;
            barraVidaUI.value = maxHealth;
        }

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

        Vector2 posicionObjetivo = new Vector2(player.position.x, rb.position.y);
        Vector2 nuevaPosicion = Vector2.MoveTowards(rb.position, posicionObjetivo, velocidad * Time.deltaTime);
        rb.MovePosition(nuevaPosicion);

        if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        }
        else if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 1f);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (barraVidaUI != null)
        {
            barraVidaUI.value = currentHealth;
        }

        if (!enFuria && currentHealth <= maxHealth / 2)
        {
            enFuria = true;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.red;
            }
            velocidad *= 1.8f;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (panelVictoriaUI != null)
        {
            panelVictoriaUI.SetActive(true);
        }
        Time.timeScale = 0f;
        Destroy(gameObject);
    }

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