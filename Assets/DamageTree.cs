using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTree : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private int treeLevel;
    [SerializeField] private int trunkLevel;

    private GameObject prefabLog;

    private Animator animator;

    private DestroyTree destroyTree;

    private bool destroyed = false;

    private void Awake()
    {
        destroyTree = GetComponentInChildren<DestroyTree>();

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        prefabLog = destroyTree.ItemWorld;
    }

    public void TakeDamage(float damage, int spawn, int itemLevel)
    {
        if(destroyed == false)
        {
            if(itemLevel >= treeLevel)
            {
                health -= damage;
            }
        }
        else 
        {
            if (itemLevel >= trunkLevel)
            {
                health -= damage;
            }
        }

        if (health <= 0)
        {
            if (destroyed == false)
            {
                destroyTree.Spawn = spawn;

                switch (spawn)
                {
                    case 1: animator.SetTrigger("Left"); break;
                    case 2: animator.SetTrigger("Right"); break;
                }

                Vector3 newScale = transform.Find("Shadow").localScale;

                newScale.y /= 2;

                transform.Find("Shadow").localScale = newScale;

                health = 10;

                destroyed = true;
            }
            else
            {
                ItemWorld game = Instantiate(prefabLog).GetComponent<ItemWorld>();

                game.transform.position = transform.position;

                game.SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 2));
                game.MoveToPoint();

                Destroy(this.gameObject);

            }
        }
    }
}
