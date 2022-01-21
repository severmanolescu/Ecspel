using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemWorld : MonoBehaviour
{
    private Item item;

    private Image itemSprite;

    private ItemSprites itemSprites;

    private TextMeshProUGUI amount;

    private SunShadowHandler sunTimer;

    private Transform shadow;

    private float moveSpeed = 2f;
    private float maxDistante = 0.1f;

    Vector3 newPosition;
    bool move = false;

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
        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        itemSprite = gameObject.GetComponentInChildren<Image>();

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

        itemSprite.sprite = itemSprites.GetItemSprite(item.ItemNO);

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

    public void MoveToPoint()
    {
        tag = "Untagged";

        this.newPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0);

        move = true;
    }

    private void Update()
    {
        if(move == true)
        {
            if (Vector3.Distance(transform.position, newPosition) >= maxDistante)
            {
                transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                move = false;

                tag = "ItemWorld";
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
