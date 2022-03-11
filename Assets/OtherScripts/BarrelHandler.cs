using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelHandler : MonoBehaviour
{
    [SerializeField] private List<ItemWithRandomAmount> items = new List<ItemWithRandomAmount>();

    [SerializeField] private GameObject itemWorld;

    [SerializeField] private float health;

    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip destroyClip;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator WaitForSound()
    {
        while(audioSource.isPlaying)
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

        Debug.Log(damage);

        if(health <= 0)
        {
            Destroy(GetComponent<BoxCollider2D>());
            Destroy(GetComponent<SpriteRenderer>());

            audioSource.clip = destroyClip;
            audioSource.Play();

            foreach (ItemWithRandomAmount item in items)
            {
                ItemWorld newItem = Instantiate(itemWorld).GetComponent<ItemWorld>();

                newItem.SetItem(item.GetItem());

                newItem.transform.position = transform.position;

                newItem.MoveToPoint();
            }

            StartCoroutine(WaitForSound());
        }
    }
}
