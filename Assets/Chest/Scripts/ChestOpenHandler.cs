using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestOpenHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemWorld;
    [SerializeField] private Sprite openSprite;
    private Sprite closeSprite;

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

                newItemWorld.SetItem(newItem);

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

        canvasTabsOpen.SetCanOpenTabs(true);

        quickSlots.SetActive(true);

        quickSlots.GetComponent<QuickSlotsChanger>().Reinitialize();
    }

    private void Update()
    {
        if (player != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (GameObject.Find("Player/Canvas/PlayerItems").activeSelf == false)
                {
                    OpenChest();
                }
                else
                {
                    CloseChestKeyPress();
                }
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
}
