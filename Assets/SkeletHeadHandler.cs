using System.Collections;
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
        Vector3 direction = player.position - transform.position;

        GetComponent<Rigidbody2D>().AddRelativeForce(direction, ForceMode2D.Impulse);

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
            !collision.tag.Contains("Enemy"))
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
