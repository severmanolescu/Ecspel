using System.Collections;
using UnityEngine;

public class CollectHarvest : MonoBehaviour
{
    [SerializeField] private Item item;

    private AudioSource audioSource;

    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator WaitForSound()
    {
        Destroy(GetComponent<SpriteRenderer>());

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    public void HarvestItem()
    {
        Item newItem = item.Copy();

        newItem.Amount = 1;

        if (inventory.AddItemWithAnimation(newItem) == 0)
        {
            audioSource.Play();

            StartCoroutine(WaitForSound());
        }
    }
}
