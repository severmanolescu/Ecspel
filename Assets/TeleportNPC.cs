using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportNPC : MonoBehaviour
{
    private Transform teleportToPoint;

    private void Awake()
    {
        teleportToPoint = GetComponent<TeleportPlayer>().TeleportToPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("NPC"))
        {
            collision.transform.position = teleportToPoint.position;
        }
    }
}
