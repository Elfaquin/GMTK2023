using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestType", menuName = "ScriptableObjects/QuestType")]
public class QuestType : ScriptableObject
{
    public QuestTypeEnum type;
    public Sprite sprite;
    public string displayName;
}

public enum QuestTypeEnum
{
    Fetch,
    Kill,
    Dungeon,
    Speak
}