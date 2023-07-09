using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroQueue : MonoBehaviour
{
    private List<Hero> heroQueue;
    public int queueSize;
    public HeroQueueDisplay heroQueueDisplay;
    public HeroDisplay heroDisplay;

    // Start is called before the first frame update
    void Start()
    {
        heroQueue = new List<Hero>();
        Hero.numberOfHerosGenerated = 0;
        for (int i = 0; i < queueSize; i++)
        {
            heroQueue.Add(Hero.RandomHero());
        }
        ActualizeQueueDisplay();
        HeroArrival();
        heroDisplay.displayHero(heroQueue[0]);
    }

    public void AssignQuestToFirstHero(Quest quest)
    {
        heroQueue[0].AssignQuest(quest);
    }

    void EnqueueNewHero()
    {
        Hero newHero = Hero.RandomHero();
        Debug.Log($"Created new hero with ID {newHero.heroId}");
        heroQueue.Add(newHero);
        ActualizeQueueDisplay();
    }

    public void ResetFirstPosition()
    {
        heroQueue.RemoveAt(0);
        EnqueueNewHero();
    }
    public void PutFirstInLastPosition()
    {
        Hero swappedHero = heroQueue[0];
        heroQueue.RemoveAt(0);
        heroQueue.Add(swappedHero);
        ActualizeQueueDisplay();
    }

    public void HeroArrival()
    {
        ActualizeQueueDisplay();
        Hero firstHero = heroQueue[0];
        if (firstHero.hasQuestAssigned)
        {
            firstHero.ResolveQuest();
            if (firstHero.isDead)
            {
                Debug.LogWarning("The hero is dead.");
                ResetFirstPosition();
                HeroArrival();
            }
        }
    }

    public void NextQueueTurn()
    {
        PutFirstInLastPosition();
        HeroArrival();
        heroDisplay.displayHero(heroQueue[0]);
    }

    public void ActualizeQueueDisplay()
    {
        heroQueueDisplay.SetHeroIcons(heroQueue);
    }
}
