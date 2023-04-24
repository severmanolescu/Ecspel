using UnityEngine;

public class PickaxeHandler : MonoBehaviour
{
    private Grid grid;

    public Grid Grid { set { grid = value; } }

    private SkillsHandler skillHandler;

    private PlayerStats playerStats;

    private HarvestCropHandler harvestCropHandler;

    private HoeSystemHandler hoeSystemHandler;

    private void Awake()
    {
        skillHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        hoeSystemHandler = GetComponent<HoeSystemHandler>();

        harvestCropHandler = GetComponent<HarvestCropHandler>();
    }

    private void UsePickaxeToObject(GridNode gridNode, Item item)
    {
        Pickaxe pickaxe = (Pickaxe)item;

        if (gridNode.objectInSpace != null)
        {
            StoneDamage stoneDamage = gridNode.objectInSpace.GetComponent<StoneDamage>();

            if (stoneDamage != null)
            {
                float skillsAttackBonus = pickaxe.Damage * skillHandler.PowerLevel * 0.05f;

                stoneDamage.TakeDamage(pickaxe.Damage + skillsAttackBonus, pickaxe.Level);
            }
            else
            {
                harvestCropHandler.DestroyCrop(gridNode);

                hoeSystemHandler.DestroySoilMousePosition(gridNode, 0);
            }
        }

        playerStats.DecreseStamina(pickaxe.Stamina);
    }

    public void UsePickaxe(Item item, GridNode mousePosition)
    {
        if (mousePosition != null)
        {
            UsePickaxeToObject(mousePosition, item);
        }
    }
}
