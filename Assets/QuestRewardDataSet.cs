using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardDataSet : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;

    public void SetData(ItemWithAmount item)
    {
        if (item != null && item.Item != null)
        {
            itemImage.sprite = item.Item.ItemSprite;
            itemAmount.text = item.Amount.ToString();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
