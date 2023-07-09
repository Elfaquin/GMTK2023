using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatsDisplay : MonoBehaviour
{
    public List<Image> catsDisplay;

    private void Start()
    {
        foreach (var c in catsDisplay)
        {
            c.enabled = false;
        }
    }

    public void DisplayOneCat()
    {
        foreach (var cat in catsDisplay)
        {
            if(cat.enabled == false)
                cat.enabled = true;
        }
    }

    public int GetNumberOfDisplayedCats()
    {
        for(int i = 0; i < catsDisplay.Count; i++)
        {
            if (catsDisplay[i].enabled == false) 
                return i;
        }
        return 0;
    }
    
}
