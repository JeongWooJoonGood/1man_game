using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public float returnSpeed = 1.5f;
    public float arriveDistance = 0.2f;

    [Header("Combat")]
    public float attackCooldown = 1.5f;
    public int attackDamage = 5;

    // 참조
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // 추가
    private float lastAttackTime = 0f;

    // 귀환 관련
    private Vector3 startPosition;
    private bool isReturning = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 추가

        startPosition = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        float distanceToStart = Vector2.Distance(transform.position, startPosition);

        if (isReturning)
        {
            ReturnToStart();

            if (distanceToStart <= arriveDistance)
            {
                isReturning = false;
                Stop();
            }

            if (distanceToPlayer <= chaseRange)
            {
                isReturning = false;
            }

            return;
        }

        if (distanceToPlayer <= chaseRange)
        {
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            if (distanceToStart > arriveDistance)
            {
                isReturning = true;
            }
            else
            {
                Stop();
            }
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }

        // FlipX 사용 (Scale 건드리지 않음!)
        if (spriteRenderer != null)
        {
            if (direction.x < 0)
                spriteRenderer.flipX = true;  // 왼쪽
            else if (direction.x > 0)
                spriteRenderer.flipX = false; // 오른쪽
        }
    }

    void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log(gameObject.name + "이(가) 플레이어 공격!");

            if (animator != null)
            {
                // animator.SetTrigger("Attack");
            }

            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            lastAttackTime = Time.time;
        }
    }

    void ReturnToStart()
    {
        Vector2 direction = (startPosition - transform.position).normalized;
        rb.linearVelocity = direction * returnSpeed;

        if (animator != null)
        {
            animator.SetBool("isRunning", true);
        }

        // FlipX 사용 (Scale 건드리지 않음!)
        if (spriteRenderer != null)
        {
            if (direction.x < 0)
                spriteRenderer.flipX = true;  // 왼쪽
            else if (direction.x > 0)
                spriteRenderer.flipX = false; // 오른쪽
        }
    }

    void Stop()
    {
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 drawPosition = Application.isPlaying ? startPosition : transform.position;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(drawPosition, 0.3f);
        Gizmos.DrawLine(drawPosition + Vector3.up * 0.5f, drawPosition - Vector3.up * 0.5f);
        Gizmos.DrawLine(drawPosition + Vector3.right * 0.5f, drawPosition - Vector3.right * 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}