using UnityEngine;
using System; // 추가!

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 50;
    private int currentHealth;

    [Header("UI Settings")]
    public HealthBar healthBar;

    // 죽음 이벤트 추가!
    public event Action onDeath;

    [Header("Experience")] // 추가
    public int expReward = 10; // 죽으면 주는 경험치

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " 체력: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " 사망!");

        // 죽음 이벤트 호출!
        onDeath?.Invoke();

        //경험치 지급
        ExperienceSystem playerExp = GameObject.FindGameObjectWithTag("Player")?.GetComponent<ExperienceSystem>();
        if (playerExp != null)
        {
            playerExp.AddExperience(expReward);
        }

        Destroy(gameObject);
    }
}