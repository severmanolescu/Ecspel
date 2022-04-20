using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ChestOpenHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemWorld;
    [SerializeField] private Sprite openSprite;
    private Sprite closeSprite;

    [SerializeField] private int chestId;

    [Header("Audio effects")]
    [SerializeField] private AudioClip chestOpen;
    [SerializeField] private AudioClip chestClose;

    private AudioSource audioSource;

    private GameObject player = null;
    private GameObject playerInventory;
    private GameObject quickSlots;

    private SpriteRenderer spriteRenderer;

    private TextMeshProUGUI text;

    private ChestStorage chestStorage;

    private PlayerMovement playerMovement;

    private ChestStorageHandler chestStorageHandler;

    private CanvasTabsOpen canvasTabsOpen;

    private List<Item> items = new List<Item>();

    private Keyboard keyboard;

    private bool fKeyPress = true;

    public int ChestId { get => chestId; set => chestId = value; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        chestStorage = GetComponent<ChestStorage>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        chestStorageHandler = GameObject.Find("Player/Canvas/ChestStorage").GetComponent<ChestStorageHandler>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        closeSprite = spriteRenderer.sprite;

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = null;

            text.gameObject.SetActive(false);

            playerMovement.TabOpen = false;
            playerInventory.SetActive(false);
            chestStorageHandler.gameObject.SetActive(false);
            chestStorageHandler.SetChestStorage(chestStorage.Items, chestStorage.ChestMaxSlots, this); 

            canvasTabsOpen.SetCanOpenTabs(true);

            quickSlots.SetActive(true);
        }
    }

    private IEnumerator WaitToNextFrame()
    {
        yield return new WaitForEndOfFrame();

        canvasTabsOpen.SetCanOpenTabs(true);
    }

    public void DropAllItems()
    {
        items = chestStorage.Items;

        foreach(Item item in items)
        {
            if (item != null)
            {
                ItemWorld newItemWorld = Instantiate(itemWorld).GetComponent<ItemWorld>();

                newItemWorld.transform.position = transform.position;

                Item newItem = item.Copy();

                newItemWorld.SetItem(newItem, false);

                newItemWorld.MoveToPoint();
            }
        }
    }

    private void OpenChest()
    {
        audioSource.clip = chestOpen;
        audioSource.Play();

        spriteRenderer.sprite = openSprite;

        playerMovement.TabOpen = true;
        playerInventory.SetActive(true);
        chestStorageHandler.gameObject.SetActive(true);
        chestStorageHandler.SetChestStorage(chestStorage.Items, chestStorage.ChestMaxSlots, this);

        canvasTabsOpen.SetCanOpenTabs(false);

        quickSlots.SetActive(false);
    }

    private void CloseChestKeyPress()
    {
        audioSource.clip = chestClose;
        audioSource.Play();

        spriteRenderer.sprite = closeSprite;

        playerMovement.TabOpen = false;
        playerInventory.SetActive(false);
        chestStorageHandler.gameObject.SetActive(false);

        chestStorage.SetItems(GameObject.Find("Player/Canvas/ChestStorage").GetComponent<ChestStorageHandler>().GetChestStorage());

        StopAllCoroutines();
        StartCoroutine(WaitToNextFrame());

        quickSlots.SetActive(true);

        quickSlots.GetComponent<QuickSlotsChanger>().Reinitialize();
    }

    private void Update()
    {
        if (player != null)
        {
            if (keyboard.fKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                fKeyPress = true;

                if (GameObject.Find("Player/Canvas/PlayerItems").activeSelf == false)
                {
                    OpenChest();
                }
                else
                {
                    CloseChestKeyPress();
                }
            }
            else if (keyboard.fKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                fKeyPress = true;

                CloseChestKeyPress();
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }

    public void CloseChest()
    {
        audioSource.clip = chestClose;
        audioSource.Play();

        spriteRenderer.sprite = closeSprite;

        playerMovement.TabOpen = false;
        playerInventory.SetActive(false);
        chestStorageHandler.gameObject.SetActive(false);

        chestStorage.SetItems(GameObject.Find("Player/Canvas/ChestStorage").GetComponent<ChestStorageHandler>().GetChestStorage());

        canvasTabsOpen.SetCanOpenTabs(true);

        quickSlots.SetActive(true);

        quickSlots.GetComponent<QuickSlotsChanger>().Reinitialize();
    }

    public void DeleteAllItems()
    {
        Debug.Log("delete all");

        chestStorage.RemoveAllItems();
    }
}
