using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LetterHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI details;

    private PlayerMovement playerMovement;
    private CanvasTabsOpen canvasTabs;

    private bool opened = false;

    private void Awake()
    {
        gameObject.SetActive(false);

        title.text = string.Empty;
        details.text = string.Empty;

        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
        canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();
    }

    public void SetData(Letter letter)
    {
        if (letter != null)
        {
            title.text = letter.Title;
            details.text = letter.TextDetails;

            playerMovement.TabOpen = true;
            canvasTabs.SetCanOpenTabs(false);

            opened = true;
        }
    }

    private void Update()
    {
        if (opened)
        {
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                title.text = string.Empty;
                details.text = string.Empty;

                playerMovement.TabOpen = false;
                canvasTabs.SetCanOpenTabs(true);

                opened = false;

                gameObject.SetActive(false);
            }
        }
    }
}
