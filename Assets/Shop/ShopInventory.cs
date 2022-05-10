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

    private bool discount = false;

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

    public void SetItems(List<Item> items, int typeOfBuyItems, bool discount)
    {
        DeleteAllOldItems();

        this.typeOfBuyItems = typeOfBuyItems;

        this.discount = discount;

        foreach (Item item in items)
        {
            Item newItem = item.Copy();

            SetItem(newItem);
        }
    }

    public void SetItem(Item item)
    {
        ItemSlot itemSlot = Instantiate(itemSlotPrefab, spawnLocation).GetComponent<ItemSlot>();

        itemSlot.SetItem(item, discount);

        itemSlot.ShopItems = true;
    }

    private void DeleteAudioSourceClip()
    {
        GetComponent<AudioSource>().clip = null;
    }

    public List<Item> GetAllItems()
    {
        DeleteAudioSourceClip();

        List<Item> items = new List<Item>();

        ItemSlot[] slots = spawnLocation.GetComponentsInChildren<ItemSlot>();

        foreach(ItemSlot slot in slots)
        {
            if (slot.Item != null)
            {
                items.Add(slot.Item.Copy());
            }
        }

        return items;
    }
}
