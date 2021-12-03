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

    [SerializeField] private LocationGridSave newGrid;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.position = teleportToPoint.position;

            currentCamera.SetActive(false);
            newCamera.SetActive(true);

            Vector3 positionCamera = new Vector3();

            positionCamera.z = newCamera.transform.position.z;

            CameraHandler newCameraPositon = newCamera.GetComponent<CameraHandler>();

            if(collision.transform.position.x >= newCameraPositon.MinX && collision.transform.position.x <= newCameraPositon.MaxX)
            {
                positionCamera.x = collision.transform.position.x;
            }
            else
            {
                if(collision.transform.position.x < newCameraPositon.MinX)
                {
                    positionCamera.x = newCameraPositon.MinX;
                }
                else
                {
                    positionCamera.x = newCameraPositon.MaxX;
                }
            }

            if (collision.transform.position.y >= newCameraPositon.MinY && collision.transform.position.y <= newCameraPositon.MaxY)
            {
                positionCamera.y = collision.transform.position.y;
            }
            else
            {
                if (collision.transform.position.y < newCameraPositon.MinY)
                {
                    positionCamera.y = newCameraPositon.MinY;
                }
                else
                {
                    positionCamera.y = newCameraPositon.MaxY;
                }
            }

            newCamera.transform.position = positionCamera;

            foreach (GameObject gameObject in objectsToSetActiveToFalse)
            {
                gameObject.SetActive(false);
            }

            foreach(GameObject gameObject in objectsToSetActiveToTrue)
            {
                gameObject.SetActive(true);
            }

            GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().LocationGrid = newGrid;
        }
    }
}
