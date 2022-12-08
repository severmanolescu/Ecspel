using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportPlayerOutsideOfCave : MonoBehaviour
{
    private GameObject currentCamera;

    private List<GameObject> objectsToSetActiveToFalse = new();
    private List<GameObject> objectsToSetActiveToTrue = new();

    private LocationGridSave newGrid;

    private CaveSystemHandler caveSystemHandler;

    private GameObject text;

    private GameObject player = null;

    private Keyboard keyboard;

    private bool fKeyPress = true;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>().gameObject;

        text.SetActive(false);

        keyboard = InputSystem.GetDevice<Keyboard>();

        currentCamera = GameObject.Find("Caves/CameraCaveArea");

        objectsToSetActiveToTrue.Add(GameObject.Find("Global/Player/FootPrintSpawnLocation"));

        caveSystemHandler = GameObject.Find("Caves").GetComponent<CaveSystemHandler>();

        newGrid = caveSystemHandler.LocationGrid;
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
        if (collision.CompareTag("Player"))
        {
            player = null;

            text.SetActive(false);
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (keyboard.fKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                currentCamera.SetActive(false);

                foreach (GameObject gameObject in objectsToSetActiveToFalse)
                {
                    gameObject.SetActive(false);
                }

                foreach (GameObject gameObject in objectsToSetActiveToTrue)
                {
                    gameObject.SetActive(true);
                }

                GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().LocationGrid = newGrid;
                GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().StopRainSound();

                caveSystemHandler.TeleportOutside();
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }
}
