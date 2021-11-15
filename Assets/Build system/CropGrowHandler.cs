using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropGrowHandler : MonoBehaviour
{
    private List<CropGrow> cropGrows = new List<CropGrow>();

    private List<SaplingGrowHandler> saplingGrows = new List<SaplingGrowHandler>();

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
                sapling.DayChange(day);
            }
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

    public void SaplingAddList(SaplingGrowHandler crop)
    {
        saplingGrows.Add(crop);
    }

    public void RemoveSaplingList(SaplingGrowHandler crop)
    {
        saplingGrows.Remove(crop);
    }
}
