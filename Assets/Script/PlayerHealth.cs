using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public enum HealthDisplayType
    {
        Iconos, // Estrellas o corazones individuales
        Barra   // Barra de vida por Sprite Sheet
    }

    [Header("Modo de Visualización")]
    public HealthDisplayType displayType = HealthDisplayType.Barra;

    [Header("Ajustes de Vida")]
    public int maxLives = 3;
    public int currentLives;

    [Header("UI - Modo Iconos (Estrellas/Corazones)")]
    public GameObject iconsContainer;
    public Image[] iconImages;

    [Header("UI - Modo Barra (Sprite Sheet)")]
    public Image healthBarImage;
    public Sprite[] healthBarSprites;

    [Header("Invulnerabilidad y Empujón")]
    public float invulnerabilityDuration = 2f;
    public float knockbackDuration = 0.3f;
    public Vector2 knockbackForce = new Vector2(7f, 6f);
    private bool isInvulnerable = false;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    void Start()
    {
        currentLives = maxLives;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();

        ConfigureActiveDisplay();
        UpdateUI();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvulnerable)
        {
            TakeDamage(collision.transform.position, 1);
        }
    }

    public void TakeDamage(Vector2 attackerPosition, int damage = 1)
    {
        if (isInvulnerable) return; // Se  AÑADE ESTA LÍNEA: Si el jugador ya es invulnerable, ignoramos el daño por completo

        if (CameraShake.Instance != null) // Se  AÑADE ESTA CONDICIÓN: Verifica si CameraShake.Instance no es nulo antes de llamar a Sacudir
        {
            CameraShake.Instance.Sacudir(0.2f, 0.5f);
        }

        currentLives -= damage;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);
        UpdateUI();

        if (animator != null)
        {
            animator.SetTrigger("Hurt");
            if (AudioManager.Instance != null && AudioManager.Instance.sonidoGolpe != null)
            {
                AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoGolpe);
            }
        }

        if (currentLives <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(KnockbackRoutine(attackerPosition));
        StartCoroutine(InvulnerabilityRoutine());
    }

    private void ConfigureActiveDisplay()
    {
        bool useIcons = (displayType == HealthDisplayType.Iconos);

        if (iconsContainer != null)
        {
            iconsContainer.SetActive(useIcons);
        }

        if (healthBarImage != null)
        {
            healthBarImage.gameObject.SetActive(!useIcons);
        }
    }

    private void UpdateUI()
    {
        if (displayType == HealthDisplayType.Iconos)
        {
            UpdateIconsUI();
        }
        else
        {
            UpdateBarUI();
        }
    }

    private void UpdateIconsUI()
    {
        if (iconImages == null || iconImages.Length == 0) return;

        for (int i = 0; i < iconImages.Length; i++)
        {
            if (iconImages[i] != null)
            {
                iconImages[i].enabled = i < currentLives;
            }
        }
    }

    private void UpdateBarUI()
    {
        if (healthBarImage == null || healthBarSprites == null || healthBarSprites.Length == 0) return;

        float healthRatio = (float)currentLives / maxLives;
        int maxIndex = healthBarSprites.Length - 1;
        int spriteIndex = Mathf.Clamp(Mathf.RoundToInt((1f - healthRatio) * maxIndex), 0, maxIndex);

        healthBarImage.sprite = healthBarSprites[spriteIndex];
    }

    private IEnumerator KnockbackRoutine(Vector2 attackerPosition)
    {
        if (playerController != null)
            playerController.canMove = false;

        float directionX = transform.position.x < attackerPosition.x ? -1f : 1f;
        rb.linearVelocity = Vector2.zero;
        rb.linearVelocity = new Vector2(directionX * knockbackForce.x, knockbackForce.y);

        yield return new WaitForSeconds(knockbackDuration);

        if (playerController != null)
            playerController.canMove = true;
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;
        float blinkInterval = 0.15f;

        while (elapsed < invulnerabilityDuration)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval * 2;
        }

        spriteRenderer.color = Color.white;
        isInvulnerable = false;
    }

    private void Die()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}