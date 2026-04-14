using UnityEngine;

public class AttackController : MonoBehaviour
{
    // 공격 설정
    public float attackRange = 1.5f;
    public int attackDamage = 10;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    // 이펙트
    public GameObject attackEffectPrefab;

    // 내부 변수
    private float lastAttackTime = 0f;
    private Vector2 attackDirection;

    void Update()
    {
        // 마우스 위치로 공격 방향 계산
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackDirection = (mousePos - transform.position).normalized;

        // 마우스 왼쪽 클릭으로 공격
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        // 공격 위치 계산
        Vector2 attackPos = (Vector2)transform.position + attackDirection * attackRange;

        // 이펙트 생성
        if (attackEffectPrefab != null)
        {
            GameObject effect = Instantiate(attackEffectPrefab, attackPos, Quaternion.identity);
            effect.SetActive(true);
        }

        // 범위 내 적 탐지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, 0.5f, enemyLayer);

        // 적에게 데미지
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("적 공격! " + enemy.name);

            // 적 체력 시스템 추가
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackDirection != Vector2.zero)
        {
            Gizmos.color = Color.red;
            Vector2 attackPos = (Vector2)transform.position + attackDirection * attackRange;
            Gizmos.DrawWireSphere(attackPos, 0.5f);
        }
    }
}