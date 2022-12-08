using System.Collections.Generic;
using UnityEngine;

public class CropGrowHandler : MonoBehaviour
{
    private List<CropGrow> cropGrows = new List<CropGrow>();

    private List<SaplingGrowHandler> saplingGrows = new List<SaplingGrowHandler>();

    private List<SaplingGrowHandler> toRemoveFromListSaplings = new List<SaplingGrowHandler>();
    private List<CropGrow> toRemoveFromListCrops = new List<CropGrow>();

    public void DayChange(int day)
    {
        if (cropGrows.Count > 0)
        {
            foreach (CropGrow crop in cropGrows)
            {
                if (crop != null)
                {
                    crop.DayChange(day);
                }
                else
                {
                    toRemoveFromListCrops.Add(crop);
                }
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
                    toRemoveFromListSaplings.Add(sapling);
                }
            }
        }

        if (toRemoveFromListSaplings.Count > 0)
        {
            foreach (SaplingGrowHandler sapling in toRemoveFromListSaplings)
            {
                saplingGrows.Remove(sapling);
            }

            toRemoveFromListSaplings.Clear();
        }

        if (toRemoveFromListCrops.Count > 0)
        {
            foreach (CropGrow cropGrow in toRemoveFromListCrops)
            {
                cropGrows.Remove(cropGrow);
            }

            toRemoveFromListCrops.Clear();
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
        toRemoveFromListSaplings.Remove(sapling);
    }

    public void ReinitializeLists()
    {
        cropGrows = new List<CropGrow>();

        saplingGrows = new List<SaplingGrowHandler>();

        toRemoveFromListSaplings = new List<SaplingGrowHandler>();
    }
}
