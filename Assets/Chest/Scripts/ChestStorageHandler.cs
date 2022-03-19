using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ChestStorageHandler : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;

    private ChestOpenHandler chestOpenHandler;

    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    public ChestOpenHandler ChestOpenHandler { get => chestOpenHandler; set => chestOpenHandler = value; }

    public void SetChestStorage(List<Item> items, int maxSlots, ChestOpenHandler chestOpenHandler)
    {
        ReinitializeStorage();

        this.ChestOpenHandler = chestOpenHandler;

        int slots = items.Count;

        foreach(Item item in items)
        {
            InstantiateSlot(item);
        }

        for(int emptySlots = slots; emptySlots < maxSlots; emptySlots++)
        {
            InstantiateSlot(null);
        }
    }

    private void InstantiateSlot(Item item)
    {
        Item newItem = null;

        if(item != null)
        {
            newItem = item.Copy();
        }

        ItemSlot itemSlot = Instantiate(slotPrefab).GetComponent<ItemSlot>();

        itemSlot.transform.SetParent(slotParent);

        itemSlot.SetItem(newItem);

        itemSlots.Add(itemSlot);

        itemSlot.transform.localScale = new Vector3(.5f, .5f, 1);
    }

    private void ReinitializeStorage()
    {
        foreach(ItemSlot itemSlot in itemSlots)
        {
            Destroy(itemSlot.gameObject);
        }

        itemSlots.Clear();
    }

    public List<Item> GetChestStorage()
    {
        List<Item> items = new List<Item>();

        foreach(ItemSlot itemSlot in itemSlots)
        {
            items.Add(itemSlot.Item);
        }

        ReinitializeStorage();

        return items;
    }

    private bool AddItemStackable(Item item)
    {
        foreach (ItemSlot auxItem in itemSlots)
        {
            if (auxItem.Item != null && auxItem.Item.Name == item.Name && item.Amount > 0)
            {
                if (auxItem.Item.Amount < item.MaxAmount)
                {
                    int auxiliarIteme = item.MaxAmount - auxItem.Item.Amount;

                    if (item.Amount <= auxiliarIteme)
                    {
                        auxItem.Item.Amount = auxItem.Item.Amount + item.Amount;

                        auxItem.ReinitializeItem();

                        return true;
                    }
                    else
                    {
                        auxItem.Item.Amount = auxItem.Item.Amount + auxiliarIteme;

                        item.Amount = item.Amount - auxiliarIteme;
                    }

                    auxItem.ReinitializeItem();
                }
            }
        }

        if (item.Amount <= 0)
        {
            return true;
        }

        foreach (ItemSlot auxItem in itemSlots)
        {
            if (auxItem.Item == null && item.Amount > 0)
            {
                if (item.Amount > item.MaxAmount)
                {
                    int auxiliar = item.Amount - item.MaxAmount;

                    Item auxChangeItem = Instantiate(item);

                    auxChangeItem.Amount = item.MaxAmount;

                    auxItem.SetItem(auxChangeItem);

                    item.Amount = auxiliar;
                }

                else
                {
                    auxItem.SetItem(item);

                    return true;
                }
            }
        }

        if (item.Amount <= 0)
        {
            return true;
        }

        return false;
    }

    public bool AddItem(Item item)
    {
        if (item != null)
        {
            if (item.MaxAmount > 1)
            {
                return AddItemStackable(item);
            }
            else
            {
                foreach (ItemSlot auxItem in itemSlots)
                {
                    if (auxItem.Item == null)
                    {
                        auxItem.SetItem(item);

                        return true;
                    }
                }

                return false;
            }
        }

        return false;
    }
}
