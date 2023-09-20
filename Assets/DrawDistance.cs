using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDistance : MonoBehaviour
{
    private GameObject mainGameObject = null;

    private void Start()
    {
        mainGameObject = GetComponentsInChildren<Transform>()[1].gameObject;

        if(mainGameObject != null )
        {
            mainGameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.gameObject.name.Equals("DrawDistance"))
        {
            mainGameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.name.Equals("DrawDistance"))
        {
            mainGameObject.SetActive(false);
        }
    }
}
