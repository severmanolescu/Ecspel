using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger quickSlots;

    private List<ItemSlot> itemsSlot;

    private List<Item> items = new List<Item>();

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        AddItem(DefaulData.GetItemWithAmount(DefaulData.stoneAxe, 1));
        AddItem(DefaulData.GetItemWithAmount(DefaulData.pickaxe, 1));
    }

    private void Awake()
    {
        itemsSlot = new List<ItemSlot>(gameObject.GetComponentsInChildren<ItemSlot>());
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private bool AddItemStackable(Item item)
    {
        foreach (ItemSlot auxItem in itemsSlot)
        {
            if (auxItem.Item != null && auxItem.Item.Name == item.Name && item.Amount > 0)
            {
                if (auxItem.Item.Amount < item.MaxAmount)
                {
                    int auxiliarIteme = item.MaxAmount - auxItem.Item.Amount;

                    if (item.Amount <= auxiliarIteme)
                    {
                        auxItem.Item.Amount = auxItem.Item.Amount + item.Amount;

                        item.Amount = 0;
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
            quickSlots.Reinitialize();

            return true;
        }

        foreach (ItemSlot auxItem in itemsSlot)
        {
            if (auxItem.Item == null && item.Amount > 0)
            {
                if (item.Amount > item.MaxAmount)
                {
                    int auxiliar = item.Amount - item.MaxAmount;

                    auxItem.SetItem(item.Copy());

                    item.Amount = auxiliar;
                }

                else
                {
                    auxItem.SetItem(item);

                    quickSlots.Reinitialize();

                    return true;
                }

            }
        }

        if (item.Amount <= 0)
        {
            quickSlots.Reinitialize();

            return true;
        }

        return false;
    }

    public bool AddItem(Item item)
    {
        if(item.MaxAmount > 1)
        {
            if (items.Count < DefaulData.maximInventorySlots)
            {
                return AddItemStackable(item);
            }
            else
            {
                return false;
            }
        }
        else
        {
            if(items.Count < DefaulData.maximInventorySlots)
            {
                foreach (ItemSlot auxItem in itemsSlot)
                {
                    if (auxItem.Item == null)
                    {
                        auxItem.SetItem(item);

                        quickSlots.Reinitialize();
                        return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
        }
    }

}
