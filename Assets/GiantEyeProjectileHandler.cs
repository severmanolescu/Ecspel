using System.Collections;
using UnityEngine;

public class GiantEyeProjectileHandler : MonoBehaviour
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

        float angleRad = Mathf.Atan2(player.transform.position.y - transform.position.y, 
                                     player.transform.position.x - transform.position.x);

        float angleDeg = (180 / Mathf.PI) * angleRad;

        this.transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        StartCoroutine(DestroyHead());
    }

    private IEnumerator DestroyHead()
    {
        yield return new WaitForSeconds(2f);

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null &&
            collision.isTrigger == false &&
            !collision.CompareTag("NotCollide") &&
            !collision.tag.Contains("Enemy"))
        {
            StopAllCoroutines();

            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerStats>().Health -= attackPower;
            }

            Destroy(gameObject);
        }
    }
}
