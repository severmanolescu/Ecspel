using System.Collections.Generic;
using UnityEngine;

public class ChangeDataFromQuest : MonoBehaviour
{
    [Header("Data type:\n" +
        "1. Shop discount\n" +
        "2. Library discount")]

    [SerializeField] private OpenShopHandler shopHandler;
    [SerializeField] private OpenShopHandler libraryHandler;

    public void ChangeDataQuest(int dataChoose, bool value)
    {
        switch (dataChoose)
        {
            case 1: shopHandler.Discount = value; break;
            case 2: libraryHandler.Discount = value; break;
            default: return;
        }
    }

    public List<bool> GetData()
    {
        List<bool> data = new List<bool>();

        data.Add(shopHandler.Discount);
        data.Add(libraryHandler.Discount);

        return data;
    }

    public void SetData(List<bool> data)
    {
        for (int indexOfData = 0; indexOfData < data.Count; indexOfData++)
        {
            ChangeDataQuest(indexOfData + 1, data[indexOfData]);
        }
    }
}
