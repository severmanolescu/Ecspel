using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropLoot : MonoBehaviour
{
    [SerializeField] private List<QuestItems> drops = new List<QuestItems>();

    [SerializeField] private GameObject itemWorldPrefab;

    public void DropItem()
    {
        foreach(QuestItems questItems in drops)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            Item item = Instantiate(questItems.Item);

            item.Amount = questItems.Drop;

            itemWorld.SetItem(item);

            itemWorld.transform.position = transform.position;

            itemWorld.MoveToPoint();

            return;
        }
    }
}
