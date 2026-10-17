using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isGrounded;
    private Animator animator;
    private bool facingRight = true;
    private bool isJumping = false;

    // === NUEVO: Control de bloqueo para daño / knockback ===
    [HideInInspector]
    public bool canMove = true;

    [Header("Efectos visuales")]
    public GameObject polvoSaltoPrefabs;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Si canMove es falso (durante el empujón), no lee entradas del teclado
        if (!canMove) return;

        // Caminar o moverse
        float move = Input.GetAxis("Horizontal");
        float speedAnimation = Mathf.Abs(move);
        animator.SetFloat("Speed", speedAnimation);
        rb.linearVelocity = new Vector2(move * moveSpeed, rb.linearVelocity.y);

        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        // Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetBool("isJumping", true);

            if (polvoSaltoPrefabs != null)
            {
                Instantiate(polvoSaltoPrefabs, transform.position, Quaternion.identity);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoSalto);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}