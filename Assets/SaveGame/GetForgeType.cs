using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetForgeType : MonoBehaviour
{
    [SerializeField] private List<GameObject> forgePrefabs = new List<GameObject>();

    public GameObject GetForgeObject(int forgeID)
    {
        return forgePrefabs[forgeID];
    }
}
