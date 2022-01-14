using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TeleportPlayerKeyPress : MonoBehaviour
{
    [Header("Can enter interval:")]
    [SerializeField] private int startHour;
    [SerializeField] private int finishHour;

    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private GameObject currentCamera;
    [SerializeField] private GameObject newCamera;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    [SerializeField] private LocationGridSave newGrid;

    private WorldTextDetails worldText;

    private DayTimerHandler dayTimer;

    private GameObject text;

    private GameObject player = null;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>().gameObject;

        text.SetActive(false);

        dayTimer = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();

        worldText = GameObject.Find("Global/Player/Canvas/WorldTextDetails").GetComponent<WorldTextDetails>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = null;

            text.SetActive(false);
        }
    }

    private void Update()
    {
        if(player != null && Input.GetKeyDown(KeyCode.F))
        {
            if (dayTimer.Hours >= startHour && dayTimer.Hours <= finishHour)
            {
                player.transform.position = teleportToPoint.position;

                currentCamera.SetActive(false);
                newCamera.SetActive(true);

                foreach (GameObject gameObject in objectsToSetActiveToFalse)
                {
                    gameObject.SetActive(false);
                }

                foreach (GameObject gameObject in objectsToSetActiveToTrue)
                {
                    gameObject.SetActive(true);
                }

                GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().LocationGrid = newGrid;
            }
            else
            {
                worldText.ShowText("Usa inchisa!");
            }
        }
    }
}
