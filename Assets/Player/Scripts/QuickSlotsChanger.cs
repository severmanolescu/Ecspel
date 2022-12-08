using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSlotsChanger : MonoBehaviour
{
    private QuickSlot[] quickSlots;

    private int selectedItemIndex;

    private BuildSystemHandler buildSystem;

    private Keyboard keyboard;
    private Mouse mouse;

    private bool forwardButtonPress = true;
    private bool backButtonPress = true;

    public int SelectedItemIndex { get { return selectedItemIndex; } }
    public Item Item { get { return quickSlots[selectedItemIndex - 1].Item; } }

    private void Awake()
    {
        quickSlots = gameObject.GetComponentsInChildren<QuickSlot>();

        buildSystem = GameObject.Find("BuildSystem").GetComponent<BuildSystemHandler>();

        keyboard = InputSystem.GetDevice<Keyboard>();
        mouse = InputSystem.GetDevice<Mouse>();
    }

    private void Start()
    {
        ChangeSelectedItem(1);

        buildSystem.SetQuickSlotHandler(this);
    }

    public void Reinitialize()
    {
        foreach (QuickSlot slot in quickSlots)
        {
            slot.Reinitialize();
        }
    }

    public void ReinitializeSelectedSlot()
    {
        quickSlots[selectedItemIndex - 1].Reinitialize();
    }

    public void DecreseAmountSelected(int amount)
    {
        quickSlots[selectedItemIndex - 1].Equiped.DecreseAmount(amount);
    }

    private void ChangeSelectedItem(int selected)
    {
        foreach (QuickSlot slot in quickSlots)
        {
            slot.DeselectItem();
        }

        quickSlots[selected - 1].SelectedItem();

        selectedItemIndex = selected;
    }

    public void SetItem(Item item)
    {
        quickSlots[selectedItemIndex - 1].SetItem(item);
    }

    private void Update()
    {
        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(1);
        }
        else if (keyboard.digit2Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(2);
        }
        else if (keyboard.digit3Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(3);
        }
        else if (keyboard.digit4Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(4);
        }
        else if (keyboard.digit5Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(5);
        }
        else if (keyboard.digit6Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(6);
        }
        else if (keyboard.digit7Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(7);
        }
        else if (keyboard.digit8Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(8);
        }
        else if (keyboard.digit9Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(9);
        }
        else if (keyboard.digit0Key.wasPressedThisFrame)
        {
            ChangeSelectedItem(10);
        }

        if (mouse.scroll.y.ReadValue() == 120 || (Joystick.current != null && Joystick.current.allControls[6].IsPressed() == false && forwardButtonPress == false))
        {
            if (selectedItemIndex > 1)
            {
                selectedItemIndex--;
                ChangeSelectedItem(selectedItemIndex);
            }

            forwardButtonPress = true;
        }
        else if (Joystick.current != null && Joystick.current.allControls[6].IsPressed() == true)
        {
            forwardButtonPress = false;
        }

        if (mouse.scroll.y.ReadValue() == -120 || (Joystick.current != null && Joystick.current.allControls[7].IsPressed() == false && backButtonPress == false))
        {
            if (selectedItemIndex < 10)
            {
                selectedItemIndex++;
                ChangeSelectedItem(selectedItemIndex);
            }

            backButtonPress = true;
        }
        else if (Joystick.current != null && Joystick.current.allControls[7].IsPressed() == true)
        {
            backButtonPress = false;
        }
    }
}