using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] private GameObject prefabItem;

    public void SpawnItems(Item item, int noOfItems, Vector3 position)
    {
        for (int indexOfItems = 0; indexOfItems < noOfItems; indexOfItems++)
        {
            ItemWorld newItem = Instantiate(prefabItem).GetComponent<ItemWorld>();

            newItem.transform.position = position;

            newItem.SetItem(DefaulData.GetItemWithAmount(item, 1));
            newItem.MoveToPoint();
        }
    }

    public void SpawnItems(Item item, Vector3 position)
    {
        ItemWorld newItem = Instantiate(prefabItem).GetComponent<ItemWorld>();

        newItem.transform.position = position;

        newItem.SetItem(DefaulData.GetItemWithAmount(item, item.Amount));
        newItem.MoveToPoint();
    }
}
