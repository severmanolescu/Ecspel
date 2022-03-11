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
                if(objects[indexOfObject] == gameObject)
                {
                    return indexOfObject;
                }
            }
        }

        return -1;
    }

    public GameObject GetObjectFromId(int id)
    {
        if(id >= 0 && id < objects.Count)
        {
            for(int indexOfObject = 0; indexOfObject < objects.Count; indexOfObject++)
            {
                DialoguePlayerEnterInTrigger dialogue = objects[indexOfObject].GetComponent<DialoguePlayerEnterInTrigger>();

                if(dialogue != null)
                {
                    if(dialogue.DialogueId == id)
                    {
                        return objects[indexOfObject];
                    }
                }
            }

            return objects[id];
        }

        return null;
    }
}
