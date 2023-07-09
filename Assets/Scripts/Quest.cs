using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public static int NUMBER_QUEST_ID_PROVIDER = 0;

    // These start at 1
    //static int MaximumGuys = 3;
    static int maxDifficulty = 5;
    static int nbRarity = 6;

    // Attributes
    public int questId; // Unique ID 
    public string title;
    public string description;
    public QuestType type;
    public bool hasLostItem;
    public Item lostItem;
    public bool hasFetchedItem;
    public Item fetchedItem;
    public MinorItem minorItem;
    public int minorItemCount;
    //public List<Hero> guysList = new(); // Replace with Connard object
    public Hero assignedHero;
    public bool hasAssignedHero;
    public int difficulty;
    public HeroTypeEnum heroType;
    public int rarity;
    public bool isKillingQuest;
    public int xpOnSuccess;

    /**
     * Insatantiates a Quest with random attributes.
     */
    public static Quest CreateRandomQuest()
    {
        Quest newQuest = new()
        {
            questId = NUMBER_QUEST_ID_PROVIDER++,
            title = "Totally Random Title",
            description = "Totally Random Description"
        };

        QuestTypeEnum randomType = GameLibrary.RandomQuestType();
        newQuest.type = GameLibrary.GetQuestTypeFromEnum(randomType);


        switch (randomType)
        {
            
            case QuestTypeEnum.Dungeon:
                newQuest.isKillingQuest = true;
                ItemEnum missingItem = GameLibrary.RandomItemFromMissing();
                if (missingItem == ItemEnum.NullItem)
                    newQuest.hasFetchedItem = false;
                else
                {
                    newQuest.hasFetchedItem = true;
                    newQuest.fetchedItem = GameLibrary.GetItemFromEnum(missingItem);
                }
                break;
            case QuestTypeEnum.Kill:
                newQuest.isKillingQuest = true;
                newQuest.hasFetchedItem = false;
                break;
            case QuestTypeEnum.Fetch:
                newQuest.isKillingQuest = false;
                ItemEnum missingItem2 = GameLibrary.RandomItemFromMissing();
                if (missingItem2 == ItemEnum.NullItem)
                    newQuest.hasFetchedItem = false;
                else
                {
                    newQuest.hasFetchedItem = true;
                    newQuest.fetchedItem = GameLibrary.GetItemFromEnum(missingItem2);
                }
                break;
            case QuestTypeEnum.Speak:
                newQuest.isKillingQuest = false;
                newQuest.hasFetchedItem = false;
                break;
        }
        float hasGivingItem = UnityEngine.Random.Range(0.0f, 1.0f);
        if(hasGivingItem > GameLibrary.ChancesToGetItemGivingQuest)
        {
            ItemEnum possessedItem = GameLibrary.RandomItemFromPossessed();
            newQuest.hasLostItem = possessedItem != ItemEnum.NullItem;
            if (newQuest.hasLostItem)
            {
                newQuest.lostItem = GameLibrary.GetItemFromEnum(possessedItem);

            }
        }

        newQuest.difficulty = UnityEngine.Random.Range(0, maxDifficulty);
        newQuest.minorItem = GameLibrary.GetMinorItemFromEnum(GameLibrary.RandomMinorItem());
        newQuest.minorItemCount = newQuest.difficulty+1;
        newQuest.heroType = GameLibrary.RandomHeroType();
        newQuest.rarity = UnityEngine.Random.Range(0, nbRarity);
        newQuest.hasAssignedHero = false;

        newQuest.xpOnSuccess = 0;

        return newQuest;
    }

    public bool isImpossible()
    {
        // Condition 1 : if the fetched item is already in the inventory
        if(hasFetchedItem)
        {
            if(GameLibrary.InventoryManager.HasItem(fetchedItem.enumValue) == false)
            {
                return true;
            }
        }
        // Condition 2 : if the given item is not in the inventory
        if (hasLostItem)
        {
            if (GameLibrary.InventoryManager.HasItem(lostItem.enumValue) == true)
            {
                return true;
            }
        }
        return false;
    }

    /*public bool AddGuy(Hero guy)
    {
        if (guysList.Count < MaximumGuys)
        {
            guysList.Add(guy);
            return true;
        }
        return false;
    }*/

    public void SetHero(Hero hero)
    {
        if (hero == null) throw new System.Exception("Le héros est nul. Nul Giroud.");
        assignedHero = hero;
        hasAssignedHero = true;
    }

    /*public bool RemoveGuy()
    {
        if (guysList.Count > 0)
        {
            guysList.RemoveAt(guysList.Count - 1);
            return true;
        }
        else
        {
            Debug.LogWarning("Can't remove the guy : there are no guys.");
            return false;
        }
    }*/

    public bool UnsetHero()
    {
        if (assignedHero == null)
        {
            Debug.LogWarning("No hero to unset.");
            return false;
        }
        else
        {
            assignedHero = null;
            hasAssignedHero = false;
            return true;
        }
    }

    /*public bool guysAreFull()
    {
        return (guysList.Count >= MaximumGuys);
    }*/

    public bool HasHeroAssigned()
    {
        return assignedHero != null;
    }
}