using System.Collections;
using UnityEngine;

public class ItemWorldTest : MonoBehaviour
{
    public Item testItem;

    public int amount = 1;

    private void Awake()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);

        Item newItem = testItem.Copy();

        newItem.Amount = amount;

        GetComponent<ItemWorld>().SetItem(newItem);
    }
}
