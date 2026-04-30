using UnityEngine;
using System.Collections;

public class TornadoEffect : MonoBehaviour
{
    public int damage = 10;
    public float damageRange = 3f;
    public float damageInterval = 0.5f;

    void Start()
    {
        StartCoroutine(DamageLoop());
    }

    IEnumerator DamageLoop()
    {
        while (true)
        {
            DamageEnemies();
            yield return new WaitForSeconds(damageInterval);
        }
    }

    void DamageEnemies()
    {
        // Box Collider 범위로 체크
        Collider2D[] enemies = Physics2D.OverlapBoxAll(
            transform.position,
            GetComponent<BoxCollider2D>().size,
            0f
        );

        foreach (Collider2D col in enemies)
        {
            if (col.CompareTag("Enemy"))
            {
                EnemyHealth health = col.GetComponent<EnemyHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        // Box 범위 시각화
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, box.size);
        }
    }
}