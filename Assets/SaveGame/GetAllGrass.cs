using System.Collections.Generic;
using UnityEngine;

public class GetAllGrass : MonoBehaviour
{
    [SerializeField] private GameObject spawnLocation;

    [SerializeField] private GetItemWorld getItemWorld;

    public List<GrassSaveGame> GetAll()
    {
        List<GrassSaveGame> grassSave = new();

        GrassDamage[] grass = spawnLocation.GetComponentsInChildren<GrassDamage>();

        foreach (GrassDamage grassDamage in grass)
        {
            GrassSaveGame newGrassSave = new();

            newGrassSave.ObjectID = grassDamage.GetComponent<SaveObjectID>().itemID;

            newGrassSave.PositionX = grassDamage.transform.position.x;
            newGrassSave.PositionY = grassDamage.transform.position.y;

            grassSave.Add(newGrassSave);
        }

        return grassSave;
    }

    public void SetGrassToWorld(List<GrassSaveGame> grassSaves)
    {
        GrassDamage[] grass = spawnLocation.GetComponentsInChildren<GrassDamage>();

        foreach (GrassDamage grassDamage in grass)
        {
            Destroy(grassDamage.gameObject);
        }

        foreach (GrassSaveGame grassSaveGame in grassSaves)
        {
            GameObject newObject = getItemWorld.GetObjectFromNo(grassSaveGame.ObjectID);

            if (newObject != null)
            {
                newObject = Instantiate(newObject);

                newObject.transform.position = new Vector3(grassSaveGame.PositionX, grassSaveGame.PositionY);

                newObject.transform.parent = spawnLocation.transform;
            }
        }
    }
}
