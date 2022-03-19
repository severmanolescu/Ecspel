using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCanHandler : MonoBehaviour
{
    private Grid<GridNode> grid;

    private PlayerStats playerStats;

    public Grid<GridNode> Grid { set { grid = value; } }

    private void Awake()
    {
        playerStats = playerStats = GameObject.Find("Global/Player/Canvas/Stats").GetComponent<PlayerStats>();
        grid = GameObject.Find("PlayerHouseGround").GetComponent<LocationGridSave>().Grid;
    }

    private void ChangeSoilState(GameObject node, int spawn, WateringCan item)
    {
        if(node != null && node.CompareTag("FarmPlot"))
        {
            FarmPlotHandler farmPlot = node.GetComponent<FarmPlotHandler>();

            if(farmPlot != null)
            {
                playerStats.DecreseStamina(item.Stamina);

                farmPlot.WetSoilChangeSprite();
            }
        }
    }

    public void UseWateringcan(Vector3 position, int spawn, WateringCan wateringCan)
    {
        GridNode gridNode = grid.GetGridObject(position);

        if (gridNode != null)
        {
            switch (spawn)
            {
                case 1:
                    {
                        if (gridNode.x - 1 >= 0)
                        {
                            ChangeSoilState(grid.gridArray[gridNode.x - 1, gridNode.y].objectInSpace, spawn, wateringCan);
                        }

                        break;
                    }
                case 2:
                    {
                        if (gridNode.x + 1 < grid.gridArray.GetLength(0))
                        {
                            ChangeSoilState(grid.gridArray[gridNode.x + 1, gridNode.y].objectInSpace, spawn, wateringCan);
                        }

                        break;
                    }
                case 3:
                    {
                        if (gridNode.y + 1 < grid.gridArray.GetLength(1))
                        {
                            ChangeSoilState(grid.gridArray[gridNode.x, gridNode.y + 1].objectInSpace, spawn, wateringCan);
                        }

                        break;
                    }
                case 4:
                    {
                        if (gridNode.y - 1 >= 0)
                        {
                            ChangeSoilState(grid.gridArray[gridNode.x, gridNode.y - 1].objectInSpace, spawn, wateringCan);
                        }

                        break;
                    }
            }
        }
    }
}
