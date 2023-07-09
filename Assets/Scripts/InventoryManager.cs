using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemEnum> fetchingItems;
	public List<ItemEnum> items;
    public List<ItemDisplayer> itemDisplayers;
    public int gold;

    private int numberOfCats = 0;
    [SerializeField] private Transform catContainer;
    [SerializeField] private Transform catPrefab;

	public bool CanHaveACat() {
        return numberOfCats < GameLibrary.PlayerXpManager.GetMaxZone();
	}

	public bool AddCat() {
		numberOfCats++;

        var cat = Instantiate(catPrefab);
        cat.SetParent(catContainer);
        cat.localScale = Vector3.one;

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

    public List<ItemEnum> GetNotPossessedNorFetchedItems()
    {
        List<ItemEnum> posessedItems = new();
        foreach (ItemEnum item in Enum.GetValues(typeof(ItemEnum)))
        {
            if (item == ItemEnum.NullItem)
            {
                continue;
            }
            if (!HasItem(item) && !IsInFetching(item))
            {
                posessedItems.Add(item);
            }
        }
        return posessedItems;
    }

    public bool IsInFetching(ItemEnum item)
    {
        return fetchingItems.Contains(item);
    }

    public bool RemoveFetching(ItemEnum item)
    {
        if (!fetchingItems.Contains(item)) return false;
        fetchingItems.Remove(item);
        return true;
    }

    public bool AddFetching(ItemEnum item)
    {
        if (fetchingItems.Contains(item)) return false;
        fetchingItems.Add(item);
        return true;
    }
}
