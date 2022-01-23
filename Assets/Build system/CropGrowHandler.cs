using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropGrowHandler : MonoBehaviour
{
    private List<CropGrow> cropGrows = new List<CropGrow>();

    public List<SaplingGrowHandler> saplingGrows = new List<SaplingGrowHandler>();

    private List<SaplingGrowHandler> toRemoveFromList = new List<SaplingGrowHandler>();

    public void DayChange(int day)
    {
        if (cropGrows.Count > 0)
        {
            foreach (CropGrow crop in cropGrows)
            {
                crop.DayChange(day);
            }
        }

        if (saplingGrows.Count > 0)
        {
            foreach (SaplingGrowHandler sapling in saplingGrows)
            {
                if (sapling != null)
                {
                    sapling.DayChange(day);
                }
                else
                {
                    toRemoveFromList.Add(sapling);
                }
            }
        }

        if(toRemoveFromList.Count > 0)
        {
            foreach (SaplingGrowHandler sapling in toRemoveFromList)
            {
                saplingGrows.Remove(sapling);
            }

            toRemoveFromList.Clear();
        }
    }

    public void CropAddList(CropGrow crop)
    {
        cropGrows.Add(crop);
    }

    public void RemoveCropList(CropGrow crop)
    {
        cropGrows.Remove(crop);
    }

    public void SaplingAddList(SaplingGrowHandler sapling)
    {
        saplingGrows.Add(sapling);
    }

    public void RemoveSaplingList(SaplingGrowHandler sapling)
    {
        toRemoveFromList.Remove(sapling);
    }
}
