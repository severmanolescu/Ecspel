using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipsCanvas : MonoBehaviour
{
    [SerializeField] private CanvasTabsOpen canvasTabs;    

    [SerializeField] private TextMeshProUGUI tipDetails;
    [SerializeField] private Toggle stopShow;

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

        Time.timeScale = 1f;

        canvasTabs.canOpenTabs = true;

        gameObject.SetActive(false);
    }

    public void SetTip(string tip)
    {
        if (!notShow)
        {
            tipDetails.text = tip;

            Time.timeScale = 0;

            canvasTabs.canOpenTabs = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
