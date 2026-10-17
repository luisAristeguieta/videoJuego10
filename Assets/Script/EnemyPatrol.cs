using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Ajustes de Movimiento")]
    public float moveSpeed = 5f;
    private bool facingRight = true;

    [Header("Detectores")]
    public Transform groundCheck;
    public float distanceToGround = 1.5f;
    public float distanceToWall = 1.5f;
    public LayerMask groundLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        // 1. Movimiento horizontal sincronizado con físicas
        rb.linearVelocity = new Vector2(facingRight ? moveSpeed : -moveSpeed, rb.linearVelocity.y); // Movimiento horizontal

        // 2. Determinar hacia dónde mira el enemigo
        Vector2 direction = facingRight ? Vector2.right : Vector2.left; // Determina la dirección del raycast según la dirección en la que el enemigo está mirando

        // 3. Raycast hacia abajo desde el objeto hijo (detector de precipicio)
        RaycastHit2D isGroundAhead = Physics2D.Raycast(groundCheck.position, Vector2.down, distanceToGround, groundLayer);

        // 4. Raycast hacia adelante (detector de muros) -- Lo estoy fijando en ground y no en el transform
        RaycastHit2D isWallAhead = Physics2D.Raycast(groundCheck.position, direction, distanceToWall, groundLayer);

        // 5. Si no hay suelo al frente O choca contra una pared, gira
        if (isGroundAhead.collider == null || isWallAhead.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

        // private void OnDrawGizmos()
        // {
        //     if (groundCheck != null)
        //     {
        //         //Gizmos.color = Color.green;
        //         Gizmos.DrawRay(groundCheck.position, Vector2.down * distanceToGround);

        //         //Gizmos.color = Color.red;
        //         Vector2 direction = facingRight ? Vector2.right : Vector2.left;
        //         Gizmos.DrawRay(groundCheck.position, direction * distanceToWall);
        //     }
        // }
}
