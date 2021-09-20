using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    [SerializeField] private ItemSlot equiped;

    private Image[] itemSprites;

    private void Awake()
    {
        itemSprites = gameObject.GetComponentsInChildren<Image>();
        
        if (equiped.GetItem() != null)
        {
            itemSprites[1].sprite = equiped.GetItem().GetSprite();
        }
        else
        {
            itemSprites[1].gameObject.SetActive(false);
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
            itemSprites[1].sprite = equiped.GetItem().GetSprite();
            itemSprites[1].gameObject.SetActive(true);
        }
        else
        {
            itemSprites[1].gameObject.SetActive(false);
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
