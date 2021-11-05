using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInGrid : MonoBehaviour
{
    [SerializeField] private int startScaleX;
    [SerializeField] private int startScaleY;

    [SerializeField] private int scaleX;
    [SerializeField] private int scaleY;

    private void Update()
    {
        Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grig;

        GridNode gridNode = grid.GetGridObject(transform.position);

        if(gridNode != null)
        {
            Vector3 position = grid.GetWorldPosition(gridNode.x, gridNode.y);

            position.x += grid.CellSize / 2;
            position.y += grid.CellSize / 2;
            position.z = 0;

            transform.position = position;

            for(int i = gridNode.x + startScaleX; i <= gridNode.x + scaleX; i++)
            {
                for (int j = gridNode.y + startScaleY; j <= gridNode.y + scaleY; j++)
                {
                    if(grid.gridArray[i, j] != null)
                    {
                        grid.gridArray[i, j].canPlace = false;
                        grid.gridArray[i, j].canPlant = false;
                    }
                }
            }

            Destroy(this);
        }
    }
}
