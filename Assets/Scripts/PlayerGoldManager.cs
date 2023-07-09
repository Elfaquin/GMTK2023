using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGoldManager : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public int initialGold = 50;
    public int currentGold;

    // Start is called before the first frame update
    void Start()
    {
        currentGold = initialGold;
        ActualizeDisplay();
    }

    public void AddGold(int count)
    {
        currentGold += count;
        ActualizeDisplay();
    }

    public bool RemoveGold(int count) 
    {
        if (currentGold - count < 0) return false;
        currentGold -= count;
        ActualizeDisplay();
        return true;
    }

    void ActualizeDisplay()
    {
        goldText.text = currentGold.ToString();
        GameLibrary.InventoryManager.gold = currentGold;
    }
}
