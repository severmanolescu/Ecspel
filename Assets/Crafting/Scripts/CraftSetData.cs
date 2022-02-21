using UnityEngine;
using UnityEngine.UI;

public class CraftSetData : MonoBehaviour
{
    [SerializeField] private GameObject itemWorldPrefab;

    private ItemSprites itemSprites;

    private Craft craft = null;

    private Transform playerLocation;

    private bool haveItems = false;

    private CraftCanvasHandler canvasHandler = null;

    private Image background;
    private Image receiveItem;
    private Image needItem1;
    private Image needItem2;
    private Image needItem3;

    private PlayerInventory playerInventory;

    public Craft Craft { get => craft; set => SetData(value); }
    public CraftCanvasHandler CanvasHandler { get => canvasHandler; set => canvasHandler = value; }

    private void Awake()
    {
        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        playerLocation = GameObject.Find("Global/Player").transform;
       
        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        Image[] images = GetComponentsInChildren<Image>();

        if (images.Length >= 5)
        {
            background = images[0];
            receiveItem = images[1];
            needItem1 = images[2];
            needItem2 = images[3];
            needItem3 = images[4];
        }
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
        if(haveItems == true)
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

                itemWorld.SetItem(auxItem);

                itemWorld.transform.position = playerLocation.position;

                itemWorld.MoveToPoint();
            }
        }

        GetComponentInParent<CraftCanvasHandler>().ReinitializeAllCraftings();
    }

    private void ChangeColorSprites(int indexOfItem, Color color)
    {
        switch (indexOfItem)
        {
            case 0: needItem1.color = color; break;
            case 1: needItem2.color = color; break;
            case 2: needItem3.color = color; break;
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
    }
}
