using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSaveHadler : MonoBehaviour
{
    [SerializeField] private List<LocationGridSave> locationGridSaves = new List<LocationGridSave>();

    public List<GridSave[,]> GetAllGridLocationData()
    {
        List<GridSave[,]> gridSaves = new List<GridSave[,]>();

        foreach(LocationGridSave locationGridSave in locationGridSaves)
        {
            GridSave[,] gridSave = new GridSave[locationGridSave.Grid.gridArray.GetLength(0), locationGridSave.Grid.gridArray.GetLength(1)];

            for(int indexOfWidth = 0; indexOfWidth < locationGridSave.Grid.gridArray.GetLength(0); indexOfWidth++)
            {
                for (int indexOfHeight = 0; indexOfHeight < locationGridSave.Grid.gridArray.GetLength(1); indexOfHeight++)
                {
                    gridSave[indexOfWidth, indexOfHeight] = new GridSave();

                    gridSave[indexOfWidth, indexOfHeight].IsWalkable = locationGridSave.Grid.gridArray[indexOfWidth, indexOfHeight].isWalkable;
                    gridSave[indexOfWidth, indexOfHeight].CanPlant = locationGridSave.Grid.gridArray[indexOfWidth, indexOfHeight].canPlant;
                    gridSave[indexOfWidth, indexOfHeight].CanPlace = locationGridSave.Grid.gridArray[indexOfWidth, indexOfHeight].canPlace;
                    gridSave[indexOfWidth, indexOfHeight].CropPlaced = locationGridSave.Grid.gridArray[indexOfWidth, indexOfHeight].cropPlaced;
                }
            }

            gridSaves.Add(gridSave);
        }

        return gridSaves;
    }

    public void SetDataToGridLocations(List<GridSave[,]> gridNodes)
    {
        for(int indexOfLocation = 0; indexOfLocation < locationGridSaves.Count; indexOfLocation++)
        {
            for (int indexOfWidth = 0; indexOfWidth < gridNodes[indexOfLocation].GetLength(0); indexOfWidth++)
            {
                for (int indexOfHeight = 0; indexOfHeight < gridNodes[indexOfLocation].GetLength(1); indexOfHeight++)
                {
                    locationGridSaves[indexOfLocation].Grid.gridArray[indexOfWidth, indexOfHeight].isWalkable = gridNodes[indexOfLocation][indexOfWidth, indexOfHeight].IsWalkable;
                    locationGridSaves[indexOfLocation].Grid.gridArray[indexOfWidth, indexOfHeight].canPlace = gridNodes[indexOfLocation][indexOfWidth, indexOfHeight].CanPlace;
                    locationGridSaves[indexOfLocation].Grid.gridArray[indexOfWidth, indexOfHeight].canPlant = gridNodes[indexOfLocation][indexOfWidth, indexOfHeight].CanPlant;
                    locationGridSaves[indexOfLocation].Grid.gridArray[indexOfWidth, indexOfHeight].cropPlaced = gridNodes[indexOfLocation][indexOfWidth, indexOfHeight].CropPlaced;
                }
            }
        }
    }
}
