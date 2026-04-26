using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI")]
    public Image healthBarFill; // 체력바 연결

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        Debug.Log("플레이어 체력: " + currentHealth);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // 죽었으면 무시

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth); // 음수 방지

        Debug.Log("플레이어 피격! 남은 체력: " + currentHealth);

        UpdateHealthBar(); // 체력바 업데이트
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = new Color(1f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("플레이어 사망!");

        // 플레이어 비활성화
        gameObject.SetActive(false);

        // 1초 후 게임오버 화면으로
        Invoke("LoadGameOver", 1f);
    }

    void LoadGameOver()
    {
        Debug.Log("GameOverScene 로드!");
        SceneManager.LoadScene("GameOverScene");
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UpdateHealthBar(); // 체력바 업데이트
        Debug.Log("체력 회복! 현재 체력: " + currentHealth);
    }

    void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // 체력 비율 계산 (0.0 ~ 1.0)
            float fillAmount = (float)currentHealth / (float)maxHealth;
            healthBarFill.fillAmount = fillAmount;
        }
    }
}