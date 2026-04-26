using UnityEngine;
using TMPro;

public class PlayerGold : MonoBehaviour
{
    [Header("Gold Settings")]
    public int currentGold = 0;

    [Header("UI")]
    public TextMeshProUGUI goldText;

    void Start()
    {
        UpdateGoldUI();
    }

    // °ñµå È¹µæ
    public void AddGold(int amount)
    {
        currentGold += amount;
        Debug.Log("°ñµå È¹µæ: +" + amount + "G (ÃÑ: " + currentGold + "G)");
        UpdateGoldUI();
    }

    // °ñµå »ç¿ë
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            Debug.Log("°ñµå »ç¿ë: -" + amount + "G (³²Àº: " + currentGold + "G)");
            UpdateGoldUI();
            return true;
        }
        else
        {
            Debug.Log("°ñµå ºÎÁ·! (ÇÊ¿ä: " + amount + "G, º¸À¯: " + currentGold + "G)");
            return false;
        }
    }

    // UI ¾÷µ¥ÀÌÆ®
    void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = "Gold: " + currentGold;
        }
    }
}