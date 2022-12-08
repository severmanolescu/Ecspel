using UnityEngine;

public class IncreseCaveObjects : MonoBehaviour
{
    public void Increse()
    {
        GetComponentInParent<SpawnLadderToNextCave>().IncredeDestroyedObjects(transform.position);
    }
}
