using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanvasTabsOpen : MonoBehaviour
{
    private GameObject playerInventory;
    private QuickSlotsChanger quickSlot;

    private QuestTabDataSet questShow;

    private PlayerMovement playerMovement;

    private GameObject canvasEffects;

    private Keyboard keyboard;

    [Header("Menu canvas")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject chestStorage;
    [SerializeField] private GameObject craftingCanvas;
    [SerializeField] private GameObject forgeCanvas;
    [SerializeField] private GameObject shopItems;
    [SerializeField] private GameObject tips;

    public bool canOpenTabs = true;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);

        playerInventory.SetActive(false);
        canvasEffects.SetActive(false);

        questShow.DeleteData();
        questShow.gameObject.SetActive(false);

        menuCanvas.SetActive(false);
    }

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        canvasEffects = transform.Find("EffectDetailed").gameObject;

        playerInventory = transform.Find("PlayerItems").gameObject;
        quickSlot = transform.Find("Field/QuickSlots").gameObject.GetComponent<QuickSlotsChanger>();
        questShow = transform.Find("QuestTab").gameObject.GetComponent<QuestTabDataSet>();

        chestStorage.SetActive(false);

        forgeCanvas.SetActive(false);

        shopItems.SetActive(false);

        tips.SetActive(false);

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (canOpenTabs == true)
        {
            if (keyboard.eKey.wasPressedThisFrame)
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
            else if (keyboard.tabKey.wasPressedThisFrame)
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
            else if (keyboard.escapeKey.wasPressedThisFrame)
            {
                if(questShow.gameObject.activeSelf == false &&
                   playerInventory.activeSelf == false &&
                   chestStorage.activeSelf == false &&
                   craftingCanvas.activeSelf == false &&
                   forgeCanvas.activeSelf == false)
                {
                    if (menuPrincipal.activeSelf == true)
                    {
                        if (menuCanvas.activeSelf == true)
                        {
                            Time.timeScale = 1f;

                            playerMovement.TabOpen = false;

                            quickSlot.gameObject.SetActive(true);

                            canOpenTabs = true;
                        }
                        else
                        {
                            Time.timeScale = 0;

                            playerMovement.TabOpen = true;

                            quickSlot.gameObject.SetActive(false);

                            canOpenTabs = false;
                        }

                        menuCanvas.SetActive(!menuCanvas.activeSelf);
                    }
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

                    chestStorage.SetActive(false);

                    quickSlot.Reinitialize();

                    craftingCanvas.SetActive(false);

                    forgeCanvas.SetActive(false);

                    tips.SetActive(false);
                }                           
            }
        }
        else
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                if(chestStorage.activeSelf == true)
                {
                    chestStorage.GetComponent<ChestStorageHandler>().ChestOpenHandler.CloseChest();
                }
            }
        }
    }

    public void SetCanOpenTabs(bool canOpenTabs)
    {
        this.canOpenTabs = canOpenTabs;
    }

    public void ControllerTabPress()
    {
        Debug.Log("Das");
    }
}
