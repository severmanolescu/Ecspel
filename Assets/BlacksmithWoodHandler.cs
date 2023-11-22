using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithWoodHandler : MonoBehaviour
{
    public SpriteRenderer[] woodList;

    public int indexOfWood = 0;

    private void Awake()
    {
        woodList = GetComponentsInChildren<SpriteRenderer>();
    }

    public bool PickUpWood()
    {
        if(indexOfWood < woodList.Length)
        {
            woodList[indexOfWood].gameObject.SetActive(false);

            indexOfWood++;

            if(indexOfWood < woodList.Length)
            {
                return true;
            }
        }

        return false;
    }

    public void AddWood()
    {
        foreach(SpriteRenderer wood in woodList)
        {
            wood.gameObject.SetActive(true);
        }

        indexOfWood = 0;
    }
}
