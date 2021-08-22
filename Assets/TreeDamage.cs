using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeDamage : MonoBehaviour
{
    [SerializeField] private GameObject stickPrefab;
    [SerializeField] private GameObject prefabSpawnLocation;

    private Animator animator;
    private ParticleStart particleStart;

    private int health;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        particleStart = GetComponentInChildren<ParticleStart>();

        health = DefaulData.treeHealth;
    }

    public void TakeDamage(int damage, int spawn)
    {
        health -= damage;

        if (health <= 0)
        {
            if (particleStart != null)
            {
                particleStart.SetSpawn(spawn);
                animator.SetBool("Fall", true);
            }
        }
        else
        {
            float random = Random.Range(0, 100);

            if(random >= 0)
            {
                GameObject stick = Instantiate(stickPrefab, prefabSpawnLocation.transform.position, prefabSpawnLocation.transform.rotation);

                int range = Random.Range(1, 3);

                stick.GetComponent<Animator>().SetInteger("Spawn", range);
                stick.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.stick, 1));
            }
        }
    }
}
