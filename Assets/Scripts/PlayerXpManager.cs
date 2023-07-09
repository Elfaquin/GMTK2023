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

    public int currentLevel;
    public int currentXp;
    public AnimationCurve playerXpCurve;
    public int xpAtStart;
    public int maxLevel;
    public int maxXp;
    public int xpGainPerBottle;
    public int xpBottleCostInGoldCurrencyWithoutInflation;

    // Start is called before the first frame update
    void Start()
    {
        currentXp = xpAtStart;
        ActualizeDisplay();
    }

    public void BuyXpBottle()
    {
        if (currentLevel == maxLevel) return;
        if (!GameLibrary.PlayerGoldManager.RemoveGold(xpBottleCostInGoldCurrencyWithoutInflation)) return;
        AddXp(xpGainPerBottle);
    }

    public int GetMaxZone() {
        if(currentLevel <= 1) {
            return 1;
		} else if(currentLevel <= 3) {
			return 2;
		} else if(currentLevel <= 5) {
			return 3;
		} else if(currentLevel <= 7) {
			return 4;
		} else if(currentLevel <= 9) {
			return 5;
		} else {
            return 6;
        }
	}

    public void AddXp(int count)
    {
        if (currentLevel == maxLevel) return;
        LogsWindow.Event_GainExperience(count);

        if(currentXp + count >= GetNextLevelXp())
        {
            currentLevel += 1;
            LogsWindow.Event_GainLevel(currentLevel);
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
        XpSlider.value = ratio;
    }

    int GetNextLevelXp()
    {
        float level = playerXpCurve.Evaluate((float)(currentLevel+1)/maxLevel);
        return (int)(level * maxXp);
    }

    int GetPreviousLevelXp()
    {
        float level = playerXpCurve.Evaluate((float)currentLevel/maxLevel);
        return (int)(level * maxXp);
    }
}
