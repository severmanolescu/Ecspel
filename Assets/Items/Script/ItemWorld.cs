using UnityEngine;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    private Item item;

    private SpriteRenderer spriteRenderer;

    private TextMeshProUGUI amount;

    private SunShadowHandler sunTimer;

    private Transform shadow;

    public Item Item { get { return item; } }

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

        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        if (transforms[1] != null && transforms[1].CompareTag("SunShadow"))
        {
            shadow = transforms[1];
        }
    }

    private void Start()
    {
        sunTimer = GameObject.Find("DayTimer").GetComponent<SunShadowHandler>();

        if (shadow != null)
        {
            sunTimer.AddShadow(shadow);
        }
    }

    public void SetItem(Item items)
    {
        this.item = items;

        spriteRenderer.sprite = items.Sprite;

        if (amount != null)
        {
            if (item.Amount > 1)
            {
                amount.SetText(item.Amount.ToString());
            }
            else
            {
                amount.SetText("");
            }
        }
    }

    public void ReinitializeItem()
    {
        if (item.Amount > 1)
        {
            amount.SetText(item.Amount.ToString());
        }
        else
        {
            amount.SetText("");
        }
    }

    public void DestroySelf()
    {
        if (shadow != null)
        {
            sunTimer.AddShadow(shadow);
        }

        Destroy(gameObject);
    }
}
