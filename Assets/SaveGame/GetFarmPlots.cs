using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFarmPlots : MonoBehaviour
{
    [SerializeField] private GameObject objectsLocation;

    private HoeSystemHandler hoeSystem;

    private void Awake()
    {
        hoeSystem = GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>();
    }

    public List<Tuple<float, float>> GetAllFarmingPlots()
    {
        List<Tuple<float, float>> plots = new List<Tuple<float, float>>();

        SpriteRenderer[] plotsInGame = objectsLocation.GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer plot in plotsInGame)
        {
            if(plot.CompareTag("FarmPlot"))
            {
                plots.Add(new Tuple<float, float>(plot.transform.position.x, plot.transform.position.y));
            }
        }

        return plots;
    }

    public void PositionFarmingPlots(List<Tuple<float, float>> farmPlots)
    {
        SpriteRenderer[] plotsInGame = objectsLocation.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer plot in plotsInGame)
        {
            hoeSystem.DestroyPlot(plot.transform.position);

            Destroy(plot.gameObject);
        }

        foreach (Tuple<float, float> farmPlot in farmPlots)
        {
            hoeSystem.Spawn(new Vector3(farmPlot.Item1, farmPlot.Item2));
        }
    }
}
