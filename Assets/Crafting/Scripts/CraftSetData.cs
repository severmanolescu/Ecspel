using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftSetData : MonoBehaviour
{
    [SerializeField] private GameObject itemWorldPrefab;

    private ItemSprites itemSprites;

    private Craft craft = null;

    private Transform playerLocation;

    private bool haveItems = false;

    private bool haveStamina = false;

    private CraftCanvasHandler canvasHandler = null;

    [SerializeField] private Image receiveItem;
    [SerializeField] private ChangeText receiveItemText;

    [SerializeField] private Image needItem1;
    [SerializeField] private ChangeText needItem1Text;

    [SerializeField] private Image needItem2;
    [SerializeField] private ChangeText needItem2Text;

    [SerializeField] private Image needItem3;
    [SerializeField] private ChangeText needItem3Text;

    [SerializeField] private Image needItem4;
    [SerializeField] private ChangeText needItem4Text;

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
            if (itemSprites == null)
            {
                Awake();
            }

            this.craft = craft;

            receiveItem.sprite = craft.ReceiveItem.Item.ItemSprite;
            receiveItemText.Change(craft.ReceiveItem.Amount.ToString());

            GetComponent<Button>().onClick.AddListener(delegate { CraftItem(); });

            switch (craft.NeedItem.Count)
            {
                case 1:
                    {
                        needItem1.sprite = craft.NeedItem[0].Item.ItemSprite;
                        needItem1Text.Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.gameObject.SetActive(false);
                        needItem3.gameObject.SetActive(false);
                        needItem4.gameObject.SetActive(false);

                        needItem2Text.HideAmount();
                        needItem3Text.HideAmount();
                        needItem4Text.HideAmount();

                        break;
                    }
                case 2:
                    {
                        needItem1.sprite = craft.NeedItem[0].Item.ItemSprite;
                        needItem1Text.Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.sprite = craft.NeedItem[1].Item.ItemSprite;
                        needItem2Text.Change(craft.NeedItem[1].Amount.ToString());

                        needItem3.gameObject.SetActive(false);
                        needItem4.gameObject.SetActive(false);

                        needItem3Text.HideAmount();
                        needItem4Text.HideAmount();

                        break;
                    }
                case 3:
                    {
                        needItem1.sprite = craft.NeedItem[0].Item.ItemSprite;
                        needItem1Text.Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.sprite = craft.NeedItem[1].Item.ItemSprite;
                        needItem2Text.Change(craft.NeedItem[1].Amount.ToString());

                        needItem3.sprite = craft.NeedItem[2].Item.ItemSprite;
                        needItem3Text.Change(craft.NeedItem[2].Amount.ToString());

                        needItem4.gameObject.SetActive(false);

                        needItem4Text.HideAmount();

                        break;
                    }
                case 4:
                    {
                        needItem1.sprite = craft.NeedItem[0].Item.ItemSprite;
                        needItem1Text.Change(craft.NeedItem[0].Amount.ToString());

                        needItem2.sprite = craft.NeedItem[1].Item.ItemSprite;
                        needItem2Text.Change(craft.NeedItem[1].Amount.ToString());

                        needItem3.sprite = craft.NeedItem[2].Item.ItemSprite;
                        needItem3Text.Change(craft.NeedItem[2].Amount.ToString());

                        needItem4.sprite = craft.NeedItem[3].Item.ItemSprite;
                        needItem4Text.Change(craft.NeedItem[3].Amount.ToString());

                        break;
                    }
            }
        }
    }

    public void CraftItem()
    {
        if (haveItems == true && haveStamina)
        {
            Item auxItem;

            if (canvasHandler != null)
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

            if (canAddItemToPlayerInventory == false)
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
            case 0: needItem1Text.ChangeColor(color); break;
            case 1: needItem2Text.ChangeColor(color); break;
            case 2: needItem3Text.ChangeColor(color); break;
            case 3: needItem4Text.ChangeColor(color); break;
        }
    }

    private void SetAmountToItem(int indexOfItem, int itemAmount, int amountInInventory)
    {
        switch (indexOfItem)
        {
            case 0: needItem1Text.GetComponent<ChangeText>().Change(itemAmount + @"/" + amountInInventory); break;
            case 1: needItem2Text.GetComponent<ChangeText>().Change(itemAmount + @"/" + amountInInventory); break;
            case 2: needItem3Text.GetComponent<ChangeText>().Change(itemAmount + @"/" + amountInInventory); break;
            case 3: needItem4Text.GetComponent<ChangeText>().Change(itemAmount + @"/" + amountInInventory); break;
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

        if (playerStats.Stamina >= craft.Stamina)
        {
            ChangeColorSprites(3, Color.white);
        }
        else
        {
            haveStamina = false;

            ChangeColorSprites(3, Color.red);
        }
    }

    private void SetDataToDetails(int itemNo)
    {
        switch(itemNo)
        {
            default:
            case 0:
                {
                    if (Craft.ReceiveItem != null && Craft.ReceiveItem.Item != null)
                    {
                        itemDetails.SetItem(Craft.ReceiveItem.Item);
                    }
                    break;
                }
            case 1:
                {
                    if (Craft.NeedItem != null && Craft.NeedItem.Count > 0)
                    {
                        itemDetails.SetItem(Craft.NeedItem[0].Item);
                    }
                    break;
                }
            case 2:
                {
                    if (Craft.NeedItem != null && Craft.NeedItem.Count > 1)
                    {
                        itemDetails.SetItem(Craft.NeedItem[1].Item);
                    }
                    break;
                }
            case 3:
                {
                    if (Craft.NeedItem != null && Craft.NeedItem.Count > 2)
                    {
                        itemDetails.SetItem(Craft.NeedItem[2].Item);
                    }
                    break;
                }
            case 4:
                {
                    if (Craft.NeedItem != null && Craft.NeedItem.Count > 3)
                    {
                        itemDetails.SetItem(Craft.NeedItem[3].Item);
                    }
                    break;
                }
        }
    }

    public void MoveItemDetailsToSlot(int itemNo)
    {
        if (itemDetails != null)
        {
            Vector3 newPosition = transform.position;

            newPosition.y -= 55;

            itemDetails.transform.position = newPosition;

            SetDataToDetails(itemNo);

            itemDetails.gameObject.SetActive(true);
        }
    }

    public void HideItemDetails()
    {
        if(itemDetails != null)
        {
            itemDetails.gameObject.SetActive(false);
        }
    }
}
