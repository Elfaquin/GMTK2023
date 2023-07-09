using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroType", menuName = "ScriptableObjects/HeroType")]
public class HeroType : ScriptableObject
{
    public AnimationCurve xpCurve;

    public string displayName;
    public string swaggName;
    public Sprite icon;
    public int maxLevel;
    public float xpFactor;

    public List<Sprite> characterSprites;

    public float GetXpNormalized(float currentXp, int level)
    {
        if (level > maxLevel) throw new System.Exception("Your level is very very bad");
        return currentXp / (xpFactor * xpCurve.Evaluate((float)(level+1)/(maxLevel+1)));
    }

    public float GetXpForNextLevel(int level)
    {
        return xpFactor * xpCurve.Evaluate((float)(level + 1) / (maxLevel + 1));
    }
}
public enum HeroTypeEnum
{
    Crook,
    Wizzezer,
    Chevalier
}