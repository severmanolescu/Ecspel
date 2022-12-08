using System.Collections.Generic;
using UnityEngine;

public class ChangeSoilsState : MonoBehaviour
{
    private List<FarmPlotHandler> farmPlotHandlers = new List<FarmPlotHandler>();

    public void AddSoil(FarmPlotHandler farmPlot)
    {
        if (farmPlot != null && !farmPlotHandlers.Contains(farmPlot))
        {
            farmPlotHandlers.Add(farmPlot);
        }
    }

    public void DryAllDoils()
    {
        foreach (FarmPlotHandler farmPlotHandler in farmPlotHandlers)
        {
            farmPlotHandler.DrySoilChangeSprite();
        }
    }

    public void WetAllDoils()
    {
        foreach (FarmPlotHandler farmPlotHandler in farmPlotHandlers)
        {
            farmPlotHandler.WetSoilChangeSprite();
        }
    }
}
