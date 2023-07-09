using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class QuestDisplayer : MonoBehaviour
{
    public Quest currentQuest; //{ get; private set; }

    // Internal
    private bool isLocked;
    public int displayerId;

    // Prefab Stuff
    [SerializeField] Sprite[] backgroundImages;

    // GUI
    [SerializeField] Image difficultyImage;
    [SerializeField] TextMeshProUGUI heroTypeText;
    [SerializeField] Image backgroundImage; // (Quest colour)
    [SerializeField] Image descriptionImage;
    [SerializeField] TextMeshProUGUI description;
    //[SerializeField] List<Image> guysPortraits;
    [SerializeField] Image guyPortrait;
    [SerializeField] HeroDisplay heroManager;
    [SerializeField] Button questButton;
    [SerializeField] Sprite spriteOfEmptyGuy;
    [SerializeField] QuestsManager questsManager;
    [SerializeField] Image frameImage;

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

    public void SetCurrentQuest(Quest currentQuest)
    {
        this.currentQuest = currentQuest;
        ActualizeDisplay();

    }

    public void ActualizeDisplay()
    {
        if(!isLocked)
        {
            difficultyImage.sprite = GameLibrary.GetDifficultySprite(currentQuest.difficulty);
            heroTypeText.text = GameLibrary.GetHeroTypeFromEnum(currentQuest.heroType).displayName;
            Debug.Log($"Rarity : {currentQuest.rarity}");
            Color frameColor = GameLibrary.GetRarityColor(currentQuest.rarity);
            Debug.Log($"Color : {frameColor}");
            frameImage.color = frameColor;
            frameImage.enabled = false;
            frameImage.enabled = true;
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
    }

    public void DisplayFullQuest(bool isFull)
    {
        questButton.interactable = !isFull;
    }

    /**
     * Called when the button is pressed.
     */
    public void OnQuestSelected()
    {
        Hero assigned_hero = heroManager.GetCurrentHero();
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
            questsManager.OnAnyQuestDisplayerSelected(displayerId);
            GameLibrary.NegotiationDisplay.SetNewQuest(currentQuest);
        }
        else Debug.LogError("Le héros est nul.");
    }

    /*
     * Called when any other QuestDisplayer button is pressed
     */
    public void OnOtherQuestDisplayerSelected()
    {
        if(hasHeroTemporarilyAssigned)
        {
            
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

    public void Unlock()
    {
        isLocked = false;
        questButton.interactable = true;
    }
}
