using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using Unity.VisualScripting;

public class QuestDisplayer : MonoBehaviour
{
    public Quest currentQuest; //{ get; private set; }

    // Internal
    public bool isLocked;
    public int displayerId;
    public int accessibleAtLevel = 0;

    // Prefab Stuff
    [SerializeField] Sprite[] backgroundImages;

    // GUI
    [SerializeField] Image difficultyImage;
    [SerializeField] TextMeshProUGUI heroTypeText;
    [SerializeField] TextMeshProUGUI questTypeText;
    [SerializeField] Image descriptionImage;
    [SerializeField] TextMeshProUGUI description;
    //[SerializeField] List<Image> guysPortraits;
    [SerializeField] Image guyPortrait;
    [SerializeField] Button questButton;
    [SerializeField] Sprite spriteOfEmptyGuy;
    [SerializeField] Image frameImage;

    [SerializeField] ItemDisplayer lostItemImageUI;
    [SerializeField] ItemDisplayer fetchedtemImageUI;
    [SerializeField] ItemDisplayer minorItemImageUI;
    [SerializeField] TextMeshProUGUI obtainedMoneyText;

    private bool hasHeroTemporarilyAssigned;

    private void Start()
    {
        hasHeroTemporarilyAssigned = false;
        /*foreach (Image guyPortrait in guysPortraits)
        {
            guyPortrait.sprite = spriteOfEmptyGuy;
        }*/
        guyPortrait.sprite = spriteOfEmptyGuy;
    }

    public void SetCurrentQuest(Quest newQuest)
    {
        this.currentQuest = newQuest;
        ActualizeDisplay();

    }

    public void ActualizeDisplay()
    {
        difficultyImage.sprite = GameLibrary.GetDifficultySprite(currentQuest.difficulty);
        heroTypeText.text = GameLibrary.GetHeroTypeFromEnum(currentQuest.heroType).displayName;
        questTypeText.text = currentQuest.type.displayName;
        Color frameColor = GameLibrary.GetRarityColor(currentQuest.rarity);
        frameImage.color = frameColor;
        frameImage.enabled = false;
        frameImage.enabled = true;
        if (currentQuest.hasFetchedItem)
        {
            fetchedtemImageUI.SetItem(currentQuest.fetchedItem);
        }
        else if (fetchedtemImageUI.hasItem) fetchedtemImageUI.RemoveItem();
        if(currentQuest.hasLostItem)
        {
            lostItemImageUI.SetItem(currentQuest.lostItem);
        }
        else if(lostItemImageUI.hasItem) lostItemImageUI.RemoveItem();
        minorItemImageUI.SetItem(currentQuest.minorItem);
        obtainedMoneyText.text = (currentQuest.minorItemCount * currentQuest.minorItem.goldCount).ToString();
        /*int i=0;
        int nbGuys = currentQuest.guysList.Count;
        foreach (Image guyPortrait in guysPortraits)
        {
            if (i < nbGuys) { guyPortrait.sprite = currentQuest.guysList[i].heroType.icon; }
            else { guyPortrait.sprite = spriteOfEmptyGuy;  }
            i++;
        }*/
        if (currentQuest.hasAssignedHero)
        {
            guyPortrait.sprite = currentQuest.assignedHero.heroType.icon;
            DisplayFullQuest(true);
        }
        else
        {
            guyPortrait.sprite = spriteOfEmptyGuy;
            DisplayFullQuest(false);
        }
        //DisplayFullQuest(!hasHeroTemporarilyAssigned);
    }

    public void DisplayFullQuest(bool isFull)
    {
        if (isFull) Lock();
        else Unlock();
    }

    /**
     * Called when the button is pressed.
     */
    public void OnQuestSelected()
    {
        Hero assigned_hero = GameLibrary.HeroDisplay.GetCurrentHero();
        if(assigned_hero != null && !hasHeroTemporarilyAssigned)
        {
            /*bool isAdded = currentQuest.AddGuy(assigned_hero);
            if (!isAdded) Debug.LogError("La quête est pleine !!!!!");*/
            /*else
            {
                hasHeroTemporarilyAssigned = true;
                ActualizeDisplay();
                questsManager.OnAnyQuestDisplayerSelected(displayerId);
            }*/
            currentQuest.SetHero(assigned_hero);
            hasHeroTemporarilyAssigned = true;
            ActualizeDisplay();
            GameLibrary.QuestManager.OnAnyQuestDisplayerSelected(displayerId);
            GameLibrary.NegotiationDisplay.SetNewQuest(currentQuest);
        }
        else if(assigned_hero == null) Debug.LogError("Le héros est nul.");
        else Debug.LogError("L'erreur étonnante.");
    }

    /*
     * Called when any other QuestDisplayer button is pressed
     */
    public void OnOtherQuestDisplayerSelected()
    {
        if(hasHeroTemporarilyAssigned)
        {
            Debug.Log($"{gameObject.name} :Oui, c'est moi.");
            //currentQuest.RemoveGuy();
            currentQuest.UnsetHero();
            ActualizeDisplay();
            hasHeroTemporarilyAssigned = false;
        }
    }

    /**
     * Triggered when the Next Turn button is pressed.
     */
    public void OnNextTurn()
    {
        hasHeroTemporarilyAssigned = false;
    }

    public void Lock()
    {
        isLocked = true;
        questButton.interactable = false;
    }

    public bool IsAccessibleAtThisLevel()
    {
        return (GameLibrary.PlayerXpManager.current this.accessibleAtLevel > 0);
    }

    public void Unlock()
    {
        isLocked = false;
        questButton.interactable = true;
    }
}
