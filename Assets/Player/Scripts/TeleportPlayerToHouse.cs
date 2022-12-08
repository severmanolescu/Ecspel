using UnityEngine;

public class TeleportPlayerToHouse : MonoBehaviour
{
    [SerializeField] private Transform bedLocation;

    [SerializeField] private GameObject houseCamera;

    private GameObject footprintSpawnLocation;

    [SerializeField] private GameObject[] cameras;

    private Transform playerTransform;

    private void Awake()
    {
        playerTransform = GameObject.Find("Global/Player").transform;

        footprintSpawnLocation = GameObject.FindWithTag("FoorprintSpawn");
    }

    public void Teleport()
    {
        foreach (GameObject camera in cameras)
        {
            camera.SetActive(false);
        }

        playerTransform.position = bedLocation.position;

        houseCamera.SetActive(true);

        footprintSpawnLocation.gameObject.SetActive(false);
    }
}
