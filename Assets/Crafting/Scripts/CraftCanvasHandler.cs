using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftCanvasHandler : MonoBehaviour
{
    private  List<CraftSetData> crafts = new List<CraftSetData>();

    [SerializeField] private List<Craft> initialCrafts = new List<Craft>();

    [SerializeField] GameObject craftPrefab;
    [SerializeField] Transform spawnLocation;

    private void Start()
    {
        foreach(Craft craft in initialCrafts)
        {
            InstantiateCraft(craft);
        }

        ReinitializeAllCraftings();
    }

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
            InstantiateCraft(craft);
        }
    }

    private void InstantiateCraft(Craft craft)
    {
        CraftSetData newCraft = Instantiate(craftPrefab, spawnLocation).GetComponent<CraftSetData>();

        newCraft.Craft = craft;

        crafts.Add(newCraft);
    }
}
