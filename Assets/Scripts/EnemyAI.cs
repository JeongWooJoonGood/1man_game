using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // AI 설정
    public float moveSpeed = 2f;          // 이동 속도
    public float chaseRange = 5f;         // 추적 시작 거리
    public float attackRange = 1f;        // 공격 거리
    public float attackCooldown = 1.5f;   // 공격 쿨다운
    public int attackDamage = 5;          // 공격 데미지

    // 참조
    private Transform player;
    private Rigidbody2D rb;
    private float lastAttackTime = 0f;

    void Start()
    {
        // Player 찾기
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        // 플레이어와의 거리 계산
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 추적 범위 안에 있으면
        if (distanceToPlayer <= chaseRange)
        {
            // 공격 범위 안이면 공격
            if (distanceToPlayer <= attackRange)
            {
                Attack();
            }
            // 아니면 추적
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            // 범위 밖이면 정지
            rb.linearVelocity = Vector2.zero;
        }
    }

    void ChasePlayer()
    {
        // 플레이어 방향으로 이동
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    void Attack()
    {
        // 이동 정지
        rb.linearVelocity = Vector2.zero;

        // 쿨다운 체크
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log(gameObject.name + "이(가) 플레이어 공격!");

            // 플레이어 체력 시스템 있으면 데미지
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }

            lastAttackTime = Time.time;
        }
    }

    // 추적/공격 범위 시각화
    void OnDrawGizmosSelected()
    {
        // 추적 범위 (노란색)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        // 공격 범위 (빨간색)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}