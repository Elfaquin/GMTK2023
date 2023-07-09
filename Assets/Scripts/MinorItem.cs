using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinorItem", menuName = "ScriptableObjects/MinorItem")]
public class MinorItem : Item
{
    public int goldCount;
}

public enum MinorItemEnum
{
    Podlou,
    Saucisse,
    Perrier
}
