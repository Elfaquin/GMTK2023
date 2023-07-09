using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero
{
    public HeroType heroType;
    public int level;
    public string displayName;
    public float xp { get; private set; }

    public static Hero RandomHero()
    {
        Hero generatedHero = new Hero();
        HeroTypeEnum heroTypeEnum = GameLibrary.RandomHeroType();
        generatedHero.heroType = GameLibrary.GetHeroTypeFromEnum(heroTypeEnum);
        generatedHero.level = Random.Range(0, generatedHero.heroType.maxLevel + 1);
        generatedHero.xp = Random.Range(0, generatedHero.heroType.GetXpForNextLevel(generatedHero.level));
        generatedHero.displayName = "Totally Random Name";
        return generatedHero;
    }
}
