using UnityEngine;

public class CraftingHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private CraftCanvasHandler craftCanvas;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        craftCanvas = GameObject.Find("Player/Canvas/Crafting").GetComponent<CraftCanvasHandler>();

        quickSlots = GameObject.Find("Player/Canvas/QuickSlots");
    }

    public void OpenCrafting()
    {
        playerMovement.TabOpen = true;
        playerInventory.SetActive(true);

        craftCanvas.gameObject.SetActive(true);
        craftCanvas.ReinitializeAllCraftings();

        quickSlots.SetActive(false);

        canvasTabsOpen.SetCanOpenTabs(false);

        craftCanvas.CraftingHandler = this;
    }

    public void CloseCraft()
    {
        playerMovement.TabOpen = false;
        playerInventory.SetActive(false);

        craftCanvas.ShowAllCrafts();
        craftCanvas.gameObject.SetActive(false);

        quickSlots.SetActive(true);

        canvasTabsOpen.SetCanOpenTabs(true);
    }
}
