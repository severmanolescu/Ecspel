using System.Collections;
using UnityEngine;

public class CanvasTabsOpen : MonoBehaviour
{
    [Header("Player Inventory")]
    [SerializeField] private GameObject PlayerInventory;

    [Header("Player Quickslots")]
    [SerializeField] private GameObject quickSlot;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

        PlayerInventory.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.SetActive(!PlayerInventory.activeSelf);

            if(PlayerInventory.activeSelf == false)
            {
                quickSlot.GetComponent<QuickSlotsChanger>().Reinitialize();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerInventory.activeSelf)
            {
                PlayerInventory.SetActive(false);
                quickSlot.SetActive(true);
            }
        }
    }
}
