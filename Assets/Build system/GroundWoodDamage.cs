using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundWoodDamage : MonoBehaviour
{
    [SerializeField] private Item log;

    [SerializeField] private GameObject itemWorld;

    [SerializeField] private int level;

    private void ChangeGridData(GridNode gridNode, Grid<GridNode> grid)
    {
        for (int i = gridNode.x; i <= gridNode.x + 1; i++)
        {
            for (int j = gridNode.y; j <= gridNode.y + 1; j++)
            {
                if (grid.gridArray[i, j] != null)
                {
                    grid.gridArray[i, j].canPlace = true;
                    grid.gridArray[i, j].canPlant = false;
                    grid.gridArray[i, j].isWalkable = true;
                    grid.gridArray[i, j].objectInSpace = null;
                }
            }
        }
    }

    public void AxeDestroy(int level)
    {
        if (level >= this.level)
        {
            ItemWorld newItem = Instantiate(itemWorld).GetComponent<ItemWorld>();

            newItem.transform.position = transform.position;

            Item newLog = Instantiate(log);

            newLog.Amount = 1;

            newItem.SetItem(newLog);
            newItem.MoveToPoint();

            Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grig;

            GridNode gridNode = grid.GetGridObject(transform.position);

            if (gridNode != null)
            {
                ChangeGridData(gridNode, grid);
            }

            Destroy(gameObject);
        }
    }
}
