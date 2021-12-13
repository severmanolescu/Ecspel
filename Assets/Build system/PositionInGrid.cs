using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInGrid : MonoBehaviour
{
    [SerializeField] private int startScaleX;
    [SerializeField] private int startScaleY;

    [SerializeField] private int scaleX;
    [SerializeField] private int scaleY;

    [SerializeField] LocationGridSave locationGrid;

    public LocationGridSave LocationGrid { get => locationGrid; set => locationGrid = value; }

    private void Start()
    {
        if(locationGrid == null)
        {
            locationGrid = GetComponentInParent<LocationGridSave>();
        }

        Grid<GridNode> grid = LocationGrid.Grid;

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
                    if (i < grid.gridArray.GetLength(0) && j < grid.gridArray.GetLength(1))
                    {
                        if (grid.gridArray[i, j] != null)
                        {
                            grid.gridArray[i, j].canPlace = false;
                            grid.gridArray[i, j].canPlant = false;
                            grid.gridArray[i, j].isWalkable = false;
                            grid.gridArray[i, j].objectInSpace = this.gameObject;
                        }
                    }
                }
            }

            DamageTree damageTree = GetComponent<DamageTree>();

            if(damageTree != null)
            {
                damageTree.GetDataFromPosition(startScaleX, startScaleY, scaleX, scaleY);
            }
            else
            {
                StoneDamage stoneDamage = GetComponent<StoneDamage>();

                if (stoneDamage != null)
                {
                    stoneDamage.GetDataFromPosition(startScaleX, startScaleY, scaleX, scaleY);
                }
            }

            Destroy(this);
        }
    }
}
