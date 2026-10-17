using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Ajustes de Ataque")]
    public Transform attackPoint;
    public float attackRange = 0.6f;
    public LayerMask enemyLayer;
    public int attackDamage = 1;

    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.ReproducirSfx(AudioManager.Instance.sonidoGolpe);
            }
            Attack();
        }
    }

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");// Parametro de animación para el ataque en animator
        }

        // Detecta todos los enemigos dentro del radio de ataque
        Collider2D[] hitEnemis = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemis)
        {
            //Componente EnemyHealth de las vidas del enemigo
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }

            // NUEVO: Daño para el Jefe Final (Boss)
            BossController boss = enemy.GetComponent<BossController>();
            if (boss != null)
            {
                boss.TakeDamage(attackDamage);
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}