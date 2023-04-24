using System.Collections.Generic;
using UnityEngine;

public class EnemyDropLoot : MonoBehaviour
{
    [SerializeField] private List<ItemWithAmount> drops = new List<ItemWithAmount>();

    private SpawnItem spawnItem;

    private void Awake()
    {
        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();
    }

    public void DropItem()
    {
        foreach (ItemWithAmount drop in drops)
        {
            spawnItem.SpawnItems(drop.Item, drop.Amount, transform.position);

            return;
        }
    }
}
