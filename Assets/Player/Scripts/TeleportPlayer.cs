using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private GameObject currentCamera;
    [SerializeField] private GameObject newCamera;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.position = teleportToPoint.position;

            currentCamera.SetActive(false);
            newCamera.SetActive(true);

            foreach(GameObject gameObject in objectsToSetActiveToFalse)
            {
                gameObject.SetActive(false);
            }

            foreach(GameObject gameObject in objectsToSetActiveToTrue)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
