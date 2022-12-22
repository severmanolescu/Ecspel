using UnityEngine;

public class AxeHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemWorld;

    private Grid grid;

    private SkillsHandler skillHandler;

    private PlayerStats playerStats;

    public Grid Grid { set { grid = value; } }

    private void Awake()
    {
        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        skillHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();
    }

    private bool UseAxeToObject(GameObject node, int spawn, Item item)
    {
        Axe axe = (Axe)item;

        if (node != null && axe != null)
        {
            DamageTree damageTree = node.GetComponent<DamageTree>();

            if (damageTree != null)
            {
                float skillsAttackBonus = axe.Damage * skillHandler.PowerLevel * 0.05f;

                damageTree.TakeDamage(axe.Damage + skillsAttackBonus, spawn, axe.Level);

                return true;
            }
            else
            {
                GroundWoodDamage groundWood = node.GetComponent<GroundWoodDamage>();

                if (groundWood != null)
                {
                    groundWood.AxeDestroy(axe.Level);

                    return true;
                }
                else
                {
                    PlaceableDataSave placeableData = node.GetComponent<PlaceableDataSave>();

                    if (placeableData != null)
                    {
                        ChestOpenHandler chestOpen = node.GetComponent<ChestOpenHandler>();

                        if (chestOpen != null)
                        {
                            chestOpen.DropAllItems();
                        }
                        else
                        {
                            ForgeOpenHandler forge = node.GetComponent<ForgeOpenHandler>();

                            if (forge != null)
                            {
                                forge.DropAllItems();
                            }
                        }

                        DestroyObject(placeableData, node);

                        return true;
                    }
                }
            }
        }

        playerStats.DecreseStamina(axe.Stamina);

        return false;
    }

    private void DestroyObject(PlaceableDataSave placeableData, GameObject node)
    {
        Destroy(placeableData.gameObject);

        ItemWorld world = Instantiate(itemWorld).GetComponent<ItemWorld>();

        world.transform.position = node.transform.position;

        Placeable newItem = (Placeable)placeableData.Placeable.Copy();

        if (placeableData.ItemWorldSize < 1f)
        {
            world.transform.localScale = new Vector3(placeableData.ItemWorldSize, placeableData.ItemWorldSize, 1f);
        }

        newItem.Amount = 1;

        world.SetItem(newItem);
        world.MoveToPoint();

        grid.ReinitializeGrid(newItem, node.transform.position);
    }

    public void UseAxe(Vector3 position, int spawn, Item item)
    {
        GridNode gridNode = grid.GetGridObject(position);

        if (gridNode != null)
        { 
            if (!UseAxeToObject(gridNode.objectInSpace, spawn, item))            
            {
                switch (spawn)
                {
                    case 1:
                        {
                            if (gridNode.x - 1 >= 0)
                            {
                                UseAxeToObject(grid.gridArray[gridNode.x - 1, gridNode.y].objectInSpace, spawn, item);
                            }

                            break;
                        }
                    case 2:
                        {
                            if (gridNode.x + 1 < grid.gridArray.GetLength(0))
                            {
                                UseAxeToObject(grid.gridArray[gridNode.x + 1, gridNode.y].objectInSpace, spawn, item);
                            }

                            break;
                        }
                    case 3:
                        {
                            if (gridNode.y + 1 < grid.gridArray.GetLength(1))
                            {
                                UseAxeToObject(grid.gridArray[gridNode.x, gridNode.y + 1].objectInSpace, spawn, item);
                            }

                            break;
                        }
                    case 4:
                        {
                            if (gridNode.y - 1 >= 0)
                            {
                                UseAxeToObject(grid.gridArray[gridNode.x, gridNode.y - 1].objectInSpace, spawn, item);
                            }

                            break;
                        }
                }
            }
        }
    }
}