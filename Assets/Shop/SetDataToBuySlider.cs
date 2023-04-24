using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetDataToBuySlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI value;
    [SerializeField] private TextMeshProUGUI coins;

    [SerializeField] private Button buySellButton;

    [Header("Prices")]
    [SerializeField] private float priceBuyWithoutDiscount = 2f;
    [SerializeField] private float priceBuyWithDiscount = 1.5f;

    [SerializeField] private float priceSellWithoutDiscount = 1f;
    [SerializeField] private float priceSellWithDiscount = 1.25f;

    [Header("Audio effects")]
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip buttonSound;

    [SerializeField] private AudioSource audioSource;

    private PlayerInventory playerInventory;

    private CoinsHandler coinsHandler;

    private Slider slider;

    private ItemSlot itemSlot;

    private bool discount;

    //true - buy
    //false - sell
    private bool getItemType = false;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();

        slider.minValue = 1;

        slider.value = 1;

        value.text = "1";

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        coinsHandler = playerInventory.GetComponentInChildren<CoinsHandler>();

        coins.text = "";

        gameObject.SetActive(false);
    }

    public void SetDataToBuy(ItemSlot itemSlot, bool discount)
    {
        if (itemSlot.Item != null && itemSlot.Item.Amount > 0)
        {
            this.discount = discount;

            slider.maxValue = itemSlot.Item.Amount;

            slider.value = 1;

            value.text = "1";

            this.itemSlot = itemSlot;

            buySellButton.onClick.RemoveAllListeners();

            buySellButton.onClick.AddListener(delegate { BuyButton(); });

            buySellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cumpara";

            getItemType = true;

            SetTextToCoin();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDataToSell(ItemSlot itemSlot, bool discount)
    {
        if (itemSlot.Item != null && itemSlot.Item.Amount > 0)
        {
            this.discount = discount;

            slider.maxValue = itemSlot.Item.Amount;

            slider.value = 1;

            value.text = "1";

            this.itemSlot = itemSlot;

            buySellButton.onClick.RemoveAllListeners();

            buySellButton.onClick.AddListener(delegate { SellButton(); });

            buySellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Vinde";

            getItemType = false;

            SetTextToCoin();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetTextToCoin()
    {
        if (itemSlot != null && itemSlot.Item != null)
        {
            switch (getItemType)
            {
                case true:
                    {
                        if (discount == false)
                        {
                            coins.text = (int.Parse(slider.value.ToString()) * (int)itemSlot.Item.SellPrice * priceBuyWithoutDiscount).ToString() + "\\" +
                                         coinsHandler.Amount;

                            if (coinsHandler.Amount >= itemSlot.Item.Amount * itemSlot.Item.SellPrice * priceBuyWithoutDiscount)
                            {
                                coins.color = Color.white;
                            }
                            else
                            {
                                coins.color = Color.red;
                            }
                        }
                        else
                        {
                            coins.text = (int.Parse(slider.value.ToString()) * (int)itemSlot.Item.SellPrice * priceBuyWithDiscount).ToString() + "\\" +
                                         coinsHandler.Amount;

                            if (coinsHandler.Amount >= itemSlot.Item.Amount * itemSlot.Item.SellPrice * priceBuyWithDiscount)
                            {
                                coins.color = Color.white;
                            }
                            else
                            {
                                coins.color = Color.red;
                            }
                        }

                        break;
                    }
                case false:
                    {
                        coins.text = (int.Parse(slider.value.ToString()) * (int)itemSlot.Item.SellPrice).ToString();

                        break;
                    }
            }
        }
    }

    public void SliderValueChange()
    {
        value.text = slider.value.ToString();

        SetTextToCoin();
    }

    public void BuyButton()
    {
        if (itemSlot != null && itemSlot.Item != null)
        {
            if ((discount == false && coinsHandler.Amount >= (int)slider.value * itemSlot.Item.SellPrice * priceBuyWithoutDiscount) ||
                (discount == true && coinsHandler.Amount >= (int)slider.value * itemSlot.Item.SellPrice * priceBuyWithDiscount))
            {
                if (discount == false)
                {
                    coinsHandler.Amount -= (int)(slider.value * itemSlot.Item.SellPrice * priceBuyWithoutDiscount);
                }
                else
                {
                    coinsHandler.Amount -= (int)(slider.value * itemSlot.Item.SellPrice * priceBuyWithDiscount);
                }

                audioSource.clip = buySound;
                audioSource.Play();

                Item itemToAdd = itemSlot.Item.Copy();

                itemToAdd.Amount = (int)slider.value;

                int canAdd = playerInventory.AddItem(itemToAdd);

                if (canAdd == 0)
                {
                    itemSlot.DecreseAmount((int)slider.value);
                }
                else
                {
                    itemSlot.DecreseAmount((int)slider.value - itemToAdd.Amount);
                }

                gameObject.SetActive(false);
            }
            else
            {
                audioSource.clip = errorSound;
                audioSource.Play();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void BuyItem(ItemSlot itemSlot)
    {
        if (itemSlot != null && itemSlot.Item != null)
        {
            if ((discount == false && coinsHandler.Amount >= itemSlot.Item.Amount * itemSlot.Item.SellPrice * priceBuyWithoutDiscount) ||
                (discount == true && coinsHandler.Amount >= itemSlot.Item.Amount * itemSlot.Item.SellPrice * priceBuyWithDiscount))
            {
                coinsHandler.Amount -= itemSlot.Item.Amount * itemSlot.Item.SellPrice * 2;

                audioSource.clip = buySound;
                audioSource.Play();

                Item itemToAdd = itemSlot.Item.Copy();

                int canAdd = playerInventory.AddItem(itemToAdd);

                if (canAdd == 0)
                {
                    if (itemSlot != null)
                    {
                        itemSlot.DeleteItem();
                    }
                }
                else
                {
                    itemSlot.DecreseAmount(itemSlot.Item.Amount - itemToAdd.Amount);
                }
            }
            else
            {
                audioSource.clip = errorSound;
                audioSource.Play();
            }
        }
    }

    public void CancelButton()
    {
        audioSource.clip = buttonSound;
        audioSource.Play();

        gameObject.SetActive(false);
    }

    public void SellButton()
    {
        if (itemSlot != null && itemSlot.Item != null)
        {
            coinsHandler.Amount += (int)slider.value * itemSlot.Item.SellPrice;

            itemSlot.DecreseAmount((int)slider.value);

            audioSource.clip = buySound;
            audioSource.Play();

            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SellItem(ItemSlot itemSlot)
    {
        if (itemSlot != null && itemSlot.Item != null)
        {
            if (discount == false)
            {
                coinsHandler.Amount += (int)(itemSlot.Item.SellPrice * itemSlot.Item.Amount * priceSellWithoutDiscount);
            }
            else
            {
                coinsHandler.Amount += (int)(itemSlot.Item.SellPrice * itemSlot.Item.Amount * priceSellWithDiscount);
            }

            audioSource.clip = buySound;
            audioSource.Play();

            itemSlot.DeleteItem();
        }
    }

    public void Close()
    {
        slider.minValue = 1;

        slider.value = 1;

        value.text = "1";

        gameObject.SetActive(false);
    }
}
