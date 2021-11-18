using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCraft : MonoBehaviour
{
    [SerializeField] private List<Craft> craft = new List<Craft>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            foreach (Craft crafts in craft)
            {
                GetComponent<CraftCanvasHandler>().AddCraft(crafts);
            }
        }
    }
}
