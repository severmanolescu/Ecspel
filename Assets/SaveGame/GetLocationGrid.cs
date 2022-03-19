using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLocationGrid : MonoBehaviour
{
    [SerializeField] private List<LocationGridSave> listOfLocations = new List<LocationGridSave>();

    public List<LocationGridSave> ListOfLocations { get => listOfLocations; set => listOfLocations = value; }

    public int GetNoFromLocation(LocationGridSave @object)
    {
        if (@object != null)
        {
            switch (@object.name)
            {
                case "PlayerHouseGround": return 0;
                case "Road": return 1;
                case "Lake": return 2;
                case "CaveArea": return 3;
                case "Ocean": return 4;

                default: return -1;
            }
        }

        return -1;
    }

    public LocationGridSave GetLocationFromNo(int locationNo)
    {
        if (locationNo >= 0 && locationNo < ListOfLocations.Count)
        {
            return ListOfLocations[locationNo];
        }

        return null;
    }
}
