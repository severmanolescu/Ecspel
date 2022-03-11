using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectsFromWorld : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnAreas;

    private GetLocationGrid getLocationGrid;

    private GetItemWorld getItemWorld;

    private void Awake()
    {
        getItemWorld = GameObject.Find("Global"). GetComponent<GetItemWorld>();

        getLocationGrid = GameObject.Find("Global"). GetComponent<GetLocationGrid>();
    }

    public List<ObjectSaveGame> GetAllObjectsFromArea()
    {
        List<ObjectSaveGame> objects = new List<ObjectSaveGame>();

        foreach (GameObject spawnArea in spawnAreas)
        {
            if (spawnArea != null)
            {
                SpawnObjectsArea[] spawnObjectsArea = spawnArea.GetComponentsInChildren<SpawnObjectsArea>();

                for (int indexOfArea = 0; indexOfArea < spawnObjectsArea.Length; indexOfArea++)
                {
                    BoxCollider2D[] objectsInArea = spawnObjectsArea[indexOfArea].GetComponentsInChildren<BoxCollider2D>();

                    LocationGridSave location = spawnObjectsArea[indexOfArea].GetComponentInParent<LocationGridSave>();

                    foreach (BoxCollider2D obj in objectsInArea)
                    {
                        if (!obj.CompareTag("Area") && obj.isTrigger == false)
                        {
                            int itemNo = obj.gameObject.GetComponent<SaveObjectID>().itemID;

                            if (itemNo > 0)
                            {
                                if (obj.gameObject.tag == "Tree")
                                {
                                    objects.Add(new ObjectSaveGame(itemNo,
                                                                   obj.transform.position.x,
                                                                   obj.transform.position.y,
                                                                   indexOfArea,
                                                                   getLocationGrid.GetNoFromLocation(location) - 1,
                                                                   obj.GetComponent<DamageTree>().Destroyed));
                                }
                                else
                                {
                                    objects.Add(new ObjectSaveGame(itemNo,
                                                                   obj.transform.position.x,
                                                                   obj.transform.position.y,
                                                                   indexOfArea,
                                                                   getLocationGrid.GetNoFromLocation(location) - 1));
                                }
                            }
                        }
                    }
                }
            }
        }

        return objects;
    }

    public void SetObjectsToWorld(List<ObjectSaveGame> objects)
    {
        foreach(GameObject spawnArea in spawnAreas)
        {
            if (spawnArea != null)
            {
                BoxCollider2D[] objectsInArea = spawnArea.GetComponentsInChildren<BoxCollider2D>();

                foreach (BoxCollider2D obj in objectsInArea)
                {
                    if (!obj.CompareTag("Area"))
                    {
                        Destroy(obj.gameObject);
                    }
                }
            }
        }

        foreach (ObjectSaveGame objectSave in objects)
        {
            if (objectSave.LocationIndex >= 0)
            {
                if (spawnAreas[objectSave.LocationIndex])
                {
                    GameObject instantiateObject = Instantiate(getItemWorld.GetObjectFromNo(objectSave.ItemNo));

                    instantiateObject.GetComponent<PositionInGrid>().LocationGrid = getLocationGrid.GetLocationFromNo(objectSave.LocationIndex);

                    instantiateObject.transform.position = new Vector3(objectSave.PositionX, objectSave.PositionY);

                    Transform spawnLocation = spawnAreas[objectSave.LocationIndex].GetComponentsInChildren<SpawnObjectsArea>()[objectSave.AreaIndex].transform;

                    instantiateObject.transform.SetParent(spawnLocation);

                    if (instantiateObject.tag == "Tree")
                    {
                        DamageTree tree = instantiateObject.GetComponent<DamageTree>();

                        tree.Destroyed = objectSave.TreeCrowDestroy;

                        tree.ChangeCrowDesroy();
                    }
                }
            }
        }
    }
}
