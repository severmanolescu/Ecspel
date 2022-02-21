using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInventory : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;

    [SerializeField] private Transform spawnLocation;

    [SerializeField] private SetDataToBuySlider setDataToBuy;

    //Types:
    //0 - Nothing
    //1 - normal items
    //2 - library items: books, crafting recipes
    private int typeOfBuyItems;

    public int TypeOfBuyItems { get => typeOfBuyItems; }

    public void Close()
    {
        setDataToBuy.Close();

        gameObject.SetActive(false);
    }

    private void DeleteAllOldItems()
    {
        ItemSlot[] itemSlots = GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot slot in itemSlots)
        {
            Destroy(slot.gameObject);
        }
    }

    public void SetItems(List<ItemWithAmount> items, int typeOfBuyItems)
    {
        DeleteAllOldItems();

        this.typeOfBuyItems = typeOfBuyItems;

        foreach (ItemWithAmount item in items)
        {
            Item newItem = item.Item.Copy();

            newItem.Amount = item.Amount;

            SetItem(newItem);
        }
    }

    public void SetItem(Item item)
    {
        ItemSlot itemSlot = Instantiate(itemSlotPrefab, spawnLocation).GetComponent<ItemSlot>();

        itemSlot.SetItem(item);

        itemSlot.ShopItems = true;
    }

    private void DeleteAudioSourceClip()
    {
        GetComponent<AudioSource>().clip = null;
    }

    public List<ItemWithAmount> GetAllItems()
    {
        DeleteAudioSourceClip();

        List<ItemWithAmount> items = new List<ItemWithAmount>();

        ItemSlot[] slots = spawnLocation.GetComponentsInChildren<ItemSlot>();

        foreach(ItemSlot slot in slots)
        {
            if (slot.Item != null)
            {
                items.Add(new ItemWithAmount(slot.Item, slot.Item.Amount));
            }
        }

        return items;
    }
}
