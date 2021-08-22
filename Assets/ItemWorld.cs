using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    private Item item;

    private SpriteRenderer spriteRenderer;

    private TextMeshProUGUI amount;

    private int randomNumber;

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemSprites.Instance.ItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        amount = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(Item items)
    {
        this.item = items;

        spriteRenderer.sprite = items.GetSprite();

        if (amount != null)
        {
            if (item.GetAmount() > 1)
            {
                amount.SetText(item.GetAmount().ToString());
            }
            else
            {
                amount.SetText("");
            }
        }
    }

    public void ReinitializeItem()
    {
        if (item.GetAmount() > 1)
        {
            amount.SetText(item.GetAmount().ToString());
        }
        else
        {
            amount.SetText("");
        }
    }

    public Item GetItem()
    {
        return item;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public int GetRandom()
    {
        return randomNumber;
    }
}
