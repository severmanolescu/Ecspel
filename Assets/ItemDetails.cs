using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    private Image itemSpriteObject;

    private TextMeshProUGUI itemNameObject;
    private TextMeshProUGUI itemDetailsObject;

    private void Start()
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
            itemSpriteObject.sprite = item.GetSprite();
            itemNameObject.text = item.GetName();
            itemDetailsObject.text = item.GetDetails();

            gameObject.SetActive(true);
        }
    }

    public void HideData()
    {
        gameObject.SetActive(false);
    }

}
