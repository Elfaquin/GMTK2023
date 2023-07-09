using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXpManager : MonoBehaviour
{
    public TextMeshProUGUI nextLevelXpCount;
    public TextMeshProUGUI prevLevelXpCount;
    public TextMeshProUGUI currentXpCount;
    public Slider XpSlider;

    int currentLevel;
    int currentXp;
    public AnimationCurve playerXpCurve;
    public int maxLevel;
    public int maxXp;
    public int xpGainPerBottle;
    public int xpBottleCostInGoldCurrencyWithoutInflation;

    // Start is called before the first frame update
    void Start()
    {
        currentXp = 0;
        ActualizeDisplay();
    }

    public void BuyXpBottle()
    {
        if (currentLevel == maxLevel) return;
        if (!GameLibrary.PlayerGoldManager.RemoveGold(xpBottleCostInGoldCurrencyWithoutInflation)) return;
        AddXp(xpGainPerBottle);
    }

    public void AddXp(int count)
    {
        if (currentLevel == maxLevel) return;
        if(currentXp + count >= GetNextLevelXp())
        {
            currentLevel += 1;
        }
        currentXp += count;
        ActualizeDisplay();
    }

    public bool RemoveXp(int count) 
    {
        if(currentXp - count < 0) return false;
        while(currentXp - count < GetPreviousLevelXp())
        {
            currentLevel  -= 1;
        }
        currentXp -= count;
        return true;
    }

    public void ActualizeDisplay()
    {
        nextLevelXpCount.text = $"Level {currentLevel+1}: {GetNextLevelXp()}";
        prevLevelXpCount.text = $"Level {currentLevel}: {GetPreviousLevelXp()}";
        currentXpCount.text = currentXp.ToString();
        float ratio = (float)(currentXp - GetPreviousLevelXp()) / (GetNextLevelXp() - GetPreviousLevelXp());
        Debug.Log("Ratio : " + ratio);
        XpSlider.value = ratio;
    }

    int GetNextLevelXp()
    {
        float level = playerXpCurve.Evaluate((float)(currentLevel+1)/maxLevel);
        Debug.Log("Level : "+ level);
        return (int)(level * maxXp);
    }

    int GetPreviousLevelXp()
    {
        float level = playerXpCurve.Evaluate((float)currentLevel/maxLevel);
        Debug.Log("Level : " + level);
        return (int)(level * maxXp);
    }
}
