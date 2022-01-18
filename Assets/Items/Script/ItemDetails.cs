using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private GameObject playerItems;

    private ItemSprites itemSprites;

    private Image itemSpriteObject;

    private TextMeshProUGUI itemNameObject;
    private TextMeshProUGUI itemDetailsObject;

    private void Awake()
    {
        gameObject.SetActive(false);

        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        Image[] auxiliarObjectImage = gameObject.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] auxiliarObjectText = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        itemSpriteObject = auxiliarObjectImage[1];

        itemNameObject = auxiliarObjectText[0];
        itemDetailsObject = auxiliarObjectText[1];
    }

    private void Update()
    {
        if (playerItems.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetItem(Item item)
    {
        if (item != null)
        {
            itemSpriteObject.sprite = itemSprites.GetItemSprite(item.ItemNO);
            itemNameObject.text = item.Name;
            itemDetailsObject.text = item.Details;

            gameObject.SetActive(true);
        }
    }

    public void HideData()
    {
        gameObject.SetActive(false);
    }

}
