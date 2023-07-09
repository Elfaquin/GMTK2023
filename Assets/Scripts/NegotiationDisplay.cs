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
            givenItemText.text = $"You give a {quest.lostItem.displayName}";
        }
        else
        {
            if(givenItem.hasItem) givenItem.RemoveItem();
            givenItemText.text = "No item given.";
        }
        minorItem.SetItem(quest.minorItem);
        goldEquivalent.text = quest.minorItem.goldCount.ToString();
        minorItemName.text = quest.minorItem.displayName;
        if(quest.hasFetchedItem)
        {
            majorItem.SetItem(quest.fetchedItem);
            majorItemName.text = quest.fetchedItem.displayName;
        }
        ComputeSatisfaction();
    }

    public void OnGoldSliderValueChanged()
    {
        givenGoldCount = (int)(givenGold.value  * GameLibrary.InventoryManager.gold * currentQuest.minorItemCount);
        givenGoldText.text = $"{givenGoldCount}/{GameLibrary.InventoryManager.gold}";
        ComputeSatisfaction();
    }

    public void OnXpSliderValueChanged()
    {
        givenXpCount = (int)(givenXp.value * GameLibrary.InventoryManager.xp);
        givenXpText.text = $"{givenXpCount}/{GameLibrary.InventoryManager.xp}";
        ComputeSatisfaction();
    }

    public void ComputeSatisfaction()
    {
        float targetSatisfaction = 0;
        targetSatisfaction += (int)(Mathf.Pow(currentQuest.assignedHero.level, 2.0f));
        float achievedSatisfaction = 0;
        if(currentQuest.hasFetchedItem)
        {
            achievedSatisfaction += 30;
        }
        achievedSatisfaction += givenGoldCount;
        achievedSatisfaction += givenXpCount;
        heroSatisfaction = Mathf.Min(1.0f, achievedSatisfaction/targetSatisfaction);
        if(heroSatisfaction >= 0.995f)
        {
            SetReady();
        }
        else
        {
            SetNotReady();
        }
    }

    public void OnValidateButton()
    {
        GameLibrary.PlayerXpManager.RemoveXp(givenXpCount);
        GameLibrary.PlayerGoldManager.RemoveGold(givenGoldCount);
        GameLibrary.HeroDisplay.GetCurrentHero();

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
        questTitle.enabled = false;
        questDescription.enabled = false;
        questType.enabled = false;
        givenXpText.enabled = false;
        givenGoldText.enabled = false;
        goldEquivalent.enabled = false;
        minorItem.enabled = false;
        majorItem.enabled = false;
        
        givenItem.Hide();
        minorItem.Hide();
        majorItem.Hide();
        
        satisfactionLogoUI.enabled = false;
        validateButton.enabled = false;
        givenStuffTitle.enabled = false;
        receivedStuffTitle.enabled = false;
        minorItemName.enabled = false;
        majorItemName.enabled = false;
        givenItemText.enabled = false;
        questHeroType.enabled = false;
        questHeroTypeImage.enabled = false;
        questHeroTypeImage.gameObject.transform.parent.GetComponent<Image>().enabled = false; // Background
        questTypeImage.enabled = false;
        givenXp.gameObject.SetActive(false);
        givenGold.gameObject.SetActive(false);
        satisfactionSlider.gameObject.SetActive(false);

        randomText1.enabled = false;
        randomText2.enabled = false;

        validateButton.gameObject.SetActive(false);
    }

    public void Show()
    {
        isDisplayed = true;
        questTitle.enabled = true;  
        questDescription.enabled = true;
        givenXpText.enabled = true;
        givenGoldText.enabled = true;
        goldEquivalent.enabled = true;
        minorItem.enabled = true;
        majorItem.enabled = true;
        givenGold.enabled = true;
        givenXp.enabled = true;
        givenItem.Show();
        minorItem.Show();
        majorItem.Show();
        satisfactionSlider.enabled = true;
        satisfactionLogoUI.enabled = true;
        validateButton.enabled = true;
        givenStuffTitle.enabled = true;
        receivedStuffTitle.enabled = true;
        minorItemName.enabled = true;
        majorItemName.enabled = true;
        givenItemText.enabled = true;
        questHeroType.enabled = true;
        questHeroTypeImage.enabled = true;
        questHeroTypeImage.gameObject.transform.parent.GetComponent<Image>().enabled = true; // Background
        questTypeImage.enabled= true;
        questType.enabled = true;
        givenXp.gameObject.SetActive(true);
        givenGold.gameObject.SetActive(true);
        satisfactionSlider.gameObject.SetActive(true);

        randomText1.enabled = true;
        randomText2.enabled = true;

        validateButton.gameObject.SetActive(true);
    }
}
