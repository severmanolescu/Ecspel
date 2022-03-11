 using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftSetData : MonoBehaviour , IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private GameObject itemWorldPrefab;

    private ItemSprites itemSprites;

    private Craft craft = null;

    private Transform playerLocation;

    private bool haveItems = false;

    private bool haveStamina = false;

    private CraftCanvasHandler canvasHandler = null;

    [SerializeField] private Image receiveItem;
    [SerializeField] private Image needItem1;
    [SerializeField] private Image needItem2;
    [SerializeField] private Image needItem3;
    [SerializeField] private Image staminaNeed;

    private PlayerInventory playerInventory;

    private ItemDetails itemDetails;
    private PlayerStats playerStats;

    public Craft Craft { get => craft; set => SetData(value); }
    public CraftCanvasHandler CanvasHandler { get => canvasHandler; set => canvasHandler = value; }

    private void Awake()
    {
        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        playerLocation = GameObject.Find("Global/Player").transform;

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        itemDetails = GameObject.Find("Player/Canvas/ItemDetails").GetComponent<ItemDetails>();
    }

    private void SetData(Craft craft)
    {
        if (craft != null)
        {
            if(itemSprites == null)
            {
                Awake();
            }

            this.craft = craft;

            receiveItem.sprite = itemSprites.GetItemSprite(craft.ReceiveItem.Item.ItemNO);
            receiveItem.GetComponent<ChangeText>().Change(craft.ReceiveItem.Amount.ToString());

            GetComponent<Button>().onClick.AddListener(delegate { CraftItem(); });

            staminaNeed.GetComponent<ChangeText>().Change(craft.Stamina.ToString());

            switch (craft.NeedItem.Count)
            {
                case 1:
                    {
                        needItem1.sprite = itemSprites.GetItemSprite(craft.NeedItem[0].Item.ItemNO);
                        needItem1.GetComponent<ChangeText>().Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.gameObject.SetActive(false);
                        needItem3.gameObject.SetActive(false);

                        break;
                    }
                case 2:
                    {
                        needItem1.sprite = itemSprites.GetItemSprite(craft.NeedItem[0].Item.ItemNO);
                        needItem1.GetComponent<ChangeText>().Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.sprite = itemSprites.GetItemSprite(craft.NeedItem[1].Item.ItemNO);
                        needItem2.GetComponent<ChangeText>().Change(craft.NeedItem[1].Amount.ToString());
                                                                        
                        needItem3.gameObject.SetActive(false);

                        break;
                    }
                case 3:
                    {
                        needItem1.sprite = itemSprites.GetItemSprite(craft.NeedItem[0].Item.ItemNO);
                        needItem1.GetComponent<ChangeText>().Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.sprite = itemSprites.GetItemSprite(craft.NeedItem[1].Item.ItemNO);
                        needItem2.GetComponent<ChangeText>().Change(craft.NeedItem[1].Amount.ToString());

                        needItem3.sprite = itemSprites.GetItemSprite(craft.NeedItem[2].Item.ItemNO);
                        needItem3.GetComponent<ChangeText>().Change(craft.NeedItem[2].Amount.ToString());

                        break;
                    }
            }
        }
    }

    public void CraftItem()
    {
        if(haveItems == true && haveStamina)
        {
            Item auxItem;

            if(canvasHandler != null)
            {
                canvasHandler.PlaySoundEffect();
            }

            foreach (ItemWithAmount item in craft.NeedItem)
            {
                auxItem = item.Item.Copy();
                auxItem.Amount = item.Amount;

                playerInventory.DeleteItem(auxItem);
            }

            auxItem = craft.ReceiveItem.Item.Copy();
            auxItem.Amount = craft.ReceiveItem.Amount;

            bool canAddItemToPlayerInventory = playerInventory.AddItem(auxItem);

            if(canAddItemToPlayerInventory == false)
            {
                ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

                itemWorld.SetItem(auxItem, false);

                itemWorld.transform.position = playerLocation.position;

                itemWorld.MoveToPoint();
            }

            playerStats.DecreseStamina(craft.Stamina);

            GetComponentInParent<CraftCanvasHandler>().ReinitializeAllCraftings();
        }
    }

    private void ChangeColorSprites(int indexOfItem, Color color)
    {
        switch (indexOfItem)
        {
            case 0: needItem1.color = color; break;
            case 1: needItem2.color = color; break;
            case 2: needItem3.color = color; break;
            case 3: staminaNeed.color = color; break;
        }
    }

    private void SetAmountToItem(int indexOfItem, int itemAmount, int amountInInventory)
    {
        switch (indexOfItem)
        {
            case 0: needItem1.GetComponent<ChangeText>().Change(amountInInventory + @"\" + itemAmount); break;
            case 1: needItem2.GetComponent<ChangeText>().Change(amountInInventory + @"\" + itemAmount); break;
            case 2: needItem3.GetComponent<ChangeText>().Change(amountInInventory + @"\" + itemAmount); break;
        }
    }

    public void CheckIfItemsAreAvaible()
    {
        haveItems = true;

        haveStamina = true;

        int indexOfItem = 0;

        foreach (ItemWithAmount item in craft.NeedItem)
        {
            int amountInInventory = playerInventory.GetAmountOfItem(item.Item);

            if (amountInInventory < item.Amount)
            {
                ChangeColorSprites(indexOfItem, Color.red);

                haveItems = false;
            }
            else
            {
                ChangeColorSprites(indexOfItem, Color.white);
            }

            SetAmountToItem(indexOfItem, item.Amount, amountInInventory);

            indexOfItem++;
        }

        if(playerStats.Stamina >= craft.Stamina)
        {
            ChangeColorSprites(3, Color.white);
        }
        else
        {
            haveStamina = false;

            ChangeColorSprites(3, Color.red);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemDetails != null)
        {
            itemDetails.HideData();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemDetails != null)
        {
            itemDetails.SetItem(craft.ReceiveItem.Item);
        }
    }
}
