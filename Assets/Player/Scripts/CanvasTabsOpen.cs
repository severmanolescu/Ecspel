using System.Collections;
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

    private bool inventoryButtonPress = true;
    private bool questButtonPress = true;
    private bool skillsButtonPress = true;

    [Header("Menu canvas")]
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject chestStorage;
    [SerializeField] private GameObject craftingCanvas;
    [SerializeField] private GameObject forgeCanvas;
    [SerializeField] private GameObject shopItems;
    [SerializeField] private GameObject tips;
    [SerializeField] private GameObject caveSelect;
    [SerializeField] private GameObject skills;
    [SerializeField] private GameObject help;
    [SerializeField] private GameObject letters;

    public bool canOpenTabs = true;

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        canvasEffects = transform.Find("EffectDetailed").gameObject;

        playerInventory = transform.Find("PlayerItems").gameObject;
        quickSlot = transform.Find("QuickSlots").gameObject.GetComponent<QuickSlotsChanger>();
        questShow = transform.Find("QuestTab").gameObject.GetComponent<QuestTabDataSet>();

        chestStorage.SetActive(false);

        forgeCanvas.SetActive(false);

        shopItems.SetActive(false);

        tips.SetActive(false);

        caveSelect.SetActive(false);

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void Start()
    {
        playerInventory.SetActive(false);
        canvasEffects.SetActive(false);

        //questShow.DeleteData();
        //questShow.gameObject.SetActive(false);

        //menuCanvas.SetActive(false);
        //skills.SetActive(false);
    }

    private void Update()
    {
        if (canOpenTabs == true)
        {
            if (keyboard.eKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[1].IsPressed() == false && inventoryButtonPress == false))
            {
                inventoryButtonPress = true;

                if (questShow.gameObject.activeSelf || !playerInventory.activeSelf)
                {
                    //questShow.DeleteData();
                    //questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(true);
                    canvasEffects.SetActive(true);

                    quickSlot.gameObject.SetActive(false);

                    caveSelect.SetActive(false);
                    skills.SetActive(false);

                    playerMovement.TabOpen = true;

                }
                else if (playerInventory.activeSelf)
                {
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(true);
                    quickSlot.Reinitialize();

                    caveSelect.SetActive(false);
                    skills.SetActive(false);

                    playerMovement.TabOpen = false;
                }
            }
            else if (keyboard.tabKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[4].IsPressed() == false && questButtonPress == false))
            {
                questButtonPress = true;

                if (playerInventory.activeSelf || !questShow.gameObject.activeSelf)
                {
                    questShow.gameObject.SetActive(true);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(false);

                    caveSelect.SetActive(false);
                    skills.SetActive(false);

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

                    caveSelect.SetActive(false);
                    skills.SetActive(false);

                    playerMovement.TabOpen = false;
                }
            }
            else if (keyboard.qKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[2].IsPressed() == false && skillsButtonPress == false))
            {
                skillsButtonPress = true;

                if (skills.activeSelf)
                {
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(true);
                    quickSlot.Reinitialize();

                    caveSelect.SetActive(false);
                    skills.SetActive(false);

                    playerMovement.TabOpen = false;
                }
                else
                {
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    canvasEffects.SetActive(false);

                    quickSlot.gameObject.SetActive(false);


                    caveSelect.SetActive(false);

                    skills.SetActive(true);

                    skills.GetComponent<SkillsHandler>().ShowSkills();

                    playerMovement.TabOpen = true;
                }
            }
            else if (keyboard.escapeKey.wasPressedThisFrame)
            {
                if (questShow.gameObject.activeSelf == false &&
                   playerInventory.activeSelf == false &&
                   chestStorage.activeSelf == false &&
                   craftingCanvas.activeSelf == false &&
                   forgeCanvas.activeSelf == false &&
                   caveSelect.activeSelf == false &&
                   skills.activeSelf == false &&
                   help.activeSelf == false &&
                   letters.activeSelf == false)
                {
                    if (menuPrincipal.activeSelf == true)
                    {
                        if (menuCanvas.activeSelf == true)
                        {
                            CloseMenu();
                        }
                        else
                        {
                            Time.timeScale = 0;

                            playerMovement.TabOpen = true;
                            playerMovement.MenuOpen = true;

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
                    playerMovement.MenuOpen = false;

                    chestStorage.SetActive(false);

                    quickSlot.Reinitialize();

                    craftingCanvas.SetActive(false);

                    forgeCanvas.SetActive(false);

                    letters.SetActive(false);

                    tips.SetActive(false);
                    caveSelect.SetActive(false);
                    skills.SetActive(false);
                    help.SetActive(false);

                    SetCanOpenTabs(true);
                }
            }

            if (Joystick.current != null && Joystick.current.allControls[1].IsPressed() == true)
            {
                inventoryButtonPress = false;
            }
            if (Joystick.current != null && Joystick.current.allControls[4].IsPressed() == true)
            {
                questButtonPress = false;
            }
            if (Joystick.current != null && Joystick.current.allControls[2].IsPressed() == true)
            {
                skillsButtonPress = false;
            }
        }
        else
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                if (chestStorage.activeSelf == true)
                {
                    chestStorage.GetComponent<ChestStorageHandler>().ChestOpenHandler.CloseChest();
                }
                else if (craftingCanvas.activeSelf == true)
                {
                    craftingCanvas.GetComponent<CraftCanvasHandler>().Close();
                }
            }
        }
    }

    public bool CanOpenTab()
    {
        if (questShow.gameObject.activeSelf == false &&
                   playerInventory.activeSelf == false &&
                   chestStorage.activeSelf == false &&
                   craftingCanvas.activeSelf == false &&
                   forgeCanvas.activeSelf == false &&
                   caveSelect.activeSelf == false &&
                   skills.activeSelf == false &&
                   help.activeSelf == false &&
                   menuCanvas.activeSelf == false)
        {
            return true;
        }

        return false;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;

        playerMovement.TabOpen = false;
        playerMovement.MenuOpen = false;

        quickSlot.gameObject.SetActive(true);

        canOpenTabs = true;

        menuCanvas.SetActive(false);
    }

    public void SetCanOpenTabs(bool canOpenTabs)
    {
        this.canOpenTabs = canOpenTabs;
    }
}
