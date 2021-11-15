using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeHandler : MonoBehaviour
{
    private Grid<GridNode> grid;

    public Grid<GridNode> Grid { set { grid = value; } }

    private void DamageTree(GameObject node, int spawn, Item item)
    {
        Axe axe = (Axe)item;

        if (node != null)
        {
            DamageTree damageTree = node.GetComponent<DamageTree>();

            if (damageTree != null)
            {
                damageTree.TakeDamage(axe.Damage, spawn, axe.Level);
            }
            else
            {
                SaplingGrowHandler saplingGrow = node.GetComponent<SaplingGrowHandler>();

                if (saplingGrow != null)
                {
                    saplingGrow.DestroyObject();
                }
                else
                {
                    GroundWoodDamage groundWood = node.GetComponent<GroundWoodDamage>();

                    if(groundWood != null)
                    {
                        groundWood.AxeDestroy(axe.Level);
                    }
                }
            }
        }

        GameObject.Find("Global/Player/Canvas/Stats").GetComponent<PlayerStats>().Stamina -= axe.Stamina;
    }

    public void UseAxe(Vector3 position, int spawn, Item item)
    {
        GridNode gridNode = grid.GetGridObject(position);

        if(gridNode != null)
        {
            switch (spawn)
            {
                case 1:
                    {
                        if (gridNode.x - 1 >= 0)
                        {
                            DamageTree(grid.gridArray[gridNode.x - 1, gridNode.y].objectInSpace, spawn, item);
                        }

                        break;
                    }
                case 2:
                    {
                        if (gridNode.x + 1 < grid.gridArray.GetLength(0))
                        {
                            DamageTree(grid.gridArray[gridNode.x + 1, gridNode.y].objectInSpace, spawn, item);
                        }

                        break;
                    }
                case 3:
                    {
                        if (gridNode.y + 1 < grid.gridArray.GetLength(1))
                        {
                            DamageTree(grid.gridArray[gridNode.x, gridNode.y + 1].objectInSpace, spawn, item);
                        }

                        break;
                    }
                case 4:
                    {
                        if (gridNode.y - 1 >= 0)
                        {
                            DamageTree(grid.gridArray[gridNode.x, gridNode.y - 1].objectInSpace, spawn, item);
                        }

                        break;
                    }
            }
        }
    }
}
