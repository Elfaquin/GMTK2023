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
    [SerializeField] private TextMeshProUGUI levelUI;
    [SerializeField] private Hero currentHero;

    public void setXpBar(float value)
    {
        xpBar.value = value;
    }

    public void setLevelUI(int value)
    {
        levelUI.text = value.ToString();
    }

    public void displayHero(Hero hero)
    {
        currentHero = hero;
        HeroType heroType = hero.heroType;
        if (hero.level > heroType.maxLevel) throw new System.Exception("Tu te fous de ma gueule avec ce level là");
        heroSpriteImage.sprite = heroType.characterSprites[hero.level];
        Debug.Log($"Normalized XP : {hero.heroType.GetXpNormalized(hero.xp, hero.level)}");
        setXpBar(hero.heroType.GetXpNormalized(hero.xp, hero.level));
        heroNameUI.text = hero.displayName;
        heroTypeUI.text = hero.heroType.swaggName;
    }

    public void NextHero()
    {
        Hero newHero = Hero.RandomHero();
        displayHero(newHero);
    }

    public Hero GetCurrentHero()
    {
        return currentHero;
    }
}
