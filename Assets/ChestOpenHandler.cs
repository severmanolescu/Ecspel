using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestOpenHandler : MonoBehaviour
{
    private GameObject player = null;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = null;

            text.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (player != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Tasta apasata!");
        }
    }
}
