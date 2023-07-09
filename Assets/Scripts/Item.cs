using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public string displayName;    
}



public enum ItemEnum
{
    Clef,
    Clou,
    Marto,
    Scie,
    Tournevis,
    NullItem
}

