using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestCropHandler : MonoBehaviour
{
    public void Harvest(Vector3 mousePosition)
    {
        Grid<GridNode> grid = GetComponent<BuildSystemHandler>().Grid;

        GridNode gridNode = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        GridNode gridNodePlayer = grid.GetGridObject(GameObject.Find("Player").transform.position);

        if(gridNode != null && gridNodePlayer != null)
        {
            if(gridNode.crop != null)
            {
                if (gridNode.x >= gridNodePlayer.x - 1 && gridNode.x <= gridNodePlayer.x + 1 &&
                    gridNode.y >= gridNodePlayer.y - 1 && gridNode.y <= gridNodePlayer.y + 1)
                {
                    gridNode.crop.GetComponent<CropGrow>().HarverstCrop();
                }
            }
        }
    }
}
