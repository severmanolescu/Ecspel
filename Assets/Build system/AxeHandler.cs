using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemWorld;

    private Grid<GridNode> grid;

    private SkillsHandler skillHandler;

    public Grid<GridNode> Grid { set { grid = value; } }

    private void Awake()
    {
        skillHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();
    }

    private void DamageTree(GameObject node, int spawn, Item item)
    {
        Axe axe = (Axe)item;

        if (node != null)
        {
            DamageTree damageTree = node.GetComponent<DamageTree>();

            if (damageTree != null)
            {
                float skillsAttackBonus = axe.Damage * skillHandler.PowerLevel * 0.05f;

                damageTree.TakeDamage(axe.Damage + skillsAttackBonus, spawn, axe.Level);
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

                    else
                    {
                        PlaceableDataSave placeableData = node.GetComponent<PlaceableDataSave>();

                        if(placeableData != null)
                        {
                            Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

                            GridNode gridNode = grid.GetGridObject(node.transform.position);

                            ChestOpenHandler chestOpen = node.GetComponent<ChestOpenHandler>();

                            if (chestOpen != null)
                            {
                                chestOpen.DropAllItems();
                            }

                            ForgeOpenHandler forge = node.GetComponent<ForgeOpenHandler>();

                            if (forge != null)
                            {
                                forge.DropAllItems();
                            }

                            Destroy(placeableData.gameObject);

                            ItemWorld world = Instantiate(itemWorld).GetComponent<ItemWorld>();

                            world.transform.position = node.transform.position;

                            Placeable newItem = (Placeable)placeableData.Placeable.Copy();

                            newItem.Amount = 1;

                            world.SetItem(newItem);

                            world.MoveToPoint();

                            if (gridNode != null)
                            {
                                ChangeGridData(gridNode, grid, newItem);
                            }

                            Debug.Log(newItem.Amount);
                        }
                    }
                }
            }
        }

        GameObject.Find("Global/Player/Canvas/Stats").GetComponent<PlayerStats>().DecreseStamina(axe.Stamina);
    }

    private void ChangeGridData(GridNode gridNode, Grid<GridNode> grid, Placeable placeable)
    {
        for (int i = gridNode.x + placeable.StartX; i <= gridNode.x + placeable.SizeX; i++)
        {
            for (int j = gridNode.y + placeable.StartY; j <= gridNode.y + placeable.SizeY; j++)
            {
                if (i < grid.gridArray.GetLength(0) && j < grid.gridArray.GetLength(1) && 
                    grid.gridArray[i, j] != null)
                {
                    grid.gridArray[i, j].canPlace = true;
                    grid.gridArray[i, j].canPlant = false;
                    grid.gridArray[i, j].isWalkable = true;
                    grid.gridArray[i, j].objectInSpace = null;
                }
            }
        }
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
