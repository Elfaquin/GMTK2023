using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QuestsManager : MonoBehaviour
{
    [SerializeField] int MaxQuestNumber;

    List<Quest> quests = new();
    [SerializeField] List<GameObject> questDisplayers;


    // Start is called before the first frame update
    void Start()
    {
        GameLibrary.DoAfter(() =>
        {
            InitMesBurnes();
        });
    }

    private void InitMesBurnes()
    {
        int initial_unlocked = 3;

        for (int i = 0; i < questDisplayers.Count; i++)
        {
            if (i < initial_unlocked) questDisplayers[i].GetComponent<QuestDisplayer>().Unlock();
            else questDisplayers[i].GetComponent<QuestDisplayer>().Lock();
        }

        for (int i = 0; i < initial_unlocked; i++)
        {
            AddQuest(Quest.CreateRandomQuest());
        }

    }


    public void AddQuest(Quest quest)
    {
        quests.Add(quest);
        ActualizeQuestsDisplay();
    }

    public void TakeQuest(int index)
    {
        quests.RemoveAt(index);
        ActualizeQuestsDisplay();
    }

    void ActualizeQuestsDisplay()
    {
        for (int i = 0; i < quests.Count; i++)
        {
            questDisplayers[i].GetComponent<QuestDisplayer>().SetCurrentQuest(quests[i]);
        }
    }

    public void OnAnyQuestDisplayerSelected(int questDisplayerIndex)
    {
        int i;
        for(i = 0; i < questDisplayers.Count ; i++)
        {
            if(i != questDisplayerIndex)
            {
                
                questDisplayers[i].GetComponent<QuestDisplayer>().OnOtherQuestDisplayerSelected();
            }
        }
    }
}
