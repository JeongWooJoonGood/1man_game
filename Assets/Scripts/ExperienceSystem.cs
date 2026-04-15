using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 10;

    public void AddExperience(int amount)
    {
        currentExp += amount;
        Debug.Log("경험치 획득! +" + amount + " (총: " + currentExp + "/" + expToNextLevel + ")");

        while (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentExp -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);

        Debug.Log("레벨업! Lv." + currentLevel);

        // 플레이어 강화
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.maxHealth += 10;
            health.Heal(10);
        }

        AttackController attack = GetComponent<AttackController>();
        if (attack != null)
        {
            attack.attackDamage += 1;
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetCurrentExp()
    {
        return currentExp;
    }

    public int GetExpToNextLevel()
    {
        return expToNextLevel;
    }
}