using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DestroyBirdAtSpawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Bird"))
        {
            GetComponentInParent<BirdAI>().DespawnAnimal();
        }
    }
}
