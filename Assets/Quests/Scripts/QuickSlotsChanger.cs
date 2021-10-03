using System.Collections;
using UnityEngine;

public class QuickSlotsChanger : MonoBehaviour
{
    private QuickSlot[] quickSlots;

    private int selectedItemIndex;

    public int SelectedItemIndex { get{ return selectedItemIndex; } }
    public Item Item{ get{ return quickSlots[selectedItemIndex - 1].Item; } }

    private void Awake()
    {
        quickSlots = gameObject.GetComponentsInChildren<QuickSlot>();
    }

    private void Start()
    {
        ChangeSelectedItem(1);
    }

    public void Reinitialize()
    {
        foreach(QuickSlot slot in quickSlots)
        {
            slot.Reinitialize();
        }
    }

    private void ChangeSelectedItem(int selected)
    {
        foreach(QuickSlot slot in quickSlots)
        {
            slot.DeselectItem();
        }

        quickSlots[selected - 1].SelectedItem();

        selectedItemIndex = selected;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSelectedItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSelectedItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSelectedItem(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSelectedItem(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSelectedItem(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeSelectedItem(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ChangeSelectedItem(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            ChangeSelectedItem(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ChangeSelectedItem(9);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ChangeSelectedItem(10);
        }
    }
}
