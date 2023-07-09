using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameLibrary : MonoBehaviour
{
    private static GameLibrary Instance;

    [SerializeField] private HeroType wizzezer;
    [SerializeField] private HeroType chevalier;
    [SerializeField] private HeroType crook;

    [SerializeField] private Item Clef;
    [SerializeField] private Item Clou;
    [SerializeField] private Item Marto;
    [SerializeField] private Item Scie;
    [SerializeField] private Item Tournevis;

    [SerializeField] private MinorItem Podlou;
    [SerializeField] private MinorItem Saucisse;
    [SerializeField] private MinorItem Perrier;

    [SerializeField] private QuestType Fetch;
    [SerializeField] private QuestType Kill;
    [SerializeField] private QuestType Dungeon;
    [SerializeField] private QuestType Speak;

    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private NegotiationDisplay negotiationDisplay;
    [SerializeField] private PlayerGoldManager playerGoldManager;
    [SerializeField] private PlayerXpManager playerXpManager;
    [SerializeField] private List<Sprite> difficultySprites;
    [SerializeField] private List<Color> questRarityColors;
    [SerializeField] private HeroDisplay heroDisplay;
    [SerializeField] private QuestsManager questManager;
    [SerializeField] private HeroQueue heroQueue;

    [SerializeField] private int staticXpGainedByHeroPerQuest;
    [SerializeField] private AnimationCurve heroXpCurve;
    [SerializeField] private int herosMaxLevel;
    [SerializeField] private int herosXpAtMaxLevel;

    public static InventoryManager InventoryManager { get { return Instance.inventoryManager; } }
    public static NegotiationDisplay NegotiationDisplay { get { return Instance.negotiationDisplay; } }
    public static float ChancesToGetItemGivingQuest { get { return Instance.chancesToGetItemGivingQuest; } }
    public static PlayerGoldManager PlayerGoldManager { get { return Instance.playerGoldManager; } }
    public static PlayerXpManager PlayerXpManager { get { return Instance.playerXpManager; } }
    public static HeroDisplay HeroDisplay { get { return Instance.heroDisplay; } }
    public static HeroQueue HeroQueue { get { return Instance.heroQueue; } }
    public static QuestsManager QuestManager { get { return Instance.questManager; } }

    public static int StaticXpGainedByHeroPerQuest { get { return Instance.staticXpGainedByHeroPerQuest; } }
    public static AnimationCurve HeroXpCurve { get { return Instance.heroXpCurve; } }
    public static int HerosMaxLevel { get { return Instance.herosMaxLevel; } }
    public static int HerosXpAtMaxLevel { get { return Instance.herosXpAtMaxLevel; } }


    // Various Game Design parameters
    [SerializeField] private float chancesToGetItemGivingQuest = 0.4f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("wtf les amis, ya plusieurs connards");
        }
        Instance = this;
    }

    public static Sprite GetDifficultySprite(int difficultyLevel)
    {
        if (difficultyLevel > Instance.difficultySprites.Count) throw new System.Exception("Cette difficulté, tu te la mets où je pense.");
        return Instance.difficultySprites[difficultyLevel];
    }

    public static Color GetRarityColor(int rarity)
    {
        if (rarity > Instance.questRarityColors.Count) return Instance.questRarityColors[0];
        else return Instance.questRarityColors[rarity];
    }

    public static HeroType GetHeroTypeFromEnum(HeroTypeEnum type) 
    {
        switch(type)
        {
            case HeroTypeEnum.Wizzezer: return Instance.wizzezer;
            case HeroTypeEnum.Chevalier: return Instance.chevalier;
            case HeroTypeEnum.Crook: return Instance.crook;
            default: throw new System.Exception("aaaaaaaaaaaaaaaaaaaaa");
        }
    }

    public static HeroTypeEnum RandomHeroType()
    {
        int randomType = Random.Range(1, 4);
        switch(randomType)
        {
            case 1: return HeroTypeEnum.Wizzezer;
            case 2: return HeroTypeEnum.Chevalier;
            case 3: return HeroTypeEnum.Crook;
        }
        return HeroTypeEnum.Wizzezer;
    }

    public static Item GetItemFromEnum(ItemEnum itemEnum)
    {
        switch(itemEnum)
        {
            case ItemEnum.Clef: return Instance.Clef;
            case ItemEnum.Clou: return Instance.Clou;
            case ItemEnum.Marto : return Instance.Marto;
            case ItemEnum.Scie: return Instance.Scie;
            case ItemEnum.Tournevis : return Instance.Tournevis;
            case ItemEnum.NullItem: throw new System.Exception("Le nullitem des familles");
            default: throw new System.Exception("bbbbbbbbbbbbbbbbbbb");
        }
    }

    public static ItemEnum RandomItem()
    {
        int randomType = Random.Range(1, 6);
        switch (randomType)
        {
            case 1: return ItemEnum.Clef;
            case 2: return ItemEnum.Clou;
            case 3: return ItemEnum.Marto;
            case 4: return ItemEnum.Scie;
            case 5: return ItemEnum.Tournevis;
        }
        return ItemEnum.Clef;
    }

    public static ItemEnum RandomItemFromMissing()
    {
        List<ItemEnum> missingItems = InventoryManager.GetMissingItems();
        if (missingItems.Count == 0) return ItemEnum.NullItem;
        int randomItem = Random.Range(0, missingItems.Count);
        return missingItems[randomItem];
    }

    public static ItemEnum RandomItemFromPossessed()
    {
        List<ItemEnum> possessedItems = InventoryManager.GetPosessedItems();
        if(possessedItems.Count == 0) return ItemEnum.NullItem;
        int randomItem = Random.Range(0, possessedItems.Count);
        return possessedItems[randomItem];
    }

    public static MinorItem GetMinorItemFromEnum(MinorItemEnum minorItemEnum)
    {
        switch(minorItemEnum) 
        {
            case MinorItemEnum.Podlou: return Instance.Podlou;
            case MinorItemEnum.Saucisse: return Instance.Saucisse;
            case MinorItemEnum.Perrier: return Instance.Perrier;
            default: throw new System.Exception("cccccccccccccccccccc");
        }
    }

    public static MinorItemEnum RandomMinorItem()
    {
        int randomType = Random.Range(1, 4);
        switch (randomType)
        {
            case 1: return MinorItemEnum.Podlou;
            case 2: return MinorItemEnum.Saucisse;
            case 3: return MinorItemEnum.Perrier;
        }
        return MinorItemEnum.Podlou;
    }

    public static QuestType GetQuestTypeFromEnum(QuestTypeEnum questTypeEnum)
    {
        switch(questTypeEnum)
        {
            case QuestTypeEnum.Dungeon: return Instance.Dungeon;
            case QuestTypeEnum.Kill: return Instance.Kill; 
            case QuestTypeEnum.Fetch: return Instance.Fetch;
            case QuestTypeEnum.Speak: return Instance.Speak;
            default: throw new System.Exception("ddddddddddddddddddd");
        }
    }

    public static QuestTypeEnum RandomQuestType()
    {
        int randomType = Random.Range(1, 5);
        switch (randomType)
        {
            case 1: return QuestTypeEnum.Dungeon;
            case 2: return QuestTypeEnum.Kill;
            case 3: return QuestTypeEnum.Fetch;
            case 4: return QuestTypeEnum.Speak;
        }
        return QuestTypeEnum.Dungeon;
    }

    public static void DoAfter(System.Action action)
    {
        Instance.StartCoroutine(Instance.RunTask(action));
    }

    private IEnumerator RunTask(System.Action action)
    {
        yield return new WaitForEndOfFrame();
        action();
    }
}