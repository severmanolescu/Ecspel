using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private GameObject playerItems;

    private Image itemSpriteObject;

    private TextMeshProUGUI itemNameObject;
    private TextMeshProUGUI itemDetailsObject;

    private void Update()
    {
        if(playerItems.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        gameObject.SetActive(false);

        Image[] auxiliarObjectImage = gameObject.GetComponentsInChildren<Image>();
        TextMeshProUGUI[] auxiliarObjectText = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        itemSpriteObject = auxiliarObjectImage[1];

        itemNameObject = auxiliarObjectText[0];
        itemDetailsObject = auxiliarObjectText[1];
    }

    public void SetItem(Item item)
    {
        if (item != null)
        {
            itemSpriteObject.sprite = item.Sprite;
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
