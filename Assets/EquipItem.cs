using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    private EquipedITem Sword;
    private EquipedITem Axe;
    private EquipedITem Pickaxe;
    private EquipedITem Bow;

    private void Start()
    {
        EquipedITem[] auxiliarEquipedItems = gameObject.GetComponentsInChildren<EquipedITem>();

        Sword = auxiliarEquipedItems[0];
        Axe = auxiliarEquipedItems[1];
        Pickaxe = auxiliarEquipedItems[2];
        Bow = auxiliarEquipedItems[3];
    }

    public void Equip(Item item, ItemSlot previousSlot)
    {
        if(item is Weapon weapon)
        {
            Sword.SetItem(item, previousSlot);
        }
    }
}
