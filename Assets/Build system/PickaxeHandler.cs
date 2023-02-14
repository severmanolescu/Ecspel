using UnityEngine;

public class PickaxeHandler : MonoBehaviour
{
    private Grid grid;

    public Grid Grid { set { grid = value; } }

    private SkillsHandler skillHandler;

    private PlayerStats playerStats;

    private void Awake()
    {
        skillHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();
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

        playerStats.DecreseStamina(pickaxe.Stamina);
    }

    public void UsePickaxe(Item item, GridNode mousePosition)
    {
        if (mousePosition != null)
        {
            DamageStone(mousePosition.objectInSpace, item);
        }
    }
}
