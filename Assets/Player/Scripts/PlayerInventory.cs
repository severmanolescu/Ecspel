using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger quickSlots;

    [SerializeField] private Animator animator;

    private List<ItemSlot> itemsSlot;

    private GetItemFromNO getItem;

    private CoinsHandler coinsHandler;

    public CoinsHandler CoinsHandler { get => coinsHandler; set => coinsHandler = value; }

    private void Awake()
    {
        getItem = GameObject.Find("Global").GetComponent<GetItemFromNO>();

        itemsSlot = new List<ItemSlot>(gameObject.GetComponentsInChildren<ItemSlot>());

        CoinsHandler = GetComponentInChildren<CoinsHandler>();
    }

    private int AddItemStackable(Item item)
    {
        foreach (ItemSlot auxItem in itemsSlot)
        {
            if (auxItem.Item != null && auxItem.Item.ItemNO == item.ItemNO && item.Amount > 0)
            {
                if (auxItem.Item.Amount < item.MaxAmount)
                {
                    int auxiliarItem = item.MaxAmount - auxItem.Item.Amount;

                    if (item.Amount <= auxiliarItem)
                    {
                        auxItem.Item.Amount = auxItem.Item.Amount + item.Amount;

                        auxItem.ReinitializeItem();

                        quickSlots.Reinitialize();

                        return 0;
                    }
                    else
                    {
                        auxItem.Item.Amount = auxItem.Item.Amount + auxiliarItem;

                        item.Amount = item.Amount - auxiliarItem;
                    }

                    auxItem.ReinitializeItem();
                }
            }
        }

        if (item.Amount <= 0)
        {
            quickSlots.Reinitialize();

            return 0;
        }

        foreach (ItemSlot auxItem in itemsSlot)
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

                    quickSlots.Reinitialize();

                    return 0;
                }
            }
        }

        quickSlots.Reinitialize();

        return item.Amount;
    }

    public int AddItem(Item item)
    {
        if (item != null)
        {
            if (item.ItemNO == 0)
            {
                CoinsHandler.Amount += item.Amount;

                return 0;
            }
            else
            {
                if (item.MaxAmount > 1)
                {
                    return AddItemStackable(item);
                }
                else
                {
                    foreach (ItemSlot auxItem in itemsSlot)
                    {
                        if (auxItem.Item == null)
                        {
                            auxItem.SetItem(item);

                            quickSlots.Reinitialize();

                            return 0;
                        }
                    }

                    return item.Amount;
                }
            }

            return item.Amount;
        }

        return 0;        
    }

    public int AddItemWithAnimation(Item item)
    {
        int amount = AddItem(item);

        if (amount == 0)
        {
            //animator.SetTrigger("Pickup");
        }

        return amount;
    }

    public bool SearchInventory(Item item, int amount)
    {
        foreach (ItemSlot itemSlot in itemsSlot)
        {
            if (itemSlot.Item != null && itemSlot.Item.ItemNO == item.ItemNO)
            {
                amount -= itemSlot.Item.Amount;
            }

            if (amount <= 0)
            {
                return true;
            }
        }
        return false;
    }

    public int GetAmountOfItem(Item item)
    {
        int amount = 0;

        foreach (ItemSlot itemSlot in itemsSlot)
        {
            if (itemSlot.Item != null && itemSlot.Item.ItemNO == item.ItemNO)
            {
                amount += itemSlot.Item.Amount;
            }
        }

        return amount;
    }

    public void DeleteItem(Item item)
    {
        int amount = item.Amount;

        foreach (ItemSlot itemSlot in itemsSlot)
        {
            if (itemSlot.Item != null && itemSlot.Item.ItemNO == item.ItemNO)
            {
                if (itemSlot.Item.Amount >= amount)
                {
                    itemSlot.DecreseAmount(amount);
                    break;
                }
                else
                {
                    amount -= itemSlot.Item.Amount;

                    itemSlot.DeleteItem();
                    continue;
                }
            }
        }

        quickSlots.Reinitialize();
    }

    public List<Tuple<int, int>> GetAllItemsNo()
    {
        List<Tuple<int, int>> items = new List<Tuple<int, int>>();

        foreach (ItemSlot itemSlot in itemsSlot)
        {
            if (itemSlot.Item != null)
            {
                items.Add(new Tuple<int, int>(itemSlot.Item.ItemNO, itemSlot.Item.Amount));
            }
            else
            {
                items.Add(new Tuple<int, int>(-1, -1));

            }
        }

        return items;
    }

    public void SetInventoryFromSave(List<Tuple<int, int>> itemsNo)
    {
        foreach (ItemSlot slot in itemsSlot)
        {
            slot.Item = null;
        }

        int indexOfItemSlot = 0;

        foreach (Tuple<int, int> item in itemsNo)
        {
            Item newItem = getItem.ItemFromNo(item.Item1);

            if (newItem != null)
            {
                newItem = newItem.Copy();

                newItem.Amount = item.Item2;

                itemsSlot[indexOfItemSlot].SetItem(newItem);
            }
            else
            {
                itemsSlot[indexOfItemSlot].SetItem(null);
            }

            indexOfItemSlot++;
        }
    }
}
