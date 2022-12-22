using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SkeletHeadHandler : MonoBehaviour
{
    [SerializeField] private float attackPower;

    private Transform player;

    private void Awake()
    {
        player = GameObject.Find("Global/Player").transform;
    }

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(player.position - transform.position, ForceMode2D.Force);

        StartCoroutine(DestroyHead());
    }

    private IEnumerator DestroyHead()
    {
        yield return new WaitForSeconds(2.5f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision != null &&
            collision.isTrigger == false && 
            !collision.CompareTag("NotCollide") && 
            !collision.CompareTag("Enemy"))
        {
            StopAllCoroutines();

            if(collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerStats>().Health -= attackPower;
            }

            Destroy(gameObject);
        }
    }
}
