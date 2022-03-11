using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreseCaveObjects : MonoBehaviour
{
    public void Increse()
    {
        GetComponentInParent<SpawnLadderToNextCave>().IncredeDestroyedObjects(transform.position);
    }
}
