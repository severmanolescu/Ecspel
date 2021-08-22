using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStart : MonoBehaviour
{
    [SerializeField] private GameObject log;
    [SerializeField] private GameObject logSpawn;

    private ParticleSystem[] particles;

    private int spawn;

    private void Start()
    {
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        foreach(ParticleSystem particle in particles)
        {
            particle.Stop();
        }
    }

    public void StartParticles()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Play();
        }
    }

    public void DestroyObject()
    {
        GameObject auxiliarInstantiate;

        if(spawn == 1)
        {
            Vector3 auxiliarPosition = logSpawn.transform.position;

            auxiliarPosition.x -= .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));

            auxiliarPosition.x -= .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));
        }
        else if (spawn == 2)
        {
            Vector3 auxiliarPosition = logSpawn.transform.position;
            auxiliarPosition.x += .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));

            auxiliarPosition.x += .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));
        }
        else if (spawn == 3)
        {
            Vector3 auxiliarPosition = logSpawn.transform.position;
            auxiliarPosition.y += .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));

            auxiliarPosition.y += .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));
        }
        else
        {
            Vector3 auxiliarPosition = logSpawn.transform.position;
            auxiliarPosition.y -= .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));

            auxiliarPosition.y -= .75f;
            auxiliarInstantiate = Instantiate(log, auxiliarPosition, logSpawn.transform.rotation);
            auxiliarInstantiate.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.log, 1));
        }

        Destroy(gameObject);
    }

    public void SetSpawn(int spawn)
    {
        this.spawn = spawn;
    }
    
}
