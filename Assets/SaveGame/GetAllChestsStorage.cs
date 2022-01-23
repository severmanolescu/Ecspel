using System;
using System.Collections.Generic;
using UnityEngine;

public class GetAllChestsStorage : MonoBehaviour
{
    [SerializeField] private GameObject placedObjects;

    [SerializeField] private ChestOpenHandler playerHouseChest;

    private GetChestType chestType;

    private GetItemFromNO getItemFromNO;

    private void Awake()
    {
        getItemFromNO = GameObject.Find("Global").GetComponent<GetItemFromNO>();

        chestType = gameObject.GetComponent<GetChestType>();
    }

    public List<ChestSave> GetAllChestStorage()
    {
        List<ChestSave> chestSaves = new List<ChestSave>();

        ChestOpenHandler[] chestOpens = placedObjects.GetComponentsInChildren<ChestOpenHandler>();

        foreach(ChestOpenHandler chestOpen in chestOpens)
        {
            ChestSave chestSave = new ChestSave();

            chestSave.PositionX = chestOpen.transform.position.x;
            chestSave.PositionY = chestOpen.transform.position.y;

            chestSave.ChestID = chestOpen.ChestId;

            ChestStorage chestStorage = chestOpen.GetComponent<ChestStorage>();

            chestSave.Items = new List<Tuple<int, int>>();

            foreach(Item item in chestStorage.Items)
            {
                int itemId = getItemFromNO.GetItemNO(item);

                if (itemId != -1)
                {
                    chestSave.Items.Add(new Tuple<int, int>(itemId, item.Amount));
                }
            }

            chestSaves.Add(chestSave);
        }

        return chestSaves;
    }

    public void SetAllChestToWorld(List<ChestSave> chestSaves)
    {
        ChestOpenHandler[] chestOpens = placedObjects.GetComponentsInChildren<ChestOpenHandler>();

        foreach (ChestOpenHandler chestOpen in chestOpens)
        {
            Destroy(chestOpen.gameObject);
        }

        foreach (ChestSave chestSave in chestSaves)
        {
            GameObject chest = Instantiate(chestType.GetChestObject(chestSave.ChestID), placedObjects.transform);

            chest.transform.position = new Vector3(chestSave.PositionX, chestSave.PositionY);

            ChestStorage chestStorage = chest.GetComponent<ChestStorage>();

            if (chestStorage != null)
            {
                foreach(Tuple<int, int> item in chestSave.Items)
                {
                    Item itemToAdd = getItemFromNO.ItemFromNo(item.Item1).Copy();

                    itemToAdd.Amount = item.Item2;

                    chestStorage.AddItem(itemToAdd);
                }
            }
        }
    }

    public List<Tuple<int, int>> GetPlayerHouseChestStorage()
    {
        List<Tuple<int, int>> houseChest = new List<Tuple<int, int>>();

        if(playerHouseChest != null)
        {
            ChestStorage chestStorage = playerHouseChest.GetComponent<ChestStorage>();

            foreach (Item item in chestStorage.Items)
            {
                int itemId = getItemFromNO.GetItemNO(item);

                if (itemId != -1)
                {
                    houseChest.Add(new Tuple<int, int>(itemId, item.Amount));
                }
            }
        }

        return houseChest;
    }

    public void SetItemsToPlayerHouseChest(List<Tuple<int, int>> chestSave)
    {
        ChestStorage chestStorage = playerHouseChest.GetComponent<ChestStorage>();

        chestStorage.RemoveAllItems();

        foreach (Tuple<int, int> item in chestSave)
        {
            Item itemToAdd = getItemFromNO.ItemFromNo(item.Item1).Copy();

            itemToAdd.Amount = item.Item2;

            chestStorage.AddItem(itemToAdd);
        }
    }
}
