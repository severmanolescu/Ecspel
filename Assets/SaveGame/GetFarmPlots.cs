using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFarmPlots : MonoBehaviour
{
    [SerializeField] private GameObject objectsLocation;

    private HoeSystemHandler hoeSystem;

    private BuildSystemHandler buildSystem;

    private HarvestCropHandler harvestCrop;

    private GetItemFromNO getItem;

    private void Awake()
    {
        hoeSystem = GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>();

        buildSystem = hoeSystem.GetComponent<BuildSystemHandler>();
        harvestCrop = hoeSystem.GetComponent<HarvestCropHandler>();

        getItem = GameObject.Find("Global").GetComponent<GetItemFromNO>();
    }

    public List<FarmPlotSave> GetAllFarmingPlots()
    {
        List<FarmPlotSave> plots = new();

        SpriteRenderer[] plotsInGame = objectsLocation.GetComponentsInChildren<SpriteRenderer>();

        foreach(SpriteRenderer plot in plotsInGame)
        {
            if(plot.CompareTag("FarmPlot"))
            {
                FarmPlotSave farmPlot = new FarmPlotSave();

                farmPlot.PositionX = plot.transform.position.x;
                farmPlot.PositionY = plot.transform.position.y;

                farmPlot.NoOfDryDays = plot.GetComponent<FarmPlotHandler>().NoOfDryDays;

                plots.Add(farmPlot);
            }
        }

        return plots;
    }

    public void PositionFarmingPlots(List<FarmPlotSave> farmPlots)
    {
        SpriteRenderer[] plotsInGame = objectsLocation.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer plot in plotsInGame)
        {
            if (plot.CompareTag("FarmPlot"))
            {
                hoeSystem.DestroyPlot(plot.transform.position);

                Destroy(plot.gameObject);
            }
        }

        foreach (FarmPlotSave farmPlot in farmPlots)
        {
            hoeSystem.Spawn(new Vector3(farmPlot.PositionX, farmPlot.PositionY), farmPlot.NoOfDryDays);
        }
    }

    public List<CropSave> GetAllCrops()
    {
        List<CropSave> cropSaves = new List<CropSave>();

        SpriteRenderer[] cropInGame = objectsLocation.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer crop in cropInGame)
        {
            if(crop.CompareTag("Crop"))
            {
                CropGrow cropGrow = crop.GetComponent<CropGrow>();

                cropSaves.Add(new CropSave(cropGrow.Item.ItemNO,
                                           cropGrow.CurrentSprite,
                                           cropGrow.StartDay,
                                           cropGrow.Destroyed,
                                           crop.transform.position.x,
                                           crop.transform.position.y));
            }
        }

        return cropSaves;
    }

    public void SetCroptsToWorld(List<CropSave> crops)
    {
        SpriteRenderer[] cropInGame = objectsLocation.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer crop in cropInGame)
        {
            if (crop.CompareTag("Crop"))
            {
                harvestCrop.DestroyCrop(crop.transform.position);

                Destroy(crop.gameObject);
            }
        }

        if (crops != null)
        {
            foreach (CropSave crop in crops)
            {
                if (crop != null && crop.CropID != -1)
                {
                    GameObject instantiateCrop = buildSystem.PlaceObject(new Vector3(crop.PositionX, crop.PositionY), getItem.ItemFromNo(crop.CropID));

                    CropGrow cropGrow = instantiateCrop.GetComponent<CropGrow>();

                    if (cropGrow != null)
                    {
                        cropGrow.CurrentSprite = crop.CurrentSprite;

                        cropGrow.StartDay = crop.StartDay;

                        if (crop.Destroyed == true)
                        {
                            cropGrow.DestroyCropOnLoadGame();
                        }

                        cropGrow.transform.position = new Vector3(crop.PositionX, crop.PositionY);
                    }
                    else
                    {
                        Destroy(instantiateCrop);
                    }
                }
            }
        }
    }
}
