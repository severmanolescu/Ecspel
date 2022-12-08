using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpecialFlowerHandler : MonoBehaviour
{
    [Header("Table SprteRenderer for change color")]
    [SerializeField] private SpriteRenderer tableSprite;

    [Header("Not active color")]
    [SerializeField] private Color notActiveColor;

    [Header("Flower pillars")]
    [SerializeField] private List<FlowerPillar> flowerPillar = new();

    [Header("Final item")]
    [SerializeField] private GameObject finalItem;

    [Header("Receive item")]
    [SerializeField] private Item item;

    [Header("Light")]
    [SerializeField] private new Light2D light;

    [Header("Sound effects")]
    [SerializeField] private AudioClip startClip;
    [SerializeField] private AudioClip stopClip;

    private AudioSource audioSource;

    private PlayerInventory playerInventory;

    private bool collectedItem = false;

    private int placedFlowers = 0;

    private int maximumOfFlowers;

    public int PlacedFlowers { get => placedFlowers; set { placedFlowers = value; FlowerCounterChange(); } }

    public bool CollectedItem { get => collectedItem; set => collectedItem = value; }

    private void Awake()
    {
        foreach (var flower in flowerPillar)
        {
            flower.SpecialFlowerHandler = this;
        }

        finalItem.SetActive(false);
        light.gameObject.SetActive(false);

        ChangeTableColor(notActiveColor);

        item = item.Copy();
        item.Amount = 1;

        maximumOfFlowers = flowerPillar.Count - 1;

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        audioSource = GetComponent<AudioSource>();
    }

    private void ChangeTableColor(Color color)
    {
        tableSprite.color = color;
    }

    private void FlowerCounterChange()
    {
        if (placedFlowers > maximumOfFlowers && collectedItem == false)
        {
            ChangeTableColor(Color.white);

            finalItem.SetActive(true);
            light.gameObject.SetActive(true);

            audioSource.clip = startClip;
            audioSource.Play();
        }
    }

    public List<bool> GetFlowerStatus()
    {
        List<bool> status = new List<bool>();

        foreach (var flower in flowerPillar)
        {
            status.Add(flower.ItemPlaced);
        }

        return status;
    }

    public void SetFlowersStatus(List<bool> status, bool collectedItem)
    {
        placedFlowers = 0;

        for (int indexOfFlower = 0; indexOfFlower < status.Count; indexOfFlower++)
        {
            flowerPillar[indexOfFlower].ChangeStateOfPillar(status[indexOfFlower], collectedItem);

            if (status[indexOfFlower] == true)
            {
                placedFlowers++;
            }
        }

        this.collectedItem = collectedItem;

        if (collectedItem == false && placedFlowers > maximumOfFlowers)
        {
            ChangeTableColor(Color.white);

            finalItem.SetActive(true);
            light.gameObject.SetActive(true);
        }
    }

    private void DeletePillarFlower()
    {
        foreach (var flower in flowerPillar)
        {
            flower.ChangeStateOfPillar(true, true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            if (collectedItem == false)
            {
                if (placedFlowers > maximumOfFlowers)
                {
                    if (playerInventory.AddItem(item) == true)
                    {
                        DeletePillarFlower();

                        collectedItem = true;

                        ChangeTableColor(notActiveColor);

                        finalItem.SetActive(false);
                        light.gameObject.SetActive(false);

                        audioSource.clip = stopClip;
                        audioSource.Play();
                    }
                }
            }
        }
    }
}
