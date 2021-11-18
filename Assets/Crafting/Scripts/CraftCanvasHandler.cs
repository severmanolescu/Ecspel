using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCanvasHandler : MonoBehaviour
{
    [SerializeField] List<CraftSetData> crafts = new List<CraftSetData>();

    [SerializeField] GameObject craftPrefab;
    [SerializeField] Transform spawnLocation;

    public void ReinitializeAllCraftings()
    {
        foreach(CraftSetData craft in crafts)
        {
            craft.CheckIfItemsAreAvaible();
        }
    }

    private bool SearchIfCraftExist(Craft craft)
    {
        foreach(CraftSetData craftSet in crafts)
        {
            if(craftSet.Craft == craft)
            {
                return true;
            }
        }

        return false;
    }

    public void AddCraft(Craft craft)
    {
        if(SearchIfCraftExist(craft) == false)
        {
            CraftSetData newCraft = Instantiate(craftPrefab, spawnLocation).GetComponent<CraftSetData>();

            //newCraft.transform.SetParent(spawnLocation);

            newCraft.Craft = craft;

            crafts.Add(newCraft);
        }
    }
}
