using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarvestCropHandler : MonoBehaviour
{
    public void Harvest(Vector3 mousePosition)
    {
        Grid<GridNode> grid = GetComponent<BuildSystemHandler>().Grid;

        GridNode gridNode = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        GridNode gridNodePlayer = grid.GetGridObject(GameObject.Find("Player").transform.position);

        if(gridNode != null && gridNodePlayer != null)
        {
            if(gridNode.crop != null)
            {
                if (gridNode.x >= gridNodePlayer.x - 1 && gridNode.x <= gridNodePlayer.x + 1 &&
                    gridNode.y >= gridNodePlayer.y - 1 && gridNode.y <= gridNodePlayer.y + 1)
                {
                    CropGrow cropGrow = gridNode.crop.GetComponent<CropGrow>();

                    if (cropGrow != null)
                    {
                        cropGrow.HarverstCrop();
                    }
                    else
                    {
                        gridNode.crop.GetComponent<CollectHarvest>().HarvestItem();
                    }
                }
            }
        }
    }

    public void DestroyCrop(Vector3 position)
    {
        Grid<GridNode> grid = GetComponent<BuildSystemHandler>().Grid;

        GridNode gridNode = grid.GetGridObject(position);

        if (gridNode != null)
        {
            if (gridNode.crop != null)
            {
                gridNode.crop = null;

                gridNode.canPlace = true;
                gridNode.canPlant = false;
                gridNode.isWalkable = true;
            }
        }
    }
}
