using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : Event
{
    [SerializeField] private Item itemToCollect;

    [SerializeField] private int amount;

    [SerializeField] private Dialogue dialogue;

    public void AddItemToInventory()
    {
        if(canTrigger)
        {
            amount = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>().AddItem(GetItem());

            if(amount == 0)
            {
                if(dialogue != null)
                {
                    GameObject.Find("Global").GetComponent<SetDialogueToPlayer>().SetDialogue(dialogue);
                }

                DestroyObject();
            }
        }
    }

    public Item GetItem()
    {
        Item auxItem = itemToCollect.Copy();

        auxItem.Amount = amount;

        return auxItem;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
