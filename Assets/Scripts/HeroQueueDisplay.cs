using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroQueueDisplay : MonoBehaviour
{
    public HeroQueue heroQueue;
    public List<Image> heroIcons;

    void SetHeroIcons(List<Hero> heroQueue)
    {
        if(heroQueue.Count != heroIcons.Count)
        {
            throw new System.Exception("Où tu vas avec ta coiffe ?!");
        }
        for(int i = 0; i < heroQueue.Count; i++)
        {
            heroIcons[i].sprite = heroQueue[i].heroType.icon;
        }
    }
}
