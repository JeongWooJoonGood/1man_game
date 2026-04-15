using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int expReward = 5;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " Ã¼·Â: " + currentHealth);

        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " »ç¸Á!");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            ExperienceSystem expSystem = player.GetComponent<ExperienceSystem>();
            if (expSystem != null)
            {
                expSystem.AddExperience(expReward);
            }
        }

        Destroy(gameObject);
    }
}