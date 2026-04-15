using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpUI : MonoBehaviour
{
    public Image expBarFill;
    public TextMeshProUGUI levelText;
    public ExperienceSystem expSystem;

    void Update()
    {
        if (expSystem != null)
        {
            // 경험치바 업데이트
            if (expBarFill != null)
            {
                float expPercent = (float)expSystem.GetCurrentExp() / expSystem.GetExpToNextLevel();
                expBarFill.fillAmount = expPercent;
            }

            // 레벨 텍스트 업데이트
            if (levelText != null)
            {
                levelText.text = "Lv." + expSystem.GetCurrentLevel();
            }
        }
    }
}