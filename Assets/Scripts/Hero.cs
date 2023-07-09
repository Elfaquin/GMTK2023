using UnityEngine;

public class Hero
{
    public static int numberOfHerosGenerated;
    public HeroType heroType;
    public int level;
    public int heroId;
    public bool isDead;
    public string displayName;
    [SerializeField] private Quest assignedQuest;
    public bool hasQuestAssigned;
    
    public float xp { get; private set; }

    private static string[] HERO_NAMES = {
        "fastAnCurious", "BadKarma", "AverageStudent", "IwilleatYou", "GimePasta", "jamailun", "Elfaquin", "perier", "Godzilla2023", "2fast4u", "iamicredible",
        "ThaDuck", "xx_jhon_xx", "theo76523", "Steve", "TheMineGuy", "Piggy308", "SP64_luigi", "paulito_2", "my_usrn", "haroldFinch", "ThePaper", "ThinAnUgly",
        "RedJhon", "waterlyguy", "melonman_xx", "AllGoodNAmesRgone", "block_me_pls", "iam_yourFriend", "45_dungeonbreaker", "toastedEggs", "BlueCactus",
        "Ubikuity", "PurpleLand", "IStompYou", "AppleJeans", "where_choko_milk", "SaintBroseph", "FrostedBanana", "fatmanTheBat", "good_sandwich", "scp720",
        "BreadPitt", "OmnipotentMe", "earthIsRound", "momIsNice", "sendFR", "Napoleon69", "PastaSauced", "PinkPenguin", "BronCorn", "melonSmasher", "unic0rns"
    };

    public static Hero RandomHero()
    {
        Hero generatedHero = new();
        generatedHero.heroId = numberOfHerosGenerated++;
        HeroTypeEnum heroTypeEnum = GameLibrary.RandomHeroType();
        generatedHero.heroType = GameLibrary.GetHeroTypeFromEnum(heroTypeEnum);
        generatedHero.level = 1;
        generatedHero.xp = 0;
        generatedHero.displayName = HERO_NAMES[Random.Range(0, HERO_NAMES.Length)];
        generatedHero.isDead = false;
        generatedHero.hasQuestAssigned = false;
        return generatedHero;
    }

    public void AssignQuest(Quest assignedQuest)
    {
        this.assignedQuest = assignedQuest;
        this.assignedQuest.chancesOfSuccess = ComputeChancesOfSuccess(assignedQuest);
        hasQuestAssigned = true;
    }

    public float ComputeChancesOfSuccess(Quest assignedQuest)
    {
        float chancesOfSuccess = 1.0f;
        float randomFactor = Random.Range(-0.1f, 0.1f);
        chancesOfSuccess = Mathf.Max(0.08f, level/(assignedQuest.difficulty*2)-randomFactor);
        return chancesOfSuccess;
    }

    public bool ResolveQuest()
    {
        if (this.assignedQuest == null)
        {
            Debug.LogWarning("You are trying to resolve a null quest.");
            return false;
        }
        float success = UnityEngine.Random.Range(0, assignedQuest.chancesOfSuccess);
        if (success > 0)
        {
            LogsWindow.Event_QuestSucceeded(assignedQuest);
            GameLibrary.PlayerGoldManager.AddGold(assignedQuest.minorItem.goldCount * assignedQuest.minorItemCount);
            if(assignedQuest.hasFetchedItem)
            {
                GameLibrary.InventoryManager.AddItem(assignedQuest.fetchedItem.enumValue);
            }
            this.AddXp (GameLibrary.StaticXpGainedByHeroPerQuest * (assignedQuest.difficulty + 1));
            this.AddXp (assignedQuest.xpOnSuccess);
            return true;
        }
        else
        {
            if(assignedQuest.isKillingQuest)
            {
                LogsWindow.Event_QuestFailed(assignedQuest);
                LogsWindow.Event_HeroDied(this);
                Die();
            }
            else
            {
                LogsWindow.Event_QuestFailed(assignedQuest);
            }
            return false;
        }
    }

    void Die()
    {
        isDead = true;
    }

    void AddXp(int xp)
    {
        this.xp += xp;
        while (this.xp + xp > GetNextLevelXp())
        {
            LevelUp();
        }
    }

    public int GetNextLevelXp()
    {
        float ratio = GameLibrary.HeroXpCurve.Evaluate((float)(level + 1) / GameLibrary.HerosMaxLevel);
        return (int)(ratio * GameLibrary.HerosXpAtMaxLevel);
    }

    void LevelUp()
    {
        this.level += 1;
        LogsWindow.Event_HeroLevelup(this);
    }
}
