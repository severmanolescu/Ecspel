using UnityEngine;
using UnityEditor;

public class ParticleStart : MonoBehaviour
{
    private GameObject log;
    [SerializeField] private GameObject logSpawn;

    private ParticleSystem[] particles;

    private int spawn;

    public int Spawn { set { spawn = value; } }

    private void Awake()
    {
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        log = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Sprites/Tilemap/Environemnt/Environemt/Trees/Prefab/Log.prefab", typeof(GameObject));

        foreach (ParticleSystem particle in particles)
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
}
