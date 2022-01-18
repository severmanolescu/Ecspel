using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemWorld : MonoBehaviour
{
    [SerializeField] private List<GameObject> listOfObjects = new List<GameObject>();

    public int GetNoFromObject(GameObject @object)
    {
        if(@object != null)
        {
            switch(@object.name)
            {
                case "FirAlmostMatureP": return 1;
                case "FirMatureP": return 2;
                case "PineAlmostMatureP": return 3;
                case "PineMatureP": return 4;
                case "StoneP (1)": return 5;
                case "StoneP (2)": return 6;
                case "StoneP (3)": return 7;
                case "StoneP (4)": return 8;
                case "StoneP (5)": return 9;
                case "StoneP (6)": return 10;
                case "StoneP 3": return 11;
                case "IronP (1)": return 12;
                case "IronP (2)": return 13;
                case "IronP (3)": return 14;
                case "IronP (4)": return 15;
                case "IronP (5)": return 16;
                case "IronP (6)": return 17;
                case "IronP 3": return 18;
                case "Tree1MatureP": return 19;
                case "Tree2MatureP": return 20;
                case "Tree3MatureP": return 21;
                case "GroundWood (1)": return 22;
                case "GroundWood (2)": return 23;
                case "GroundWood (3)": return 24;
                case "GroundWood (4)": return 25;
                case "GroundWood (5)": return 26;
                case "GroundWood (6)": return 27;
                case "GroundWood (7)": return 28;
                case "GroundWood (8)": return 29;
                case "GroundWood (9)": return 30;
                case "GroundWood (10)": return 31;
                case "GroundWood (11)": return 32;

                default: return -1;
            }
        }

        return -1;
    }

    public GameObject GetObjectFromNo(int objectNo)
    {
        if(objectNo > 0 && objectNo < listOfObjects.Count)
        {
            return listOfObjects[objectNo - 1];
        }

        return null;
    }
}
