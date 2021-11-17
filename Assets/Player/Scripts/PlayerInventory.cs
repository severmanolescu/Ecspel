using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger quickSlots;

    private List<ItemSlot> itemsSlot;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            AddItem(DefaulData.GetItemWithAmount(DefaulData.log, 10));
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        AddItem(DefaulData.GetItemWithAmount(DefaulData.stoneAxe, 10));
        AddItem(DefaulData.GetItemWithAmount(DefaulData.pickAxe, 10));
        AddItem(DefaulData.GetItemWithAmount(DefaulData.hoe, 10));
        AddItem(DefaulData.GetItemWithAmount(DefaulData.sword, 10));
        
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

                        auxItem.ReinitializeItem();

                        quickSlots.Reinitialize();

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

                    Item auxChangeItem = Instantiate(item);

                    auxChangeItem.Amount = item.MaxAmount;

                    auxItem.SetItem(auxChangeItem);

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
                    return true;
                }
            }

            return false;
        }
    }

    public bool AddItem(List<QuestItems> questItems)
    {
        foreach (QuestItems quest in questItems)
        {
            Item item = quest.Item;

            item.Amount = quest.Amount;

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
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public bool SearchInventory(Item item, int amount)
    {
        foreach(ItemSlot itemSlot in itemsSlot)
        {
            if(itemSlot.Item != null && itemSlot.Item.Name == item.name)
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

    public void DeteleItems(List<QuestItems> items)
    {
        foreach (QuestItems questItem in items)
        {
            int amount = questItem.Amount;

            foreach (ItemSlot itemSlot in itemsSlot)
            {
                if (itemSlot.Item != null && itemSlot.Item.Name == questItem.Item.Name)
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
        }

        quickSlots.Reinitialize();
    }

    public void DeleteItem(Item item)
    {
        int amount = item.Amount;

        foreach (ItemSlot itemSlot in itemsSlot)
        {
            if (itemSlot.Item != null && itemSlot.Item.Name == item.Name)
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
}
