using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetChestType : MonoBehaviour
{
    [SerializeField] private List<GameObject> chestPrefabs = new List<GameObject>();

    public GameObject GetChestObject(int chestId)
    {
        return chestPrefabs[chestId];
    }
}
