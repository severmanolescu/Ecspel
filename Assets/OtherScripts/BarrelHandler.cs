using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelHandler : MonoBehaviour
{
    [SerializeField] private List<ItemWithRandomAmount> items = new List<ItemWithRandomAmount>();

    [SerializeField] private float health;

    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip destroyClip;

    private SpawnItem spawnItem;

    private AudioSource audioSource;

    private void Awake()
    {
        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator WaitForSound()
    {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    public void GetDamage(float damage)
    {
        audioSource.clip = damageClip;
        audioSource.Play();

        health -= damage;

        if (health <= 0)
        {
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<SpriteRenderer>());

            audioSource.clip = destroyClip;
            audioSource.Play();

            foreach (ItemWithRandomAmount item in items)
            {
                spawnItem.SpawnItems(item.GetItem(), transform.position);
            }

            StartCoroutine(WaitForSound());
        }
    }
}
