using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    [Header("Level Settings")]
    public int currentLevel = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Experience Formula")]
    public float baseExp = 100f;           // 기본 경험치
    public float expMultiplier = 1.5f;     // 레벨당 곱셈 배율
    public float expExponent = 1.2f;       // 지수 (높을수록 급격히 증가)

    void Start()
    {
        CalculateExpToNextLevel();
    }

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

        // 다음 레벨 필요 경험치 계산
        CalculateExpToNextLevel();

        Debug.Log("레벨업! Lv." + currentLevel + " (다음 레벨: " + expToNextLevel + " exp)");

        // 플레이어 강화
        PlayerHealth health = GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.maxHealth += 10;
            health.Heal(10);
            health.HealFull();
        }

        AttackController attack = GetComponent<AttackController>();
        if (attack != null)
        {
            attack.attackDamage += 1;
        }

        // 스킬 해금 체크
        CheckSkillUnlocks();
    }

    void CalculateExpToNextLevel()
    {
        // 메이플 스타일 공식: baseExp * (level ^ expExponent) * expMultiplier
        expToNextLevel = Mathf.RoundToInt(baseExp * Mathf.Pow(currentLevel, expExponent) * expMultiplier);
    }

    void CheckSkillUnlocks()
    {
        if (currentLevel == 30)
        {
            Debug.Log("회오리 스킬 해금!");
        }

        if (currentLevel == 50)
        {
            Debug.Log("포획 스킬 해금!");
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

    public float GetExpPercentage()
    {
        return (float)currentExp / expToNextLevel;
    }
}