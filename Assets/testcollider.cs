using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcollider : MonoBehaviour
{
    public int da = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        da++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        da++;
    }
}
