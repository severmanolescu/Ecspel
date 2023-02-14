using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    private ItemSlot equiped;

    private TextMeshProUGUI amount;

    private Image[] itemSprites;

    public Item Item { get { return Equiped.Item; } }

    public ItemSlot Equiped { get => equiped; set => equiped = value; }

    private void Awake()
    {
        itemSprites = gameObject.GetComponentsInChildren<Image>();

        Equiped = GameObject.Find("Player/Canvas/PlayerItems/SlotsPockets/" + gameObject.name).GetComponent<ItemSlot>();

        amount = GetComponentInChildren<TextMeshProUGUI>();

        if (Equiped.Item != null)
        {
            itemSprites[1].sprite = Equiped.Item.ItemSprite;
        }
        else
        {
            itemSprites[1].gameObject.SetActive(false);

            amount.gameObject.SetActive(false);
        }

        DeselectItem();
    }

    public void SetItem(Item item)
    {
        equiped.SetItem(item);
    }

    public void Reinitialize()
    {
        if (Equiped.Item != null)
        {
            itemSprites[1].sprite = Equiped.Item.ItemSprite;
            itemSprites[1].gameObject.SetActive(true);

            if (Equiped.Item.Amount <= 0)
            {
                Equiped.DeleteItem();

                itemSprites[1].gameObject.SetActive(false);

                amount.gameObject.SetActive(false);
            }
            else if (Equiped.Item.Amount > 1)
            {
                amount.text = Equiped.Item.Amount.ToString();

                amount.gameObject.SetActive(true);

                Equiped.ReinitializeItem();
            }
            else
            {
                Equiped.ReinitializeItem();

                amount.gameObject.SetActive(false);
            }

            if (equiped.Item is WateringCan)
            {
                WateringCan wateringCan = (WateringCan)equiped.Item;

                amount.text = wateringCan.RemainWater.ToString();

                amount.gameObject.SetActive(true);
            }
        }
        else
        {
            itemSprites[1].gameObject.SetActive(false);
            amount.gameObject.SetActive(false);
        }
    }

    public void SelectedItem()
    {
        if (itemSprites != null)
        {
            Color color = itemSprites[0].color;

            color.a = 1f;

            itemSprites[0].color = color;

            color = itemSprites[1].color;

            color.a = 1f;

            itemSprites[1].color = color;
        }
    }

    public void DeselectItem()
    {
        if (itemSprites != null)
        {
            Color color = itemSprites[0].color;

            color.a = 0.5f;

            itemSprites[0].color = color;

            color = itemSprites[1].color;

            color.a = 0.5f;

            itemSprites[1].color = color;
        }
    }
}