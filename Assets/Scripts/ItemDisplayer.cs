using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemDisplayer : MonoBehaviour
{
    [SerializeField] public bool hasItem;
    public bool isDisplayed;
    public Item containedItem;
    [SerializeField] Image itemImageUI;
    [SerializeField] Image backgroundImageUI;

    public bool debugHasStartingItem;
    public ItemEnum debugStartingItem;

    private void Start()
    {
        hasItem = debugHasStartingItem;
        if (hasItem)
        {
            SetItem(GameLibrary.GetItemFromEnum(debugStartingItem));
        }
    }

    public void ActualizeDisplay()
    {
        if(hasItem)
        {
            itemImageUI.enabled = true;
            itemImageUI.sprite = containedItem.sprite;
        }
        else
        {
            itemImageUI.enabled = false;
            itemImageUI.sprite = null;
        }
    }

    public void SetItem(Item item)
    {
        if (item != null)
        {
            containedItem = item;
            hasItem = true;
            ActualizeDisplay();
        }
        else Debug.LogError($"{transform.name} : The item passed is null.");
    }

    public void RemoveItem()
    {
        if(hasItem)
        {
            hasItem = false;
            containedItem = null;
            ActualizeDisplay();
        }
        else Debug.LogError($"{transform.name} : Doesn't have any item to remove.");
    }

    public void Show()
    {
        isDisplayed = true;
        itemImageUI.enabled= true;
        backgroundImageUI.enabled = true;
        ActualizeDisplay();
    }

    public void Hide()
    {
        isDisplayed=false;
        itemImageUI.enabled=false;
        backgroundImageUI.enabled = false;
        ActualizeDisplay();
    }
}
