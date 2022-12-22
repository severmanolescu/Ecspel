using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HarvestCropHandler : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    public void Harvest()
    {
        Grid grid = GetComponent<BuildSystemHandler>().Grid;

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);

        GridNode gridNode = grid.GetGridObject(mousePositionWorld);
        GridNode gridNodePlayer = grid.GetGridObject(GameObject.Find("Player").transform.position);

        if (gridNode != null && gridNodePlayer != null)
        {
            if (gridNode.crop != null)
            {
                if (gridNode.x >= gridNodePlayer.x - 1 && gridNode.x <= gridNodePlayer.x + 1 &&
                    gridNode.y >= gridNodePlayer.y - 1 && gridNode.y <= gridNodePlayer.y + 1)
                {
                    CropGrow cropGrow = gridNode.crop.GetComponent<CropGrow>();

                    if (cropGrow != null)
                    {
                        cropGrow.HarverstCrop();
                    }
                    else
                    {
                        gridNode.crop.GetComponent<CollectHarvest>().HarvestItem();
                    }
                }
            }
        }
    }

    public bool DestroyCropWithHoe()
    {
        Grid grid = GetComponent<BuildSystemHandler>().Grid;

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);

        GridNode gridNode = grid.GetGridObject(mousePositionWorld);
        GridNode gridNodePlayer = grid.GetGridObject(GameObject.Find("Player").transform.position);

        if (gridNode != null && gridNodePlayer != null)
        {
            if (gridNode.crop != null)
            {
                if (gridNode.x >= gridNodePlayer.x - 1 && gridNode.x <= gridNodePlayer.x + 1 &&
                    gridNode.y >= gridNodePlayer.y - 1 && gridNode.y <= gridNodePlayer.y + 1)
                {
                    CropGrow cropGrow = gridNode.crop.GetComponent<CropGrow>();

                    if (cropGrow != null)
                    {
                        if (cropGrow.DestroyCropByHoe() == true)
                        {
                            Destroy(cropGrow.gameObject);

                            gridNode.canPlant = true;
                            gridNode.crop = null;
                            gridNode.cropPlaced = false;

                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    public void DestroyCrop(Vector3 position)
    {
        Grid grid = GetComponent<BuildSystemHandler>().Grid;

        GridNode gridNode = grid.GetGridObject(position);

        if (gridNode != null)
        {
            if (gridNode.crop != null)
            {
                gridNode.crop = null;

                gridNode.canPlace = true;
                gridNode.canPlant = false;
                gridNode.isWalkable = true;
            }
        }
    }
}
