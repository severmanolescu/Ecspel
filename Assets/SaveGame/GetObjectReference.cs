using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetObjectReference : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects = new();

    public int GetObjectId(GameObject gameObject)
    {
        if(gameObject != null)
        {
            for(int indexOfObject = 0; indexOfObject < objects.Count; indexOfObject++)
            {
                if(objects[indexOfObject].name == gameObject.name)
                {
                    return indexOfObject;
                }
            }
        }

        return -1;
    }

    public GameObject GetObjectFromId(int id)
    {
        return objects[id];
    }
}
