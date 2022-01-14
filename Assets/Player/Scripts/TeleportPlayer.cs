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

    [SerializeField] private bool deactivatePrevLocation = true;

    private LocationGridSave newGrid;
    private LocationGridSave oldGrid;

    public Transform TeleportToPoint { get => teleportToPoint; set => teleportToPoint = value; }

    private void Awake()
    {
        newGrid = newCamera.GetComponentInParent<LocationGridSave>();
        oldGrid = currentCamera.GetComponentInParent<LocationGridSave>();
    }

    private void SetObject()
    {
        foreach (GameObject gameObject in objectsToSetActiveToFalse)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in objectsToSetActiveToTrue)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            newGrid.gameObject.SetActive(true);

            collision.transform.position = TeleportToPoint.position;

            if (oldGrid != null && deactivatePrevLocation == true)
            {
                oldGrid.ChangeLocation();
            }

            currentCamera.SetActive(false);

            Vector3 positionCamera = new Vector3();

            positionCamera.z = newCamera.transform.position.z;

            CameraHandler newCameraPositon = newCamera.GetComponent<CameraHandler>();

            newCamera.transform.position = positionCamera;

            SetObject();

            GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().LocationGrid = newGrid;

            newCamera.SetActive(true);

            newGrid.SpawnEnemy();
        }
    }
}
