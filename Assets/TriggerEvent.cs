using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] Transform startLocation;
    [SerializeField] Transform endLocation;

    [SerializeField] float distanceTolerance = .1f;

    [SerializeField] GameObject prefab;

    [SerializeField] float speed = 0.5f;

    [SerializeField] ParticleSystem particleSystemStart;
    [SerializeField] ParticleSystem particleSystemEnd;

    [SerializeField] Dialogue dialogue;
    [SerializeField] Direction direction;

    GameObject spawnedObject = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision != null               && 
            collision.gameObject != null    && 
            collision.isTrigger             && 
            collision.CompareTag("Player")) 
        {
            SpawnObject();

            StartDialogue();

            if(particleSystemStart != null)
            {
                particleSystemStart.Play();
            }
        }
    }

    private void SpawnObject()
    {
        spawnedObject = Instantiate(prefab);

        spawnedObject.transform.position = startLocation.position;
    }

    private void StartDialogue()
    {
        if(dialogue != null)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().ChangeIdleAnimationDirection(direction);

            GameObject.Find("Global").GetComponent<SetDialogueToPlayer>().SetDialogue(dialogue);
        }
    }

    private void FixedUpdate()
    {
        if(spawnedObject != null)
        {
            Vector3 moveDir = (endLocation.position - spawnedObject.transform.position).normalized;

            spawnedObject.transform.position = spawnedObject.transform.position + moveDir * speed * Time.deltaTime;

            if(Vector3.Distance(spawnedObject.transform.position, endLocation.position) < distanceTolerance)
            {
                if (particleSystemEnd != null)
                {
                    particleSystemEnd.Play();
                }

                Destroy(spawnedObject);

                Destroy(gameObject);
            }
        }
    }

}
