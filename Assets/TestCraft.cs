using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCraft : MonoBehaviour
{
    [SerializeField] private Craft craft;

    private void Start()
    {
        GetComponent<CraftSetData>().Craft = craft;
    }
}
