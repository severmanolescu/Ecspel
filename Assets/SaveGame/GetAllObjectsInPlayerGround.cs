using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllObjectsInPlayerGround : MonoBehaviour
{
    [SerializeField] private GameObject objectLocation;

    private LocationGridSave locationGrid;

    private GetLocationGrid getLocationGrid;

    private GetItemWorld getItemWorld;

    private void Awake()
    {
        locationGrid = gameObject.GetComponent<LocationGridSave>();

        getItemWorld = GameObject.Find("Global").GetComponent<GetItemWorld>();

        getLocationGrid = GameObject.Find("Global").GetComponent<GetLocationGrid>();
    }

    public List<ObjectSaveGame> GetAllObjects()
    {
        List<ObjectSaveGame> objectSaves = new List<ObjectSaveGame>();

        BoxCollider2D[] objects = objectLocation.GetComponentsInChildren<BoxCollider2D>();

        foreach(BoxCollider2D obj in objects)
        {
            if (obj != null && obj.isTrigger == false)
            {
                if (obj.CompareTag("Tree"))
                {
                    objectSaves.Add(new ObjectSaveGame(
                                    obj.GetComponent<SaveObjectID>().itemID,
                                    obj.transform.position.x,
                                    obj.transform.position.y,
                                    0,
                                    getLocationGrid.GetNoFromLocation(locationGrid),
                                    obj.GetComponent<DamageTree>().Destroyed));
                }
                else
                {
                    objectSaves.Add(new ObjectSaveGame(
                                    obj.GetComponent<SaveObjectID>().itemID,
                                    obj.transform.position.x,
                                    obj.transform.position.y,
                                    0,
                                    getLocationGrid.GetNoFromLocation(locationGrid)));
                }
            }
        }

        return objectSaves;
    }

    public void SetObjectsInArea(List<ObjectSaveGame> objects)
    {
        BoxCollider2D[] objectsInArea = objectLocation.GetComponentsInChildren<BoxCollider2D>();

        foreach(BoxCollider2D box in objectsInArea)
        {
            Destroy(box.gameObject);
        }

        objectsInArea = null;

        foreach (ObjectSaveGame obj in objects)
        {
            if (obj.ItemNo != -1)
            {
                GameObject instantiateObject = Instantiate(getItemWorld.GetObjectFromNo(obj.ItemNo), objectLocation.transform);

                instantiateObject.GetComponent<PositionInGrid>().LocationGrid = getLocationGrid.GetLocationFromNo(obj.LocationIndex);

                instantiateObject.transform.position = new Vector3(obj.PositionX, obj.PositionY);

                if (instantiateObject.CompareTag("Tree"))
                {
                    DamageTree tree = instantiateObject.GetComponent<DamageTree>();

                    tree.Destroyed = obj.TreeCrowDestroy;

                    tree.ChangeCrowDesroy();
                }
            }
        }
    }
}
