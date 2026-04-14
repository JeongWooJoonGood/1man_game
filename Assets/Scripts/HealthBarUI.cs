using UnityEngine;
using UnityEngine.UI;
using TMPro;  // 추가!

public class HealthBarUI : MonoBehaviour
{
    public Image healthBarFill;
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;  // 추가!

    void Update()
    {
        if (playerHealth != null && healthBarFill != null)
        {
            float healthPercent = (float)playerHealth.GetCurrentHealth() / playerHealth.maxHealth;
            healthBarFill.fillAmount = healthPercent;

            // 추가!
            if (healthText != null)
            {
                healthText.text = playerHealth.GetCurrentHealth() + " / " + playerHealth.maxHealth;
            }
        }
    }
}