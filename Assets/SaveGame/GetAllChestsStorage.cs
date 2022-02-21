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

        chestType = GetComponent<GetChestType>();   
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
                if (item != null && item.ItemNO != -1)
                {
                    chestSave.Items.Add(new Tuple<int, int>(item.ItemNO, item.Amount));
                }
                else
                {
                    chestSave.Items.Add(new Tuple<int, int>(-1, -1));
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
            if (chestSave != null)
            {
                GameObject chest = Instantiate(chestType.GetChestObject(chestSave.ChestID), placedObjects.transform);

                chest.transform.position = new Vector3(chestSave.PositionX, chestSave.PositionY);
                
                chest.transform.parent = placedObjects.transform;

                ChestStorage chestStorage = chest.GetComponent<ChestStorage>();

                if (chestStorage != null)
                {
                    foreach (Tuple<int, int> item in chestSave.Items)
                    {
                        Item newItem = getItemFromNO.ItemFromNo(item.Item1);

                        if (newItem != null)
                        {
                            Item itemToAdd = newItem.Copy();

                            itemToAdd.Amount = item.Item2;

                            chestStorage.AddItem(itemToAdd);
                        }
                        else
                        {
                            chestStorage.AddItem(null);
                        }
                    }
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
                if (item != null && item.ItemNO != -1)
                {
                    houseChest.Add(new Tuple<int, int>(item.ItemNO, item.Amount));
                }
                else
                {
                    houseChest.Add(new Tuple<int, int>(-1, -1));
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
            Item newItem = getItemFromNO.ItemFromNo(item.Item1);

            if (newItem != null)
            {
                Item itemToAdd = newItem.Copy();

                itemToAdd.Amount = item.Item2;

                chestStorage.AddItem(itemToAdd);
            }
            else
            {
                chestStorage.AddItem(null);
            }
        }
    }
}
