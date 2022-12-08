using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TipsCanvas : MonoBehaviour
{
    [SerializeField] private CanvasTabsOpen canvasTabs;

    [SerializeField] private TextMeshProUGUI tipDetails;
    [SerializeField] private Toggle stopShow;

    [SerializeField] private PlayerMovement playerMovement;

    private bool notShow;

    public bool NotShow { get => notShow; set { notShow = value; stopShow.isOn = value; } }

    private void Awake()
    {
        tipDetails.text = "";

        stopShow.isOn = false;
    }

    public void CloseCanvas()
    {
        notShow = stopShow.isOn;

        canvasTabs.canOpenTabs = true;

        playerMovement.TabOpen = false;

        gameObject.SetActive(false);
    }

    public void SetTip(string tip)
    {
        if (!notShow)
        {
            tipDetails.text = tip;

            canvasTabs.canOpenTabs = false;

            playerMovement.TabOpen = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
