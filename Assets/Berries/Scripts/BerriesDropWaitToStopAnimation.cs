using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerriesDropWaitToStopAnimation : MonoBehaviour
{
    public void DeactivateCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void ActivateCollider()
    {
        GetComponent<BoxCollider2D>().enabled = true;

        Destroy(this);
    }
}
