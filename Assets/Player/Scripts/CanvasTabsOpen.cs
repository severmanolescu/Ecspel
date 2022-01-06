using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTabsOpen : MonoBehaviour
{
    private GameObject playerInventory;
    private QuickSlotsChanger quickSlot;

    private QuestTabDataSet questShow;

    private PlayerMovement playerMovement;

    private GameObject canvasEffects;

    [Header("Menu canvas")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject menuSettingsCanvas;
    [SerializeField] private GameObject menuQuitCanvas;
    [SerializeField] private GameObject chestStorage;

    private bool canOpenTabs = true;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

        playerInventory.SetActive(false);
        canvasEffects.SetActive(false);

        questShow.DeleteData();
        questShow.gameObject.SetActive(false);
    }

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        canvasEffects = transform.Find("Field/Effect").gameObject;

        playerInventory = transform.Find("PlayerItems").gameObject;
        quickSlot = transform.Find("Field/QuickSlots").gameObject.GetComponent<QuickSlotsChanger>();
        questShow = transform.Find("QuestTab").gameObject.GetComponent<QuestTabDataSet>();

        chestStorage.SetActive(false);
    }

    private void Start()
    {
        menuCanvas.SetActive(false);

        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (canOpenTabs == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (questShow.gameObject.activeSelf || !playerInventory.activeSelf)
                {
                    questShow.DeleteData();
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(true);
                    canvasEffects.SetActive(true);

                    quickSlot.gameObject.SetActive(false);

                    playerMovement.TabOpen = true;

                }
                else if (playerInventory.activeSelf)
                {
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(true);
                    quickSlot.Reinitialize();

                    playerMovement.TabOpen = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (playerInventory.activeSelf || !questShow.gameObject.activeSelf)
                {
                    questShow.gameObject.SetActive(true);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(false);

                    playerMovement.TabOpen = true;
                }
                else if (questShow.gameObject.activeSelf)
                {
                    questShow.DeleteData();
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(true);
                    quickSlot.Reinitialize();

                    playerMovement.TabOpen = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuSettingsCanvas.activeSelf == false && 
                    menuQuitCanvas.gameObject.activeSelf == false)                    
                {
                    if(questShow.gameObject.activeSelf == false &&
                       playerInventory.activeSelf == false)
                    {
                        if (menuCanvas.activeSelf == true)
                        {
                            Time.timeScale = 1f;

                            playerMovement.TabOpen = false;

                            quickSlot.gameObject.SetActive(true);
                        }
                        else
                        {
                            Time.timeScale = 0;

                            playerMovement.TabOpen = true;

                            quickSlot.gameObject.SetActive(false);
                        }

                        menuCanvas.SetActive(!menuCanvas.activeSelf);
                    }
                    else
                    {
                        questShow.DeleteData();
                        questShow.gameObject.SetActive(false);

                        playerInventory.SetActive(false);
                        canvasEffects.SetActive(false);

                        quickSlot.gameObject.SetActive(true);
                        quickSlot.Reinitialize();

                        playerMovement.TabOpen = false;
                    }
                }                             
            }
        }
    }

    public void SetCanOpenTabs(bool canOpenTabs)
    {
        this.canOpenTabs = canOpenTabs;
    }
}
