using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDetails : MonoBehaviour
{
    [SerializeField] private GameObject playerItems;

    [SerializeField] private GameObject spawnLocation;
    [SerializeField] private GameObject statPrefab;
    [SerializeField] private Sprite coinSprite;
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite powerSprite;
    [SerializeField] private Sprite heartSprite;
    [SerializeField] private Sprite lightningSprite;

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

    public void SetItem(Item item, int price = -1)
    {
        if (item != null)
        {
            itemSpriteObject.sprite = itemSprites.GetItemSprite(item.ItemNO);
            itemNameObject.text = item.Name;
            itemDetailsObject.text = "    " + item.Details;

            StatDataSet[] oldStats = spawnLocation.GetComponentsInChildren<StatDataSet>();
            
            foreach(StatDataSet stat in oldStats)
            {
                Destroy(stat.gameObject);
            }

            SetDataToStats(item, price);

            gameObject.SetActive(true);
        }
    }

    private void SetDataToStats(Item item, int price)
    {
        GameObject instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

        if (price == -1)
        {
            instantiateStat.GetComponent<StatDataSet>().SetData(coinSprite, item.SellPrice.ToString());
        }
        else
        {
            instantiateStat.GetComponent<StatDataSet>().SetData(coinSprite, price.ToString());
        }

        if(item is Weapon)
        {
            Weapon weapon = (Weapon)item;

            instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

            instantiateStat.GetComponent<StatDataSet>().SetData(attackSprite, weapon.AttackPower.ToString());
        }
        else if(item is Pickaxe)
        {
            Pickaxe pickaxe = (Pickaxe)item;

            instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

            instantiateStat.GetComponent<StatDataSet>().SetData(attackSprite, pickaxe.Damage.ToString());

            instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

            instantiateStat.GetComponent<StatDataSet>().SetData(powerSprite, pickaxe.Level.ToString());
        }
        else if(item is Axe)
        {
            Axe axe = (Axe)item;

            instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

            instantiateStat.GetComponent<StatDataSet>().SetData(attackSprite, axe.Damage.ToString());

            instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

            instantiateStat.GetComponent<StatDataSet>().SetData(powerSprite, axe.Level.ToString());
        }
        else if(item is Consumable)
        {
            Consumable consumable = (Consumable)item;

            if (consumable.Health > 0)
            {
                instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

                instantiateStat.GetComponent<StatDataSet>().SetData(heartSprite, consumable.Health.ToString());
            }

            if (consumable.Stamina > 0)
            {
                instantiateStat = Instantiate(statPrefab, spawnLocation.transform);

                instantiateStat.GetComponent<StatDataSet>().SetData(lightningSprite, consumable.Stamina.ToString());
            }
        }
    }

    public void HideData()
    {
        gameObject.SetActive(false);
    }

}
