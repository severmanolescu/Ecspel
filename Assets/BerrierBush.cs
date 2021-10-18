using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BerrierBush : MonoBehaviour
{
    [SerializeField] private Item berrieType;
    [SerializeField] private int minAmount;
    [SerializeField] private int maxAmount;

    [SerializeField] private Sprite emptyBush;

    private Sprite fullBush;

    private GameObject prefab;

    private CanvasTabsOpen canvasTabs;

    bool player = false;

    bool ready = true;

    private void Awake()
    {
        prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Berries/Prefab/YellowRaspberry.prefab", typeof(GameObject));

        fullBush = GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if(player && ready && Input.GetKeyDown(KeyCode.E))
        {
            SpawnBerrie();

            GetComponent<SpriteRenderer>().sprite = emptyBush;

            ready = false;

            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);

        canvasTabs.SetCanOpenTabs(true);
    }

    private void SpawnBerrie()
    {
        ItemWorld itemWorld = Instantiate(prefab, transform.position, transform.rotation).GetComponent<ItemWorld>();

        itemWorld.transform.SetParent(transform);

        Item item = Instantiate(berrieType);
        item.Amount = UnityEngine.Random.Range(minAmount, maxAmount);

        itemWorld.SetItem(item);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (ready)
            {
                canvasTabs = collision.GetComponentInChildren<CanvasTabsOpen>();

                canvasTabs.SetCanOpenTabs(false);

                player = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ready)
            {
                canvasTabs.SetCanOpenTabs(true);

                player = false;
            }
        }
    }
}
