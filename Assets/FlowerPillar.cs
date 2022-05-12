using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlowerPillar : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private Color notActtiveColor;

    private new Light2D light;

    private SpriteRenderer[] sprites;

    private SpriteRenderer itemSprite;

    private TextMeshProUGUI buttonPressText;

    private bool playerInSpace = false;

    private bool itemPlaced = false;

    private bool fKeyPress = true;

    private bool deletedItem = false;

    private SpecialFlowerHandler specialFlowerHandler;

    private PlayerInventory playerInventory;

    private WorldTextDetails worldTextDetails;

    private AudioSource audioSource;

    public SpecialFlowerHandler SpecialFlowerHandler { get => specialFlowerHandler; set => specialFlowerHandler = value; }
    public bool ItemPlaced { get => itemPlaced; set => itemPlaced = value; }

    private void Awake()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();

        itemSprite = sprites[sprites.Length - 1];

        buttonPressText = GetComponentInChildren<TextMeshProUGUI>();

        light = GetComponentInChildren<Light2D>();

        buttonPressText.gameObject.SetActive(false);
        itemSprite.gameObject.SetActive(false);
        light.gameObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();
        worldTextDetails = GameObject.Find("Global/Player/Canvas/WorldTextDetails").GetComponent<WorldTextDetails>();

        ChangeSpritesColor(notActtiveColor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && itemPlaced == false && deletedItem == false)
        {
            playerInSpace = true;

            buttonPressText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSpace = false;

            buttonPressText.gameObject.SetActive(false);
        }
    }

    private void ChangeSpritesColor(Color color)
    {
        for (int indexOfSprite = 0; indexOfSprite < sprites.Length - 1; indexOfSprite++)
        {
            sprites[indexOfSprite].color = color;
        }
    }

    private void Update()
    {
        if(playerInSpace == true && itemPlaced == false && deletedItem == false)
        {
            if((Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame) || 
               (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                if(playerInventory.SearchInventory(item, 1))
                {
                    Item itemCopy = item.Copy();
                    itemCopy.Amount = 1;

                    playerInventory.DeleteItem(itemCopy);

                    specialFlowerHandler.PlacedFlowers += 1;

                    audioSource.Play();

                    PlaceItem();
                }
                else
                {
                    worldTextDetails.ShowText("Nu detii obiectul necesar!");
                }
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }

    private void PlaceItem()
    {
        itemSprite.gameObject.SetActive(true);
        light.gameObject.SetActive(true);

        buttonPressText.gameObject.SetActive(false);

        itemPlaced = true;

        ChangeSpritesColor(Color.white);
    }

    private void ResetAll()
    {
        ChangeSpritesColor(notActtiveColor);

        buttonPressText.gameObject.SetActive(false);

        itemSprite.gameObject.SetActive(false);
        light.gameObject.SetActive(false);
    }

    public void ChangeStateOfPillar(bool itemPlaced, bool deletedItem)
    {
        this.deletedItem = deletedItem;
        this.itemPlaced = itemPlaced;

        if(deletedItem == true)
        {
            itemSprite.gameObject.SetActive(false);
            light.gameObject.SetActive(false);

            ChangeSpritesColor(notActtiveColor);
        }
        else if(itemPlaced == true)
        {
            PlaceItem();
        }
        else
        {
            ResetAll();
        }
    }
}
