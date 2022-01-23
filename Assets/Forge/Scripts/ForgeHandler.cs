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

    public void SetDataAtOpen(Item inputItem, Item fuelItem, Item outputItem, ForgeOpenHandler forgeOpenHandler)
    {
        inputSlot.SetItem(inputItem);
        fuelSlot.SetItem(fuelItem);
        outputSlot.SetItem(outputItem);

        forge = forgeOpenHandler;
    }

    public void HideDataAtClose()
    {
        inputSlot.Item = null;
        fuelSlot.Item = null;
        outputSlot.Item = null;

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
