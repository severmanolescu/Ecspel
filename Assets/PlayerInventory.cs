using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemSlot> itemsSlot;

    private List<Item> items = new List<Item>();

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        AddItem(DefaulData.GetItemWithAmount(DefaulData.stoneAxe, 1));
        AddItem(DefaulData.GetItemWithAmount(DefaulData.pickaxe, 1));
    }

    private void Start()
    {
        itemsSlot = new List<ItemSlot>(gameObject.GetComponentsInChildren<ItemSlot>());

        StartCoroutine(Wait());
    }

    private bool AddItemStackable(Item item)
    {
        foreach (ItemSlot auxItem in itemsSlot)
        {
            if (auxItem.GetItem() != null && auxItem.GetItem().GetName() == item.GetName() && item.GetAmount() > 0)
            {
                if (auxItem.GetItem().GetAmount() < item.GetMaximAmount())
                {
                    int auxiliarIteme = item.GetMaximAmount() - auxItem.GetItem().GetAmount();

                    if (item.GetAmount() <= auxiliarIteme)
                    {
                        auxItem.GetItem().ChangeAmount(auxItem.GetItem().GetAmount() + item.GetAmount());

                        item.ChangeAmount(0);
                    }
                    else
                    {
                        auxItem.GetItem().ChangeAmount(auxItem.GetItem().GetAmount() + auxiliarIteme);

                        item.ChangeAmount(item.GetAmount() - auxiliarIteme);
                    }

                    auxItem.ReinitializeItem();
                }
            }
        }

        if (item.GetAmount() <= 0)
        {
            return true;
        }

        foreach (ItemSlot auxItem in itemsSlot)
        {
            if (auxItem.GetItem() == null && item.GetAmount() > 0)
            {
                if (item.GetAmount() > item.GetMaximAmount())
                {
                    int auxiliar = item.GetAmount() - item.GetMaximAmount();

                    auxItem.SetItem(item.Copy());

                    item.ChangeAmount(auxiliar);
                }

                else
                {
                    auxItem.SetItem(item);

                    return true;
                }

            }
        }

        if (item.GetAmount() <= 0)
        {
            return true;
        }

        return false;
    }

    public bool AddItem(Item item)
    {
        if(item.GetMaximAmount() > 1)
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
                    if (auxItem.GetItem() == null)
                    {
                        auxItem.SetItem(item);

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
