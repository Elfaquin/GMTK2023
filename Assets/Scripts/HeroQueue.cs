using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroQueue : MonoBehaviour
{
    private List<Hero> heroQueue;
    public int queueSize;

    // Start is called before the first frame update
    void Start()
    {
        heroQueue = new List<Hero>();
        Hero.numberOfHerosGenerated = 0;
        for (int i = 0; i < queueSize; i++)
        {
            heroQueue.Add(new Hero());
        }
    }

    void EnqueueNewHero()
    {
        Hero newHero = Hero.RandomHero();
        Debug.Log($"Created new hero with ID {newHero.heroId}");
        heroQueue.Add(newHero);
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
    }

    public void HeroArrival()
    {
        Hero firstHero = heroQueue[0];
        if (firstHero.hasQuestAssigned)
        {
            firstHero.ResolveQuest();
            if (firstHero.isDead)
            {
                EnqueueNewHero();
                HeroArrival();
            }
        }
    }

    public void NextQueueTurn()
    {
        PutFirstInLastPosition();
        HeroArrival();
    }
}
