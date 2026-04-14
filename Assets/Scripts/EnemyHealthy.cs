using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " Ã¼·Â: " + currentHealth);

        // ÇÇ°Ý È¿°ú (»ö ±ôºýÀÓ)
        StartCoroutine(FlashRed());

        // Á×À½ Ã³¸®
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;

        sr.color = Color.white;  // Èò»öÀ¸·Î ±ôºý
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    void Die()
    {
        Debug.Log(gameObject.name + " »ç¸Á!");
        Destroy(gameObject);
    }
}