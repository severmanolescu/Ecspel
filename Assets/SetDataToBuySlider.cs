using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SetDataToBuySlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI value;

    [SerializeField] private Button buySellButton;

    [Header("Audio effects")]
    [SerializeField] private AudioClip buySound;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip buttonSound;

    [SerializeField] private AudioSource audioSource;

    private PlayerInventory playerInventory;

    private CoinsHandler coinsHandler;

    private Slider slider;

    private ItemSlot itemSlot;

    private Animator animator;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();

        slider.minValue = 1;

        value.text = "1";

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        coinsHandler = playerInventory.GetComponentInChildren<CoinsHandler>();

        animator = GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public void SetDataToBuy(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null && itemSlot.Item.Amount > 0)
        {
            slider.maxValue = itemSlot.Item.Amount;

            slider.value = 1;

            value.text = "1";

            this.itemSlot = itemSlot;

            buySellButton.onClick.RemoveAllListeners();

            buySellButton.onClick.AddListener(delegate { BuyButton(); });

            buySellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Cumpara";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetDataToSell(ItemSlot itemSlot)
    {
        if (itemSlot.Item != null && itemSlot.Item.Amount > 0)
        {
            slider.maxValue = itemSlot.Item.Amount;

            slider.value = 1;

            value.text = "1";

            this.itemSlot = itemSlot;

            buySellButton.onClick.RemoveAllListeners();

            buySellButton.onClick.AddListener(delegate { SellButton(); });

            buySellButton.GetComponentInChildren<TextMeshProUGUI>().text = "Vinde";
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SliderValueChange()
    {
        value.text = slider.value.ToString();
    }

    public void BuyButton()
    {
        if (itemSlot != null && itemSlot.Item != null)
        {
            if (coinsHandler.Amount >= (int)slider.value * itemSlot.Item.SellPrice * 2)
            {
                coinsHandler.Amount -= (int)slider.value * itemSlot.Item.SellPrice * 2;

                audioSource.clip = buySound;
                audioSource.Play();

                Item itemToAdd = itemSlot.Item.Copy();

                itemToAdd.Amount = (int)slider.value;

                bool canAdd = playerInventory.AddItem(itemToAdd);

                if (canAdd == true)
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

                animator.SetTrigger("Start");
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
            if (coinsHandler.Amount >= itemSlot.Item.Amount * itemSlot.Item.SellPrice * 2)
            {
                coinsHandler.Amount -= itemSlot.Item.Amount * itemSlot.Item.SellPrice * 2;

                audioSource.clip = buySound;
                audioSource.Play();

                Item itemToAdd = itemSlot.Item.Copy();

                bool canAdd = playerInventory.AddItem(itemToAdd);

                if (canAdd == true)
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
            coinsHandler.Amount += itemSlot.Item.SellPrice * itemSlot.Item.Amount;

            audioSource.clip = buySound;
            audioSource.Play();

            itemSlot.DeleteItem();

            itemSlot.DeleteItem();
        }
    }
}
