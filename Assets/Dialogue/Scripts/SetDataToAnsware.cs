using UnityEngine;
using UnityEngine.UI;

public class SetDataToAnsware : MonoBehaviour
{
    [SerializeField] private GameObject reward;
    [SerializeField] private Image rewardImage;

    private ItemSprites itemSprites;

    private void Awake()
    {
        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();
    }

    public void ChangeReward(GiveItem giveItem)
    {
        if (giveItem.ItemsNeeds != null && giveItem.ItemsNeeds.Count > 0)
        {
            reward.SetActive(true);

            if (itemSprites == null)
            {
                itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();
            }

            if (itemSprites != null)
            {
                rewardImage.sprite = giveItem.itemsNeeds[0].Item.ItemSprite;
            }
        }
        else
        {
            reward.SetActive(false);
        }
    }

    public void HideReward()
    {
        reward.SetActive(false);
    }
}
