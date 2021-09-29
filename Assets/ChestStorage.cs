using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestStorage : MonoBehaviour
{
    [SerializeField] private ChestStorageCanvas chestStorage;

    private CanvasTabsOpen canvasTabs;

    public List<Item> items = new List<Item>();

    private TextMeshProUGUI text;

    public List<Item> Items { get { return items; } }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.gameObject.SetActive(false);

        canvasTabs = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
    }

    private void Start()
    { 
        items.Add(DefaulData.GetItemWithAmount(DefaulData.log, 10));
        items.Add(DefaulData.GetItemWithAmount(DefaulData.pickaxe, 1));
        items.Add(DefaulData.GetItemWithAmount(DefaulData.stone, 10));
        items.Add(DefaulData.GetItemWithAmount(DefaulData.stoneAxe, 1));
    }

    public void SetItems(List<Item> items)
    {
        this.items = items;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);

            canvasTabs.SetChestItems(items, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);

            canvasTabs.DeleteChestItems();
        }
    }
}
