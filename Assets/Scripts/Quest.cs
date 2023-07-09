using UnityEngine;

[System.Serializable]
public class Quest
{
    public static int NUMBER_QUEST_ID_PROVIDER = 0;


	private static string[] DUNGEON_NAMES = { "The icy cave", "The lumberjack's home", "Santas's heavy chest", "Dark alley", "Elves's maison", "Paris", "Lalaland", "The montain of ashes." };
	private static string[] MOB_NAMES = { "angry wolves", "blue gobelins", "black trehants", "Chtulu the Destroyer of Worlds", "small worms", "angry birds", "Perier", "Heavy godzillas", "Santa's assitants", "plague doctors", "small bears", "cow" };
    private static string random_str(string[] a) {
        return a[Random.Range(0, a.Length)];
    }

	// These start at 1
	//static int MaximumGuys = 3;
	static int maxDifficulty = 5;
    static int nbRarity = 6;

    static float chanceFecthCat = 0.12f;

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
    public bool isCat;
    public int xpOnSuccess;
    public float chancesOfSuccess;




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
		newQuest.isCat = false;
		if(GameLibrary.InventoryManager.CanHaveACat() && Random.value <= chanceFecthCat) {
            randomType = QuestTypeEnum.SaveTheCat;
			newQuest.isCat = true;
		}


        newQuest.type = GameLibrary.GetQuestTypeFromEnum(randomType);

        

        switch(randomType) {

            case QuestTypeEnum.Dungeon:
                newQuest.isKillingQuest = true;
                ItemEnum missingItem = GameLibrary.RandomItemFromNotPossessedNorFetched();
				newQuest.title = "Ransack the dungeon of '"+ random_str(DUNGEON_NAMES) + "'";
				newQuest.description = "Destroying this dungeon seems like a good oppotunity.";
				if(missingItem == ItemEnum.NullItem) {
                    newQuest.hasFetchedItem = false;
                } else {
                    newQuest.hasFetchedItem = true;
                    newQuest.fetchedItem = GameLibrary.GetItemFromEnum(missingItem);
					newQuest.description = "While you're doing this, fetch the " + newQuest.fetchedItem.displayName + " at the same time.";
				}
                break;
            case QuestTypeEnum.Kill:
                newQuest.isKillingQuest = true;
                newQuest.hasFetchedItem = false;
				newQuest.title = "Kill "+Random.Range(3, 27)+" " + random_str(MOB_NAMES) + ".";
				newQuest.description = "I hate those things.";
				break;
            case QuestTypeEnum.Fetch:
                newQuest.isKillingQuest = false;
				ItemEnum missingItem2 = GameLibrary.RandomItemFromNotPossessedNorFetched();
                if (missingItem2 == ItemEnum.NullItem)
                    newQuest.hasFetchedItem = false;
                else
                {
                    newQuest.hasFetchedItem = true;
                    newQuest.fetchedItem = GameLibrary.GetItemFromEnum(missingItem2);
					newQuest.title = "Fetch the " + newQuest.fetchedItem.displayName;
					newQuest.description = "I gave it to another adventurer before... Could you go and get it ? Pretty please ?";
				}
                break;
			case QuestTypeEnum.Speak:
				newQuest.title = "Speak to... some other NPC.";
				newQuest.description = "You know, I lost an item, and the other dude need another one too... I won't see this player before some time.";
				newQuest.isKillingQuest = false;
				newQuest.hasFetchedItem = false;
				break;
			case QuestTypeEnum.SaveTheCat:
				newQuest.isKillingQuest = true;
				newQuest.hasFetchedItem = false;
				newQuest.title = "Save the cat of destiny !";
				newQuest.description = "You've heard of a cat... Send this player fetch it for you !";
				break;
		}

        if(Random.value > GameLibrary.ChancesToGetItemGivingQuest)
        {
            ItemEnum possessedItem = GameLibrary.RandomItemFromPossessed();
            newQuest.hasLostItem = possessedItem != ItemEnum.NullItem;
            if (newQuest.hasLostItem)
            {
                newQuest.lostItem = GameLibrary.GetItemFromEnum(possessedItem);

            }
        }

        newQuest.difficulty = Random.Range(0, Mathf.Min(maxDifficulty, GameLibrary.PlayerXpManager.currentLevel+3));
        newQuest.minorItem = GameLibrary.GetMinorItemFromEnum(GameLibrary.RandomMinorItem());
        newQuest.minorItemCount = newQuest.difficulty+1;
        newQuest.heroType = GameLibrary.RandomHeroType();
        newQuest.rarity = Random.Range(0, 5);
        if(newQuest.isCat) {
            newQuest.rarity = 5;
        }
        newQuest.hasAssignedHero = false;
        newQuest.chancesOfSuccess = 1.0f;

        newQuest.xpOnSuccess = 0;

        return newQuest;
    }

    public bool isImpossible()
    {
        // Condition 1 : if the fetched item is already in the inventory
        // Condition 3 : if the fetched item is already in fetching
        if (hasFetchedItem)
        {
            if(GameLibrary.InventoryManager.HasItem(fetchedItem.enumValue) == true) 
            {
                Debug.LogWarning($"Quest impossible : wants to fetch the possessed item {fetchedItem.enumValue}");
                return true;
            }
            if (GameLibrary.InventoryManager.IsInFetching(fetchedItem.enumValue) == true)
            {
                Debug.LogWarning($"Quest impossible : wants to fetch the fetched item {fetchedItem.enumValue}");
                return true;
            }
        }
        // Condition 2 : if the given item is not in the inventory
        if (hasLostItem)
        {
            if (GameLibrary.InventoryManager.HasItem(lostItem.enumValue) == false)
            {
                Debug.LogWarning($"Quest impossible : wants to give the item {lostItem.enumValue}");
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