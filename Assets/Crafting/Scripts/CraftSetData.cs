using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftSetData : MonoBehaviour
{
    private ItemSprites itemSprites;

    private Craft craft = null;

    private bool haveItems = false;

    private Image background;
    private Image receiveItem;
    private Image needItem1;
    private Image needItem2;
    private Image needItem3;

    private PlayerInventory playerInventory;

    public Craft Craft { get => craft; set => SetData(value); }

    private void Awake()
    {
        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        Image[] images = GetComponentsInChildren<Image>();

        background  = images[0];
        receiveItem = images[1];
        needItem1   = images[2];
        needItem2   = images[3];
        needItem3   = images[4];
    }

    private void SetData(Craft craft)
    {
        if (craft != null)
        {
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

            foreach (ItemWithAmount item in craft.NeedItem)
            {
                auxItem = item.Item.Copy();
                auxItem.Amount = item.Amount;

                playerInventory.DeleteItem(auxItem);
            }

            auxItem = craft.ReceiveItem.Item.Copy();
            auxItem.Amount = craft.ReceiveItem.Amount;

            playerInventory.AddItem(auxItem);
        }

        GetComponentInParent<CraftCanvasHandler>().ReinitializeAllCraftings();
    }

    private void ChangeColorSprites(Color color)
    {
        background.color = color;
        receiveItem.color = color;
        needItem1.color = color;
        needItem2.color = color;
        needItem3.color = color;
    }

    public void CheckIfItemsAreAvaible()
    {
        haveItems = true;

        foreach (ItemWithAmount item in craft.NeedItem)
        {
            if (playerInventory.SearchInventory(item.Item, item.Amount) == false)
            {
                haveItems = false;

                break;
            }
        }

        if(haveItems)
        {
            ChangeColorSprites(Color.white);   
        }
        else
        {
            ChangeColorSprites(Color.red);
        }
    }
}
