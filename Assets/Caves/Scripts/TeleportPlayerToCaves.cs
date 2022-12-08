using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportPlayerToCaves : MonoBehaviour
{
    [SerializeField] private CaveSystemHandler caveSystemHandler;

    private GameObject text;

    private GameObject player = null;

    private Keyboard keyboard;

    private bool fKeyPress = true;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>().gameObject;

        text.SetActive(false);

        keyboard = InputSystem.GetDevice<Keyboard>();
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
                caveSystemHandler.TeleportToCaves();
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }
}
