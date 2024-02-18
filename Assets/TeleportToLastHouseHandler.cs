using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToLastHouseHandler : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private GameObject currentCamera;
    [SerializeField] private GameObject newCamera;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    [SerializeField] private Transform playerLocation;

    [SerializeField] private TransferImageHandler transferImageHandler;

    private PlayerMovement playerMovement;

    private DayTimerHandler dayTimerHandler;

    public Transform TeleportToPoint { get => teleportToPoint; set => teleportToPoint = value; }

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
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

    private IEnumerator WaitForBack(GameObject collision)
    {
        playerMovement.TabOpen = true;

        transferImageHandler.StartTransfer();

        yield return new WaitForSeconds(2f);

        collision.transform.position = TeleportToPoint.position;

        currentCamera.SetActive(false);

        SetObject();

        newCamera.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        transferImageHandler.HideImage();

        playerMovement.TabOpen = false;

        dayTimerHandler.StopTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(WaitForBack(collision.gameObject));
        }
    }
}
