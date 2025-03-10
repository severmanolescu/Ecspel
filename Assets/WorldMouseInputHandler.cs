using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class WorldMouseInputHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float maxDistanteFromPlayer = 3f;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen canvasTabsOpen;

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
        canvasTabsOpen = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();
    }

    private bool CheckForObjectType(RaycastResult raycastResult)
    {
        ChestOpenHandler chestOpen = raycastResult.gameObject.GetComponent<ChestOpenHandler>();

        if (chestOpen != null)
        {
            chestOpen.OpenChest();

            return true;
        }
        else
        {
            CollectItem collectItem = raycastResult.gameObject.GetComponent<CollectItem>();

            if(collectItem != null)
            {
                collectItem.AddItemToInventory();

                return true;
            }
            else
            {
                DialogueDisplay dialogueDisplay = raycastResult.gameObject.GetComponent<DialogueDisplay>();

                if (dialogueDisplay != null)
                {
                    dialogueDisplay.ShowDialogue();

                    return true;
                }
                else
                {
                    CraftingHandler craftingHandler = raycastResult.gameObject.GetComponent<CraftingHandler>();

                    if (craftingHandler != null)
                    {
                        craftingHandler.OpenCrafting();

                        return true;
                    }
                    else
                    {
                        CampFireHandler campFireHandler = raycastResult.gameObject.GetComponent<CampFireHandler>();

                        if (campFireHandler != null)
                        {
                            campFireHandler.StartFire();

                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    private void Update()
    {
        if(Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

                PointerEventData pointer = new PointerEventData(EventSystem.current);
                pointer.position = mousePosition;

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointer, raycastResults);

                if (raycastResults.Count > 0 && canvasTabsOpen.canOpenTabs == true && playerMovement.CanMove && playerMovement.TabOpen == false && playerMovement.Dialogue == false)
                {
                    foreach (RaycastResult raycastResult in raycastResults)
                    {
                        if (Vector2.Distance(raycastResult.gameObject.transform.position, playerMovement.transform.position) <= maxDistanteFromPlayer)
                        {
                            if(CheckForObjectType(raycastResult))
                            {
                                break;
                            }
                        }
                    }                    
                }
            }
        }
    }
}
