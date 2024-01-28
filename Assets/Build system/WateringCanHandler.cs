using UnityEngine;

public class WateringCanHandler : MonoBehaviour
{
    private Grid grid;

    private PlayerStats playerStats;

    public Grid Grid { set { grid = value; } }

    private void Awake()
    {
        playerStats = playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();
        grid = GameObject.Find("Global/AI_Grid").GetComponent<LocationGridSave>().Grid;
    }

    private void ChangeSoilState(GameObject node, WateringCan item)
    {
        if (node != null && node.CompareTag("FarmPlot"))
        {
            FarmPlotHandler farmPlot = node.GetComponent<FarmPlotHandler>();

            if (farmPlot != null)
            {
                playerStats.DecreseStamina(item.Stamina);

                farmPlot.WetSoilChangeSprite();
            }
        }
    }

    public void UseWateringcan(WateringCan wateringCan, GridNode farmObject)
    {
        if (farmObject != null)
        {
            ChangeSoilState(farmObject.objectInSpace, wateringCan);
        }
    }
}
