using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGamePositionNPC : MonoBehaviour
{
    [SerializeField] private Vector3 position;

    public void LoadGame()
    {
        transform.position = position;

        DestroyScript();
    }

    public void DestroyScript()
    {
        Destroy(this);
    }
}
