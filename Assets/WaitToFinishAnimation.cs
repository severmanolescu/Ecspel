using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitToFinishAnimation : MonoBehaviour
{
    public void FinishAnimation()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
