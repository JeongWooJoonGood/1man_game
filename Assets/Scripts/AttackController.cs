using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public int attackDamage = 1;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    [Header("Effect Settings")]
    public GameObject attackEffectPrefab;
    public float effectScale = 1f;
    public float effectDuration = 0.5f;

    private float lastAttackTime = 0f;
    private Vector2 attackDirection;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        attackDirection = (mousePos - transform.position).normalized;

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
        Debug.Log("공격 시도!");

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePos - (Vector2)transform.position).normalized;

        if (attackEffectPrefab != null)
        {
            Vector2 effectPos = (Vector2)transform.position + attackDirection * (attackRange * 0.5f);

            GameObject effect = Instantiate(attackEffectPrefab, effectPos, Quaternion.identity);
            effect.transform.localScale = new Vector3(effectScale, effectScale, 1f);

            Destroy(effect, effectDuration);
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, attackRange, attackDirection, 0);

        Debug.Log("감지된 오브젝트: " + hits.Length);

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
            Gizmos.DrawWireSphere(attackPos, attackRange);
        }
    }
}