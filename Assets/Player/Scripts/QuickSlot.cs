using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuickSlot : MonoBehaviour
{
    private ItemSlot equiped;

    private TextMeshProUGUI amount;

    private Image[] itemSprites;

    public Item Item { get { return equiped.Item; } }

    private void Awake()
    {
        itemSprites = gameObject.GetComponentsInChildren<Image>();

        equiped = GameObject.Find("Player/Canvas/PlayerItems/Slots/" + gameObject.name).GetComponent<ItemSlot>();

        amount = GetComponentInChildren<TextMeshProUGUI>();

        if (equiped.Item != null)
        {
            itemSprites[1].sprite = equiped.Item.Sprite;
        }
        else
        {
            itemSprites[1].gameObject.SetActive(false);

            amount.gameObject.SetActive(false);
        }
        
        DeselectItem();
    }

    public void Reinitialize()
    {
        if (equiped.Item != null)
        {
            itemSprites[1].sprite = equiped.Item.Sprite;
            itemSprites[1].gameObject.SetActive(true);

            if(equiped.Item.Amount <= 0)
            {
                equiped.DeleteItem();

                itemSprites[1].gameObject.SetActive(false);

                amount.gameObject.SetActive(false);
            }
            else if (equiped.Item.Amount > 1)
            {
                amount.text = equiped.Item.Amount.ToString();

                amount.gameObject.SetActive(true);

                equiped.ReinitializeItem();
            }
            else
            {
                equiped.ReinitializeItem();

                amount.gameObject.SetActive(false);
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