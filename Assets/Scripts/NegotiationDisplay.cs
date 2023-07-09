using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class NegotiationDisplay : MonoBehaviour
{
    [SerializeField] bool isDisplayed;

    public Image panelHider;

    public TextMeshProUGUI randomText1;
    public TextMeshProUGUI randomText2;

    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questDescription;
    public TextMeshProUGUI questType;
    public Image questTypeImage;
    public TextMeshProUGUI questHeroType;
    public Image questHeroTypeImage;

    public TextMeshProUGUI givenStuffTitle;
    public Slider givenXp;
    public Slider givenGold;
    int givenXpCount;
    int givenGoldCount;
    public TextMeshProUGUI givenXpText;
    public TextMeshProUGUI givenGoldText;
    public ItemDisplayer givenItem;
    public TextMeshProUGUI givenItemText;

    public TextMeshProUGUI receivedStuffTitle;
    public ItemDisplayer minorItem;
    public TextMeshProUGUI goldEquivalent;
    public TextMeshProUGUI minorItemName;
    public ItemDisplayer majorItem;
    public TextMeshProUGUI majorItemName;

    public Quest currentQuest;
    public Slider satisfactionSlider;
    public Image satisfactionLogoUI;
    public Sprite satisfiedLogo;
    public Sprite unsatisfiedLogo;
    public Button validateButton;

    public TextMeshProUGUI chancesOfSuccessUI;

    public float heroSatisfaction;

    // Start is called before the first frame update
    void Start()
    {
        currentQuest = null;
        isDisplayed = false;
        validateButton.interactable = false;
        Hide();
    }

    public void SetNewQuest(Quest quest)
    {
        Reset();
        DisplayQuest(quest);
    }


    private void Reset()
    {
        return;
    }

    void DisplayQuest(Quest quest)
    {
        Show();
        currentQuest = quest;
        questTitle.text = quest.title;
        questDescription.text = quest.description;
        questType.text = "Quest type: " + quest.type.displayName;
        questTypeImage.sprite = quest.type.sprite;
        questHeroType.text = GameLibrary.GetHeroTypeFromEnum(quest.heroType).displayName;
        questHeroTypeImage.sprite = GameLibrary.GetHeroTypeFromEnum(quest.heroType).icon;
        givenXpCount = 0;
        givenGoldCount = 0;
        givenXp.value = 0;
        givenGold.value = 0;
        givenXpText.text = givenXpCount.ToString();
        givenGoldText.text = givenGoldCount.ToString();
        if(quest.hasLostItem)
        {
            givenItem.SetItem(quest.lostItem);
            givenItemText.text = $"The hero will receive your {quest.lostItem.displayName} as a reward.";
        }
        else
        {
            if(givenItem.hasItem) givenItem.RemoveItem();
            givenItemText.text = "The hero won't receive any item as a reward.";
        }
        minorItem.SetItem(quest.minorItem);
        goldEquivalent.text = (quest.minorItem.goldCount  * quest.minorItemCount).ToString();
        minorItemName.text = $"The hero will bring you {quest.minorItem.displayName} that you can sell.";
        if(quest.hasFetchedItem)
        {
            majorItem.SetItem(quest.fetchedItem);
            majorItemName.text = $"The hero will fetch a {quest.fetchedItem.displayName} for you.";
        }
        else
        {
            if (majorItem.hasItem) majorItem.RemoveItem();
            majorItemName.text = "You won't receive any item.";
        }

        float chancesOfSucces = Mathf.Round(GameLibrary.HeroDisplay.GetCurrentHero().ComputeChancesOfSuccess(quest) * 100.0f);
        chancesOfSuccessUI.text = $"Chances of success : {Mathf.Max(chancesOfSucces-5, 8)}% - {Mathf.Min(100,chancesOfSucces+5)}%";

        ComputeSatisfaction();
    }

    public void OnGoldSliderValueChanged()
    {
        givenGoldCount = (int)(givenGold.value  * GameLibrary.InventoryManager.gold);
        givenGoldText.text = $"{givenGoldCount}/{GameLibrary.InventoryManager.gold}";
        ComputeSatisfaction();
    }

    public void OnXpSliderValueChanged()
    {
        Debug.Log($"Available XP : {GameLibrary.PlayerXpManager.currentXp}");
        givenXpCount = (int)(givenXp.value * GameLibrary.PlayerXpManager.currentXp);
        givenXpText.text = $"{givenXpCount}/{GameLibrary.PlayerXpManager.currentXp}";
        ComputeSatisfaction();
    }

    public void ComputeSatisfaction()
    {
        float targetSatisfaction = 10;
        targetSatisfaction += (int)(Mathf.Pow(currentQuest.assignedHero.level, 2.0f));
        float achievedSatisfaction = 0;
        if(currentQuest.hasLostItem)
        {
            achievedSatisfaction += 30;
        }
        achievedSatisfaction += givenGoldCount;
        achievedSatisfaction += givenXpCount;
        heroSatisfaction = Mathf.Min(1.0f, achievedSatisfaction/targetSatisfaction);
        satisfactionSlider.value = heroSatisfaction;
        //Debug.Log($"ComputeSatisfaction : Target {targetSatisfaction}, got {achievedSatisfaction}");
        if (heroSatisfaction >= 0.995f)
        {
            SetReady();
        }
        else
        {
            SetNotReady();
        }
    }

    public float ComputeChancesOfSuccess()
    {
        return 1.0f;
    }

    public void OnValidateButton()
    {
        GameLibrary.PlayerXpManager.RemoveXp(givenXpCount);
        GameLibrary.PlayerXpManager.AddXp(GameLibrary.StaticXpGainedByPlayerPerQuest);
        GameLibrary.PlayerGoldManager.RemoveGold(givenGoldCount);
        GameLibrary.HeroQueue.AssignQuestToFirstHero(currentQuest);
        GameLibrary.HeroQueue.NextQueueTurn();
        if(currentQuest.hasLostItem)
        {
            GameLibrary.InventoryManager.RemoveItem(currentQuest.lostItem.enumValue);
        }
        if(currentQuest.hasFetchedItem)
        {
            GameLibrary.InventoryManager.AddFetching(currentQuest.fetchedItem.enumValue);
        }
        currentQuest = null;
        GameLibrary.QuestManager.RerollQuests();
        Hide();
    }

    public void SetReady()
    {
        satisfactionLogoUI.sprite = satisfiedLogo;
        validateButton.interactable = true;
    }

    public void SetNotReady()
    {
        satisfactionLogoUI.sprite = unsatisfiedLogo;
        validateButton.interactable = false;
    }

    public void Hide()
    {
        isDisplayed = false;
        panelHider.enabled = true;
    }

    public void Show()
    {
        isDisplayed = true;
        panelHider.enabled = false;
    }
}
