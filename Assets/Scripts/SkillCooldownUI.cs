using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    [System.Serializable]
    public class SkillUI
    {
        public Image skillIcon;
        public Image cooldownOverlay;
        public Text cooldownText;
        public KeyCode keyCode;
    }

    [Header("Skill UI Elements")]
    public SkillUI qSkill;

    [Header("References")]
    public TornadoSkill tornadoSkill;
    public ExperienceSystem experienceSystem;

    [Header("Visual Effects")]
    public Color readyColor = Color.white;
    public Color cooldownColor = Color.gray;
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public float pulseSpeed = 2f;

    void Start()
    {
        if (tornadoSkill == null)
        {
            tornadoSkill = FindAnyObjectByType<TornadoSkill>();
        }

        if (experienceSystem == null)
        {
            experienceSystem = FindAnyObjectByType<ExperienceSystem>();
        }

        if (qSkill.cooldownOverlay != null)
        {
            qSkill.cooldownOverlay.fillAmount = 0;
        }
        if (qSkill.cooldownText != null)
        {
            qSkill.cooldownText.text = "";
        }
    }

    void Update()
    {
        if (tornadoSkill == null) return;

        UpdateSkillUI(qSkill, tornadoSkill.cooldownTime, tornadoSkill.skillCooldownTimer);
    }

    void UpdateSkillUI(SkillUI skill, float maxCooldown, float currentCooldown)
    {
        bool isLevelUnlocked = experienceSystem != null && experienceSystem.currentLevel >= tornadoSkill.requiredLevel;

        if (!isLevelUnlocked)
        {
            if (skill.skillIcon != null)
            {
                skill.skillIcon.color = lockedColor;
            }

            if (skill.cooldownOverlay != null)
            {
                skill.cooldownOverlay.fillAmount = 1;
            }

            if (skill.cooldownText != null)
            {
                skill.cooldownText.text = "Lv." + tornadoSkill.requiredLevel;
            }
        }
        else if (currentCooldown > 0)
        {
            float fillAmount = currentCooldown / maxCooldown;

            if (skill.cooldownOverlay != null)
            {
                skill.cooldownOverlay.fillAmount = fillAmount;
            }

            if (skill.cooldownText != null)
            {
                skill.cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
            }

            if (skill.skillIcon != null)
            {
                skill.skillIcon.color = cooldownColor;
            }
        }
        else
        {
            if (skill.cooldownOverlay != null)
            {
                skill.cooldownOverlay.fillAmount = 0;
            }

            if (skill.cooldownText != null)
            {
                skill.cooldownText.text = "";
            }

            if (skill.skillIcon != null)
            {
                float pulse = Mathf.PingPong(Time.time * pulseSpeed, 0.3f) + 0.7f;
                skill.skillIcon.color = readyColor * pulse;
            }
        }
    }
}