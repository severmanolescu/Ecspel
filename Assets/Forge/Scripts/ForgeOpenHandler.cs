using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class ForgeOpenHandler : MonoBehaviour
{
    [SerializeField] private Sprite workingSprite;

    [SerializeField] private ParticleSystem smokeParticle;
    [SerializeField] private ParticleSystem fireParticle;

    [SerializeField] private ItemWorld itemSlotPrefab;

    [SerializeField] private int forgeID;

    private new Light2D light;

    private Item inputItem;

    private Item fuelItem;

    private Item outputItem;

    private bool canvasOpen = false;

    private bool fuelConsumeCoroutineStart = false;
    private bool smeltingConsumeCoroutineStart = false;

    private bool workingForge = false;

    private Sprite notWorkingForge;

    private GameObject player = null;

    private TextMeshProUGUI text;

    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private ForgeHandler forgeHandler;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private Keyboard keyboard;

    private AudioSource audioSource;

    private int elapsedTimeSmelting;
    private int elapsedTimeFuel;

    public Item InputItem { get => inputItem; set { inputItem = value; ItemsChange(); } }
    public Item FuelItem { get => fuelItem; set { fuelItem = value; ItemsChange(); } }
    public Item OutputItem { get => outputItem; set { outputItem= value; ItemsChange(); } }

    public int ForgeID { get => forgeID; set => forgeID = value; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        light = GetComponentInChildren<Light2D>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        forgeHandler = GameObject.Find("Player/Canvas/Forge").GetComponent<ForgeHandler>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");

        notWorkingForge = GetComponent<SpriteRenderer>().sprite;

        audioSource = GetComponent<AudioSource>();

        smokeParticle.Stop();
        fireParticle.Stop();
        audioSource.Stop();
        light.enabled = false;

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;

            text.gameObject.SetActive(false);

            playerMovement.TabOpen = false;
            playerInventory.SetActive(false);
                        
            forgeHandler.HideDataAtClose();
            forgeHandler.gameObject.SetActive(false);

            canvasTabsOpen.SetCanOpenTabs(true);

            quickSlots.SetActive(true);

            canvasOpen = false;
        }
    }

    public Item GetFuelItem()
    {
        if (OutputItem != null)
        {
            Item newItem = OutputItem.Copy();

            if (workingForge == true)
            {
                newItem.Amount++;
            }
            return newItem;
        }
        return null;
    
    }

    private IEnumerator FuelConsume()
    {
        if (fuelItem != null && fuelItem.Amount > 0 &&
            fuelItem is Fuel &&
            inputItem != null && inputItem.Amount > 0)
        {
            elapsedTimeFuel = 0;

            Fuel fuel = (Fuel)fuelItem;

            workingForge = true;

            fuelItem.Amount--;

            if (canvasOpen == true)
            {
                forgeHandler.ChangeFuelItem(fuelItem);

                forgeHandler.SetValuetoFuelSlider(1f);
            }

            while (elapsedTimeFuel < fuel.Duration)
            {
                yield return new WaitForSeconds(1);

                elapsedTimeFuel++;

                if (canvasOpen == true)
                {
                    forgeHandler.SetValuetoFuelSlider(1f - (float)elapsedTimeFuel / fuel.Duration);
                }
            }

            StartCoroutine(FuelConsume());
        }
        else
        {
            if (canvasOpen == true)
            {
                forgeHandler.HideFuelProgress();
            }

            if(fuelItem != null && fuelItem.Amount == 0)
            {
                fuelItem = null;
            }

            fuelConsumeCoroutineStart = false;

            StopForge();

            StopAllCoroutines();
        }
    }

    private IEnumerator Smelting()
    {
        elapsedTimeSmelting = 0;

        Smelting smelting = (Smelting)inputItem;

        if (smelting != null &&
            ((fuelItem != null && fuelItem.Amount > 0) || workingForge == true) &&
            inputItem != null && inputItem.Amount > 0 &&
            ((outputItem != null && outputItem.name == smelting.NextItem.name && outputItem.Amount < outputItem.MaxAmount) || 
            outputItem == null ) )
        {
            if (canvasOpen == true)
            {
                forgeHandler.SetValuetoForgeSlider(0f);
            }

            while (elapsedTimeSmelting < smelting.Duration && inputItem != null)
            {
                yield return new WaitForSeconds(1);

                elapsedTimeSmelting++;

                if (canvasOpen == true)
                {
                    forgeHandler.SetValuetoForgeSlider((float)elapsedTimeSmelting / smelting.Duration);
                }
            }

            if (inputItem == null)
            {
                if (canvasOpen == true)
                {
                    forgeHandler.HideForgeProgress();
                }

                StopCoroutine(FuelConsume());

                smeltingConsumeCoroutineStart = false;
            }
            else
            {
                inputItem.Amount--;

                if (outputItem == null)
                {
                    outputItem = smelting.NextItem.Copy();

                    outputItem.Amount = 1;
                }
                else
                {
                    outputItem.Amount++;
                }

                if (inputItem.Amount <= 0)
                {
                    inputItem = null;
                }

                if (canvasOpen == true)
                {
                    forgeHandler.ChangeInputItem(inputItem);
                    forgeHandler.ChangeOutputItem(outputItem);
                }


                StartCoroutine(Smelting());
            }
        }
        else
        {
            if (canvasOpen == true)
            {
                forgeHandler.HideForgeProgress();
            }

            if (inputItem != null && inputItem.Amount == 0)
            {
                inputItem = null;
            }

            StopCoroutine(Smelting());

            smeltingConsumeCoroutineStart = false;
        }
    }

    private void StartForge()
    {
        GetComponent<SpriteRenderer>().sprite = workingSprite;

        workingForge = true;

        var main = fireParticle.main;

        main.loop = true;

        main = smokeParticle.main;

        main.loop = true;

        fireParticle.Play();
        smokeParticle.Play();
        audioSource.Play();

        light.enabled = true;

        if (fuelConsumeCoroutineStart == false)
        {
            fuelConsumeCoroutineStart = true;

            StartCoroutine(FuelConsume());
        }

        if (smeltingConsumeCoroutineStart == false)
        {
            smeltingConsumeCoroutineStart = true;

            StartCoroutine(Smelting());
        }
    }

    public void StopForge()
    {
        workingForge = false;

        var main = fireParticle.main;

        main.loop = false;

        main = smokeParticle.main;

        main.loop = false;

        audioSource.Stop();

        light.enabled = false;

        DeleteDataFromCanvas();

        fuelConsumeCoroutineStart = false;

        smeltingConsumeCoroutineStart = false;

        GetComponent<SpriteRenderer>().sprite = notWorkingForge;
    }

    private void DeleteDataFromCanvas()
    {
        if (canvasOpen == true)
        {
            forgeHandler.SetValuetoForgeSlider(0f);
            forgeHandler.SetValuetoFuelSlider(1f);

            forgeHandler.HideForgeProgress();
            forgeHandler.HideFuelProgress();
        }
    }

    public void ItemsChange()
    {
        if(inputItem != null && inputItem.Amount > 0)
        {
            if(fuelItem != null && fuelItem.Amount > 0 && fuelItem is Fuel)
            {
                if(inputItem is Smelting)
                {
                    Smelting smelting = (Smelting)inputItem;

                    if(outputItem != null && 
                       outputItem.name.CompareTo(smelting.NextItem.name) == 0 &&
                       outputItem.Amount <= outputItem.MaxAmount)
                    {
                        StartForge();
                    }
                    else if(outputItem == null)
                    {
                        StartForge();
                    }
                }
            }
        }
        else
        {
            StopCoroutine(Smelting());

            if(canvasOpen == true)
            {
                forgeHandler.HideForgeProgress();
            }
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (keyboard.fKey.wasPressedThisFrame)
            {
                if (playerInventory.activeSelf == false)
                {
                    playerMovement.TabOpen = true;
                    playerInventory.SetActive(true);
                    canvasTabsOpen.SetCanOpenTabs(false);

                    float smeltingProgress = 0;
                    float fuelProgress = 0;

                    if(inputItem != null && inputItem is Smelting)
                    {
                        Smelting smelting = (Smelting)(inputItem);

                        smeltingProgress = (float)elapsedTimeSmelting / smelting.Duration;
                    }
                    if(fuelItem != null && fuelItem is Fuel)
                    {
                        Fuel smelting = (Fuel)(fuelItem);

                        fuelProgress = 1f -  (float)elapsedTimeFuel / smelting.Duration;
                    }

                    forgeHandler.SetDataAtOpen(InputItem, FuelItem, OutputItem, this, smeltingProgress, fuelProgress, workingForge);

                    quickSlots.SetActive(false);

                    canvasOpen = true;

                    forgeHandler.gameObject.SetActive(true);
                }
                else
                {
                    playerMovement.TabOpen = false;
                    playerInventory.SetActive(false);
                    canvasTabsOpen.SetCanOpenTabs(true);

                    forgeHandler.GetItems(out inputItem, out fuelItem, out outputItem);

                    forgeHandler.HideDataAtClose();
                    forgeHandler.gameObject.SetActive(false);

                    quickSlots.SetActive(true);

                    quickSlots.GetComponent<QuickSlotsChanger>().Reinitialize();

                    DeleteDataFromCanvas();

                    canvasOpen = false;
                }
            }
        }
    }

    private void InstantiateItemWorld(Item item)
    {
        if (item != null)
        {
            ItemWorld instantiateItem = Instantiate(itemSlotPrefab).GetComponent<ItemWorld>();

            instantiateItem.transform.position = transform.position;

            instantiateItem.SetItem(item, false);

            instantiateItem.MoveToPoint();
        }
    }

    public void DropAllItems()
    {
        StopForge();

        InstantiateItemWorld(inputItem);
        InstantiateItemWorld(outputItem);
        InstantiateItemWorld(fuelItem);

        Destroy(gameObject);
    }
}
