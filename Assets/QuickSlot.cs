using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private ItemSlot equiped;

    public Image slotImage;
    public Image itemImage;

    private void Awake()
    {
        Image[] itemSprites = gameObject.GetComponentsInChildren<Image>();

        slotImage = itemSprites[0];
        itemImage = itemSprites[1];
    }

    private void Start()
    {
        if (equiped.GetItem() != null)
        {
            itemImage.sprite = equiped.GetItem().GetSprite();
        }
        else
        {
            itemImage.gameObject.SetActive(false);
        }

        DeselectItem();
    }

    public Item GetItem()
    {
        return equiped.GetItem();
    }

    public void Reinitialize()
    {
        if (equiped.GetItem() != null)
        {
            itemImage.sprite = equiped.GetItem().GetSprite();
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            itemImage.gameObject.SetActive(false);
        }
    }

    public void SelectedItem()
    {
        if (itemImage != null)
        {
            Color color = slotImage.color;

            color.a = 1f;

            slotImage.color = color;

            color = itemImage.color;

            color.a = 1f;

            itemImage.color = color;
        }
    }

    public void DeselectItem()
    {
        if (itemImage != null)
        {
            Color color = slotImage.color;

            color.a = 0.5f;

            slotImage.color = color;

            color = itemImage.color;

            color.a = 0.5f;

            itemImage.color = color;
        }
    }
}
