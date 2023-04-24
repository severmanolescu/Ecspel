using UnityEngine;

public class AxeHandler : MonoBehaviour
{
    [SerializeField] private GameObject itemWorld;

    private Grid grid;

    private SkillsHandler skillHandler;

    private PlayerStats playerStats;

    private SpawnItem spawnItem;

    public Grid Grid { set { grid = value; } }

    private void Awake()
    {
        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

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
                            else
                            {
                                CampFireHandler campFireHandler = node.GetComponent<CampFireHandler>();

                                if (campFireHandler != null)
                                {
                                    if (campFireHandler.FireStarted())
                                    {
                                        playerStats.DecreseStamina(axe.Stamina);

                                        grid.ReinitializeGrid(placeableData.Placeable, node.transform.position);     

                                        campFireHandler.DestroyFire(true);
                                    }
                                    else
                                    {
                                        campFireHandler.DestroyFire();

                                        DestroyObject(placeableData, node);
                                    }
                                        
                                    return true;
                                }
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

        spawnItem.SpawnItems(placeableData.Placeable, 1, node.transform.position);

        grid.ReinitializeGrid(placeableData.Placeable, node.transform.position);
    }

    public void UseAxe(int spawn, Item item, GridNode mousePosition)
    {
        if (mousePosition != null)
        {
            UseAxeToObject(mousePosition.objectInSpace, spawn, item);
        }
    }
}