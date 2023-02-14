using TMPro;
using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    private Item item;

    private SpriteRenderer itemSprite;

    private TextMeshProUGUI amount;

    private bool enteredInPlayer = false;

    private float moveSpeed = 2f;
    private float maxDistante = 0.1f;

    Vector3 newPosition;
    bool move = false;

    public Item Item { get { return item; } }

    public bool EnteredInPlayer { get => enteredInPlayer; set => enteredInPlayer = value; }

    public static ItemWorld SpawnItemWorld(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemSprites.Instance.ItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);

        return itemWorld;
    }

    private void Awake()
    {
        itemSprite = GetComponent<SpriteRenderer>();

        amount = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(Item items, bool luck = true)
    {
        this.item = items;

        if (item == null || item.Amount == 0)
        {
            Destroy(gameObject);
        }
        else if (item != null)
        {
            if (itemSprite == null)
            {
                itemSprite = GetComponent<SpriteRenderer>();
            }

            itemSprite.sprite = item.ItemSprite;

            if (amount != null)
            {
                SkillsHandler skillsHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();

                if (luck)
                {
                    int chanceForanotherItem = Random.Range(0, 100);

                    if (chanceForanotherItem <= skillsHandler.LuckLevel * 3.5)
                    {
                        item.Amount++;
                    }
                }

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
    }

    public void MoveToPoint()
    {
        tag = "Untagged";

        this.newPosition = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), 0);

        move = true;
    }

    private void Update()
    {
        if (move == true)
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
        if (item != null)
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
        else
        {
            DestroySelf();
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
