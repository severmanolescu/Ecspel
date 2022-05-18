using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class LastCaveHandler : MonoBehaviour
{
    [SerializeField] private Item needItem;

    [SerializeField] private Transform firstCaveLocation;
    [SerializeField] private Transform secondCaveLocation;

    [SerializeField] private GameObject firstCaveCamera;
    [SerializeField] private GameObject secondCaveCamera;

    [SerializeField] private Transform playerLocation;

    [SerializeField] private QuickSlotsChanger quickSlots;

    [SerializeField] private TransferImageHandler transferImageHandler;

    [SerializeField] private PlayerInventory playerInventory;

    private bool leftMousePress = true;

    private float distanceBetweenCaves;

    private bool player = false;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        distanceBetweenCaves = firstCaveLocation.position.x - secondCaveLocation.position.x;

        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Player"))
        {
            player = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            player = false;
        }
    }

    private IEnumerator WaitForBack()
    {
        playerMovement.TabOpen = true;

        transferImageHandler.StartTransfer();

        yield return new WaitForSeconds(2f);

        Item deleteItem = needItem.Copy();
        deleteItem.Amount = 1;

        playerInventory.DeleteItem(deleteItem);

        playerLocation.position = new Vector3(playerLocation.position.x - distanceBetweenCaves, playerLocation.position.y, playerLocation.position.z);

        firstCaveCamera.SetActive(false);
        secondCaveCamera.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        transferImageHandler.HideImage();

        playerMovement.TabOpen = false;
    }

    void Update()
    {
        if(player == true)
        {
            if (playerMovement.Speed == 0 && playerMovement.CanMove == true && playerMovement.TabOpen == false)
            {
                if ((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
                    (Joystick.current != null && Joystick.current.allControls[5].IsPressed() == false && leftMousePress == false))
                {
                    if (quickSlots.Item.ItemNO == needItem.ItemNO)
                    {
                        StartCoroutine(WaitForBack());
                    }
                }

                if (Joystick.current != null && Joystick.current.allControls[5].IsPressed() == true)
                {
                    leftMousePress = false;
                }
            }
        }
    }
}
