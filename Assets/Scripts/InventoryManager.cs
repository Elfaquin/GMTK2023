using System;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public class InventoryManager : MonoBehaviour
{
    public List<ItemEnum> fetchingItems;
	[SerializeField] private TMPro.TMP_Text cat_text;
	public List<ItemEnum> items;
    public List<ItemDisplayer> itemDisplayers;
    public int gold;

    private int numberOfCats = 0;

    public bool CanHaveACat() {
        return numberOfCats < GameLibrary.PlayerXpManager.GetMaxZone();
	}

	public bool AddCat() {
		numberOfCats++;

        cat_text.text = ("" + numberOfCats);
        LogsWindow.Event_CollectedCat(numberOfCats);

		return numberOfCats >= 6;
	}



	public bool HasItem(ItemEnum itemEnum)
    {
        foreach (ItemEnum gotItemEnum in items)
        {
            if (gotItemEnum == itemEnum) return true;
        }
        return false;
    }

    public bool AddItem(ItemEnum itemEnum)
    {
        if (HasItem(itemEnum))
        {
            return false;
        }
        items.Add(itemEnum);
        ResortDisplayers();

        return true;

        // REEVALUATE QUESTS !
    }

    public bool RemoveItem(ItemEnum itemEnum)
    {
        if (!HasItem(itemEnum))
        {
            return false;
        }
        if (items.Contains(itemEnum))
        {
            items.Remove(itemEnum);
            ResortDisplayers();
            return true;
        }
        return false;
    }

    public void ResortDisplayers()
    {

        // Reset
        foreach (var itemDisplayer in itemDisplayers)
        {
            if(itemDisplayer.hasItem) itemDisplayer.RemoveItem();
        }

        // Fill
        int i = 0;
        foreach (var item in items)
        {
            itemDisplayers[i].SetItem(GameLibrary.GetItemFromEnum(item));
            i++;
        }
    }

    /**
     * <summary>
     * Returns -1 if the item is not in the inventory, else returns the index of the displayer containing it.
     * </summary>
     */
    public int GetItemIndex(ItemEnum itemEnum)
    {
        for(int i = 0; i < itemDisplayers.Count; i++)
        { 
            if (itemDisplayers[i].containedItem == GameLibrary.GetItemFromEnum(itemEnum)) return i;
        }
        return -1;
    }

    public List<ItemEnum> GetMissingItems()
    {
        List<ItemEnum> missingItems = new();
        foreach (ItemEnum item in Enum.GetValues(typeof(ItemEnum)))
        {
            if (item == ItemEnum.NullItem)
            {
                continue;
            }
            if(!HasItem(item))
            {
                missingItems.Add(item);
            }
        }
        return missingItems;
    }

    public List<ItemEnum> GetPosessedItems()
    {
        List<ItemEnum> posessedItems = new();
        foreach (ItemEnum item in Enum.GetValues(typeof(ItemEnum)))
        {
            if (item == ItemEnum.NullItem)
            {
                continue;
            }
            if (HasItem(item))
            {
                posessedItems.Add(item);
            }
        }
        return posessedItems;
    }

    public bool IsFetching(ItemEnum item)
    {
        return fetchingItems.Contains(item);
    }
}
