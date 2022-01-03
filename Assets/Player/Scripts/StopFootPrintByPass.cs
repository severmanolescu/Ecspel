using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFootPrintByPass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameObject footPrintSpawn = collision.transform.Find("FootPrintSpawnLocation").gameObject;


            footPrintSpawn.SetActive(!footPrintSpawn.activeSelf);
        }
    }
}
