using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllCrafts : MonoBehaviour
{
    [SerializeField] private List<Craft> crafts = new List<Craft>();

    [SerializeField] private CraftCanvasHandler craftCanvas;

    public Craft GetCraft(int craftID)
    {
        if (craftID >= 0 && craftID < crafts.Count)
        {
            return crafts[craftID];
        }

        return null;
    }

    public int GetCraftId(Craft craft)
    {
        if(craft != null)
        {
            return crafts.IndexOf(craft);
        }

        return -1;
    }

    public List<int> GetCrafts()
    {
        List<Craft> list = craftCanvas.GetAllCrafts();

        List<int> result = new List<int>();

        foreach (Craft craft in list)
        {
            result.Add(GetCraftId(craft));
        }

        return result;
    }

    public void SetCrafts(List<int> crafts)
    {
        craftCanvas.DeleteAllCrafts();

        foreach(int craft in crafts)
        {
            Craft craftItem = GetCraft(craft);

            if(craftItem != null)
            {
                craftCanvas.AddCraft(craftItem, false);
            }
        }
    }
}
