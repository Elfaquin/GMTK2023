using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeroDisplay : MonoBehaviour
{
    [SerializeField] private Image heroSpriteImage;
    [SerializeField] private TextMeshProUGUI heroNameUI;
    [SerializeField] private TextMeshProUGUI heroTypeUI;
    [SerializeField] private Slider xpBar;
    [SerializeField] private Hero currentHero;

    public TextMeshProUGUI heroCurrentLevel;
    public TextMeshProUGUI heroCurrentXP;
    public TextMeshProUGUI xpForNextLevel;

    private void Start()
    {
        AudioManager.PlaySoundEffect(AudioManager.SoundEffect.Greeting);
    }

    public void setXpBar(float value)
    {
        xpBar.value = value;
    }

    public void HideDisplay()
    {
        currentHero = null;
        heroSpriteImage.enabled = false;
    }

    public void displayHero(Hero hero)
    {
        heroSpriteImage.enabled = true;
        currentHero = hero;
        HeroType heroType = hero.heroType;
        if (hero.level > GameLibrary.HerosMaxLevel) throw new System.Exception("Tu te fous de ma gueule avec ce level là");

        if(hero.level > 2 * GameLibrary.HerosMaxLevel / 3) {
			heroSpriteImage.sprite = heroType.characterSprites[2];
		} else if(hero.level > GameLibrary.HerosMaxLevel / 3) {
			heroSpriteImage.sprite = heroType.characterSprites[1];
		} else {
			heroSpriteImage.sprite = heroType.characterSprites[0];
		}

        setXpBar(hero.heroType.GetXpNormalized(hero.xp, hero.level));
        heroNameUI.text = hero.displayName;
        heroTypeUI.text = hero.heroType.swaggName;
        heroCurrentLevel.text = hero.level.ToString();
        heroCurrentXP.text = hero.xp.ToString();
        xpForNextLevel.text = hero.GetNextLevelXp().ToString();
    }

    public void NextHero()
    {
        Hero newHero = Hero.RandomHero();
        displayHero(newHero);
    }

    public Hero GetCurrentHero()
    {
        if(currentHero == null) return null;
        Debug.Log($"GetCurrentHero called for {currentHero.displayName}");
        return currentHero;
    }
}
