using UnityEngine;
using TMPro;

public class UpgradeStation : MonoBehaviour
{
    public enum UpgradeType { Attack, Health }
    [Header("Upgrade Type")]
    public UpgradeType upgradeType;

    [Header("Settings")]
    public KeyCode interactKey = KeyCode.F;
    public int baseCost = 1;
    public float baseIncrement = 1f;
    public int currentLevel = 0;

    [Header("Interaction")]
    public float interactionRange = 2f;
    private bool playerInRange = false;
    private GameObject player;

    [Header("UI")]
    public GameObject interactionUI;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI infoText;

    // References (Header 없이)
    private PlayerGold playerGold;
    private AttackController attackController;
    private PlayerHealth playerHealth;

    // 현재 비용 및 증가량
    private int currentCost;
    private float currentIncrement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerGold = player.GetComponent<PlayerGold>();
            attackController = player.GetComponent<AttackController>();
            playerHealth = player.GetComponent<PlayerHealth>();
        }

        // 초기 비용 및 증가량 설정
        currentCost = baseCost;
        currentIncrement = baseIncrement;

        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }

        UpdateInfoText();
    }

    void Update()
    {
        CheckPlayerDistance();

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            TryUpgrade();
        }
    }

    void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance <= interactionRange)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                ShowUI();
            }
        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                HideUI();
            }
        }
    }

    void TryUpgrade()
    {
        if (playerGold == null)
        {
            Debug.Log("PlayerGold를 찾을 수 없습니다!");
            return;
        }

        // 골드 소비
        if (playerGold.SpendGold(currentCost))
        {
            currentLevel++;

            // 업그레이드 타입에 따라 처리
            if (upgradeType == UpgradeType.Attack)
            {
                if (attackController != null)
                {
                    attackController.attackDamage += Mathf.RoundToInt(currentIncrement);
                    Debug.Log("공격력 업그레이드! Lv." + currentLevel +
                             " (공격력: " + attackController.attackDamage +
                             ", 증가량: +" + currentIncrement + ")");
                }
                else
                {
                    Debug.Log("AttackController를 찾을 수 없습니다!");
                }
            }
            else if (upgradeType == UpgradeType.Health)
            {
                if (playerHealth != null)
                {
                    int healthIncrease = Mathf.RoundToInt(currentIncrement);
                    playerHealth.maxHealth += healthIncrease;
                    playerHealth.currentHealth += healthIncrease;
                    Debug.Log("체력 업그레이드! Lv." + currentLevel +
                             " (최대 체력: " + playerHealth.maxHealth +
                             ", 증가량: +" + currentIncrement + ")");
                }
                else
                {
                    Debug.Log("PlayerHealth를 찾을 수 없습니다!");
                }
            }

            // 10레벨마다 증가폭 상향 (0.5씩)
            if (currentLevel % 10 == 0)
            {
                currentIncrement += 0.5f;
                Debug.Log("증가폭 상승! 이제 +" + currentIncrement + "씩 증가합니다!");
            }

            // 비용 증가 (1골드씩)
            currentCost += 1;

            UpdateInfoText();
        }
    }

    void ShowUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }

        UpdatePromptText();
    }

    void HideUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }

    void UpdatePromptText()
    {
        if (promptText != null)
        {
            string upgradeName = upgradeType == UpgradeType.Attack ? "공격력" : "체력";
            promptText.text = "[" + interactKey.ToString() + "] " + upgradeName + " 업그레이드";
        }
    }

    void UpdateInfoText()
    {
        if (infoText != null)
        {
            string upgradeName = upgradeType == UpgradeType.Attack ? "공격력" : "체력";
            infoText.text = "Lv." + currentLevel + "\n" +
                           upgradeName + " +" + currentIncrement + "\n" +
                           "비용: " + currentCost + "G";
        }
    }

    void OnDrawGizmosSelected()
    {
        // 상호작용 범위 시각화
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}