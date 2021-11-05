using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropGrowHandler : MonoBehaviour
{
    private List<CropGrow> cropGrows = new List<CropGrow>();

    public void DayChange(int day)
    {
        if (cropGrows.Count > 0)
        {
            foreach (CropGrow crop in cropGrows)
            {
                crop.DayChange(day);
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
}
