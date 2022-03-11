using UnityEngine;
using UnityEngine.UI;

public class ForgeHandler : MonoBehaviour
{
    [SerializeField] private Slider forgeProgress;
    [SerializeField] private Slider fuelProgress;

    [SerializeField] private ItemSlot inputSlot;
    [SerializeField] private ItemSlot fuelSlot;
    [SerializeField] private ItemSlot outputSlot;

    [SerializeField] private Image fireImage;

    [SerializeField] private Color workingForgeColor;

    private ForgeOpenHandler forge;

    private void Awake()
    {
        forgeProgress.gameObject.SetActive(false);

        fuelProgress.gameObject.SetActive(false);

        fireImage.color = Color.white;
    }

    public void SetDataAtOpen(Item inputItem, Item fuelItem, Item outputItem, ForgeOpenHandler forgeOpenHandler, float smeltingProgress, float fuelProgress, bool working)
    {
        if (inputItem == null)
        {
            inputSlot.DeleteItem();
        }
        else
        {
            inputSlot.SetItem(inputItem);
        }
        if (fuelItem == null)
        {
            fuelSlot.DeleteItem();
        }
        else
        {
            fuelSlot.SetItem(fuelItem);
        }
        if (outputItem == null)
        {
            outputSlot.DeleteItem();
        }
        else
        {
            outputSlot.SetItem(outputItem);
        }

        forge = forgeOpenHandler;

        if(working)
        {
            SetValuetoForgeSlider(smeltingProgress);
            SetValuetoFuelSlider(fuelProgress);
        }
    }

    public void GetItems(out Item inputItem, out Item fuelItem, out Item outputItem)
    {
        if (inputSlot.Item != null)
        {
            inputItem = inputSlot.Item.Copy();
        }
        else
        {
            inputItem = null;
        }

        if (fuelSlot.Item != null)
        {
            fuelItem = fuelSlot.Item.Copy();
        }
        else
        {
            fuelItem = null;
        }

        if (outputSlot.Item != null)
        {
            outputItem = outputSlot.Item.Copy();
        }
        else
        {
            outputItem = null;
        }
    }

    public void HideDataAtClose()
    {
        forge = null;
    }

    public void ChangeItemInSlot(ItemSlot itemSlot)
    {
        if (forge != null)
        {
            if (itemSlot == inputSlot)
            {
                forge.InputItem = inputSlot.Item;
            }
            else if (itemSlot == fuelSlot)
            {
                forge.FuelItem = fuelSlot.Item;
            }
            else if (itemSlot == outputSlot)
            {
                forge.OutputItem = outputSlot.Item;
            }
        }
    }

    public void SetValuetoFuelSlider(float value)
    {
        fuelProgress.value = value;

        fuelProgress.gameObject.SetActive(true);
    }

    public void HideFuelProgress()
    {
        fuelProgress.gameObject.SetActive(false);
    }

    public void SetValuetoForgeSlider(float value)
    {
        forgeProgress.value = value;

        forgeProgress.gameObject.SetActive(true);
    }

    public void HideForgeProgress()
    {
        forgeProgress.gameObject.SetActive(false);
    }

    public void ChangeFuelItem(Item item)
    {
        fuelSlot.SetItem(item);
    }

    public void ChangeInputItem(Item item)
    {
        inputSlot.SetItem(item);
    }

    public void ChangeOutputItem(Item item)
    {
        outputSlot.SetItem(item);
    }
}
