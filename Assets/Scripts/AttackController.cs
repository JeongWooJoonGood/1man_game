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
        Debug.Log("공격 시도!"); // 이거 추가

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePos - (Vector2)transform.position).normalized;

        if (attackEffectPrefab != null)
        {
            Vector2 effectPos = (Vector2)transform.position + attackDirection * attackRange;
            GameObject effect = Instantiate(attackEffectPrefab, effectPos, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, attackDirection, 0);

        Debug.Log("감지된 오브젝트: " + hits.Length); // 이거 추가

        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("hit: " + hit.collider.name + ", Tag: " + hit.collider.tag);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(attackDamage);
                    Debug.Log("적 공격! 데미지: " + attackDamage);
                }
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