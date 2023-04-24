using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpenHandler : MonoBehaviour
{
    [SerializeField] private Sprite openSprite;
    private Sprite closeSprite;

    [SerializeField] private int chestId;

    [Header("Audio effects")]
    [SerializeField] private AudioClip chestOpen;
    [SerializeField] private AudioClip chestClose;

    private AudioSource audioSource;

    private GameObject playerInventory;
    private GameObject quickSlots;

    private SpriteRenderer spriteRenderer;

    private ChestStorage chestStorage;

    private PlayerMovement playerMovement;

    private ChestStorageHandler chestStorageHandler;

    private CanvasTabsOpen canvasTabsOpen;

    private SpawnItem spawnItem;

    private List<Item> items = new List<Item>();

    public int ChestId { get => chestId; set => chestId = value; }

    private void Awake()
    {
        chestStorage = GetComponent<ChestStorage>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        chestStorageHandler = GameObject.Find("Player/Canvas/ChestStorage").GetComponent<ChestStorageHandler>();

        quickSlots = GameObject.Find("Player/Canvas/QuickSlots");

        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        closeSprite = spriteRenderer.sprite;

        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();
    }

    private IEnumerator WaitToNextFrame()
    {
        yield return new WaitForEndOfFrame();

        canvasTabsOpen.SetCanOpenTabs(true);
    }

    public void DropAllItems()
    {
        items = chestStorage.Items;

        foreach (Item item in items)
        {
            if (item != null)
            {
                spawnItem.SpawnItems(item, transform.position);
            }
        }
    }

    public void OpenChest()
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
        chestStorage.RemoveAllItems();
    }
}
