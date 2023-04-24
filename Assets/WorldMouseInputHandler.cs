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

    private void CheckForObjectType(RaycastResult raycastResult)
    {
        ChestOpenHandler chestOpen = raycastResult.gameObject.GetComponent<ChestOpenHandler>();

        if (chestOpen != null)
        {
            chestOpen.OpenChest();
        }
        else
        {
            DialogueDisplay dialogueDisplay = raycastResult.gameObject.GetComponent<DialogueDisplay>();

            if (dialogueDisplay != null)
            {
                dialogueDisplay.ShowDialogue();
            }
            else
            {
                CraftingHandler craftingHandler = raycastResult.gameObject.GetComponent<CraftingHandler>();

                if (craftingHandler != null)
                {
                    craftingHandler.OpenCrafting();
                }
                else
                {
                    CampFireHandler campFireHandler = raycastResult.gameObject.GetComponent<CampFireHandler>();

                    if (campFireHandler != null)
                    {
                        campFireHandler.StartFire();
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame &&
            EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0 && canvasTabsOpen.canOpenTabs == true && playerMovement.CanMove && playerMovement.TabOpen == false && playerMovement.Dialogue == false)
            {
                if (Vector2.Distance(raycastResults[0].gameObject.transform.position, playerMovement.transform.position) <= maxDistanteFromPlayer)
                {
                    CheckForObjectType(raycastResults[0]);
                }
            }
        }
    }
}
