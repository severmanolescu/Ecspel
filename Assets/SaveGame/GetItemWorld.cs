using System.Collections.Generic;
using UnityEngine;

public class GetItemWorld : MonoBehaviour
{
    [SerializeField] private List<GameObject> listOfObjects = new List<GameObject>();

    public GameObject GetObjectFromNo(int objectNo)
    {
        if (objectNo > 0 && objectNo <= listOfObjects.Count)
        {
            return listOfObjects[objectNo - 1];
        }

        return null;
    }
}
