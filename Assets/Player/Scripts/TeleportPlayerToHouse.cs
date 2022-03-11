using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerToHouse : MonoBehaviour
{
    [SerializeField] private Transform bedLocation;

    [SerializeField] private GameObject houseCamera;

    GameObject footprintSpawnLocation;

    private GameObject[] cameras;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.Find("Global/Player").transform;

        cameras = GameObject.FindGameObjectsWithTag("MainCamera");

        footprintSpawnLocation = GameObject.FindWithTag("FoorprintSpawn");
    }

    public void Teleport()
    {
        foreach(GameObject camera in cameras)
        {
            camera.SetActive(false);
        }

        playerTransform.position = bedLocation.position;

        houseCamera.SetActive(true);

        footprintSpawnLocation.gameObject.SetActive(false);
    }
}
