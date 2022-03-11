using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HelpHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI helpText;
    [SerializeField] private TextMeshProUGUI pageIndexText;

    [TextArea(7, 7)]
    [SerializeField] private List<string> helpPages = new List<string>();

    private int indexOfPages = 0;

    private QuickSlotsChanger quickSlot;
    private PlayerMovement playerMovement;
    private CanvasTabsOpen canvasTabs;

    private void Awake()
    {
        helpText.text = string.Empty;
        pageIndexText.text = string.Empty;

        quickSlot = GameObject.Find("Global/Player/Canvas/Field/QuickSlots").GetComponent<QuickSlotsChanger>();
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
        canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        gameObject.SetActive(false);
    }

    public void StartHelp()
    {
        indexOfPages = 0;

        gameObject.SetActive(true);

        if(helpPages != null && indexOfPages < helpPages.Count)
        {
            quickSlot.gameObject.SetActive(false);
            playerMovement.TabOpen = true;
            canvasTabs.canOpenTabs = false;

            helpText.text = helpPages[indexOfPages];

            pageIndexText.text = (indexOfPages + 1).ToString() + "/" + helpPages.Count.ToString();

            Time.timeScale = 0f;
        }
    }

    public void NextHelpPage()
    {
        indexOfPages++;

        if (helpPages != null && indexOfPages < helpPages.Count)
        {
            helpText.text = helpPages[indexOfPages];

            pageIndexText.text = (indexOfPages + 1).ToString() + "/" + helpPages.Count.ToString();
        }
    }

    public void CloseHelp()
    {
        quickSlot.gameObject.SetActive(true);
        playerMovement.TabOpen = false;
        canvasTabs.canOpenTabs = true;

        Time.timeScale = 1f;

        helpText.text = string.Empty;
        pageIndexText.text = string.Empty;

        gameObject.SetActive(false);
    }

    public void PlayButtonSound()
    {
        GetComponent<AudioSource>().Play();
    }
}
