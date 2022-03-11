using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class TeleportToNextCave : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private CaveSystemHandler caveSystemHandler;

    private bool playerInSpace = false;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        caveSystemHandler = GameObject.Find("Caves").GetComponent<CaveSystemHandler>();

        textMesh.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerInSpace = true;

            textMesh.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSpace = false;

            textMesh.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if(playerInSpace == true && Keyboard.current.fKey.wasPressedThisFrame)
        {
            caveSystemHandler.TeleportToNextCave();
        }
    }
}
