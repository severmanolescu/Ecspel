using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemWorldSpawn : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;

    public void SpawnItem(Vector3 position, Item item)
    {
        Transform transform = Instantiate(ItemSprites.Instance.ItemWorld, position, Quaternion.identity);

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.SetItem(item);
    }
}
