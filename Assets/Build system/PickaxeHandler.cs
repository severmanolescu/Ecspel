using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeHandler : MonoBehaviour
{
    private Grid<GridNode> grid;

    public Grid<GridNode> Grid { set { grid = value; } }

    private SkillsHandler skillHandler;

    private void Awake()
    {
        skillHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();
    }

    private void DamageStone(GameObject node, Item item)
    {
        Pickaxe pickaxe = (Pickaxe)item;

        if (node != null)
        {
            StoneDamage stoneDamage = node.GetComponent<StoneDamage>();

            if (stoneDamage != null)
            {
                float skillsAttackBonus = pickaxe.Damage * skillHandler.PowerLevel * 0.05f;

                stoneDamage.TakeDamage(pickaxe.Damage + skillsAttackBonus, pickaxe.Level);
            }
        }

        GameObject.Find("Global/Player/Canvas/Stats").GetComponent<PlayerStats>().DecreseStamina(pickaxe.Stamina);
    }

    public void UsePickaxe(Vector3 position, int spawn, Item item)
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
                            DamageStone(grid.gridArray[gridNode.x - 1, gridNode.y].objectInSpace, item);
                        }

                        break;
                    }
                case 2:
                    {
                        if (gridNode.x + 1 < grid.gridArray.GetLength(0))
                        {
                            DamageStone(grid.gridArray[gridNode.x + 1, gridNode.y].objectInSpace, item);
                        }

                        break;
                    }
                case 3:
                    {
                        if (gridNode.y + 1 < grid.gridArray.GetLength(1))
                        {
                            DamageStone(grid.gridArray[gridNode.x, gridNode.y + 1].objectInSpace, item);
                        }

                        break;
                    }
                case 4:
                    {
                        if (gridNode.y - 1 >= 0)
                        {
                            DamageStone(grid.gridArray[gridNode.x, gridNode.y - 1].objectInSpace, item);
                        }

                        break;
                    }
            }
        }
    }
}
