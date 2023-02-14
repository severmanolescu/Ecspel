using UnityEngine;
using UnityEngine.InputSystem;

public class HoeSystemHandler : MonoBehaviour
{
    [Header("Dry farm:")]
    public Sprite center;
    public Sprite left;
    public Sprite up;
    public Sprite right;
    public Sprite down;
    public Sprite full;

    public Sprite centerDown;
    public Sprite centerLeft;
    public Sprite centerUp;
    public Sprite centerRight;

    public Sprite cornerLeftDown;
    public Sprite cornerLeftUp;
    public Sprite cornerRightDown;
    public Sprite cornerRightUp;

    public Sprite leftright;
    public Sprite updown;

    public Sprite headlight;
    public Sprite headlightGreen;

    [Header("Wet farm:")]
    public Sprite wetFarm;

    [SerializeField] private GameObject prefabGameObject;

    [SerializeField] private Transform spawnLocation;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private int maxDistanceFromPlayer = 1;

    private GridNode nodeToSpawn = null;

    private GameObject headlightObject;

    private BuildSystemHandler buildSystem;

    private HarvestCropHandler harvestCrop;

    private PlayerStats playerStats;

    private Grid grid;

    public Grid Grid { set { grid = value; } }

    private void Start()
    {
        headlightObject = null;

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        harvestCrop = GetComponent<HarvestCropHandler>();

        buildSystem = GetComponent<BuildSystemHandler>();
    }

    public GameObject Spawn(GridNode gridNode)
    {
        if (gridNode.isWalkable && gridNode.canPlant == false && gridNode.canPlace == true)
        {
            GameObject soil = Instantiate(prefabGameObject, spawnLocation);

            soil.tag = "FarmPlot";

            SpriteRenderer sprite = soil.AddComponent<SpriteRenderer>();

            sprite.sortingOrder = -2;

            Vector3 position = grid.GetWorldPosition(gridNode.x, gridNode.y);
            Vector3 scale = Vector3.one;

            position.x += grid.CellSize / 2;
            position.y += grid.CellSize / 2;
            position.z = 0;

            soil.transform.position = position;
            soil.transform.localScale = scale;

            gridNode.isWalkable = true;
            gridNode.canPlant = true;
            gridNode.canPlace = false;
            gridNode.cropPlaced = false;
            gridNode.currentObject = soil.GetComponent<SpriteRenderer>();
            gridNode.objectInSpace = soil;

            soil.AddComponent<FarmPlotHandler>();
            soil.GetComponent<FarmPlotHandler>().WetEffect = wetFarm;

            ChangeSprite(gridNode, sprite, soil.GetComponent<FarmPlotHandler>());

            ChangeNeighbour(gridNode);

            return soil;
        }
        else if (gridNode.crop != null && gridNode.crop.GetComponent<CropGrow>().Destroyed == true)
        {
            Destroy(gridNode.crop);
            gridNode.crop = null;
            gridNode.isWalkable = true;
            gridNode.canPlant = true;
            gridNode.canPlace = false;
            gridNode.cropPlaced = false;

            return gameObject;
        }

        return null;
    }

    public void Spawn(Vector3 position, int noOfDryDays = -1)
    {
        GameObject soil = Spawn(grid.GetGridObject(position));

        if (soil != null && noOfDryDays >= 0)
        {
            soil.GetComponent<FarmPlotHandler>().NoOfDryDays = noOfDryDays;
        }
    }

    public void DestroyPlot(Vector3 position)
    {
        grid.ReinitializeGrid(position);
    }

    public void ChangeNeighbour(GridNode gridNode)
    {
        if (gridNode.x - 1 >= 0)
        {
            ChangeNeighbour(grid.gridArray[gridNode.x - 1, gridNode.y], 3);
        }
        if (gridNode.x + 1 < grid.gridArray.GetLength(0))
        {
            ChangeNeighbour(grid.gridArray[gridNode.x + 1, gridNode.y], 4);
        }
        if (gridNode.y + 1 < grid.gridArray.GetLength(1))
        {
            ChangeNeighbour(grid.gridArray[gridNode.x, gridNode.y + 1], 1);
        }
        if (gridNode.y - 1 >= 0)
        {
            ChangeNeighbour(grid.gridArray[gridNode.x, gridNode.y - 1], 2);
        }
    }

    private void ChangeNeighbour(GridNode gridNode, int type)
    {
        if (gridNode.currentObject != null)
        {
            if (gridNode.currentObject.sprite == full)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(up); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(down); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(left); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(right); break;
                }
            }
            else if (gridNode.currentObject.sprite == up)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(updown); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftUp); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightUp); break;
                }
            }
            else if (gridNode.currentObject.sprite == down)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(updown); ; break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftDown); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightDown); break;
                }
            }
            else if (gridNode.currentObject.sprite == right)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightUp); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightDown); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(leftright); ; break;
                }
            }
            else if (gridNode.currentObject.sprite == left)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftUp); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftDown); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(leftright); break;
                }
            }
            else if (gridNode.currentObject.sprite == updown)
            {
                switch (type)
                {
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight); break;
                }
            }
            else if (gridNode.currentObject.sprite == leftright)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerUp); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerLeft)
            {
                switch (type)
                {
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerRight)
            {
                switch (type)
                {
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerDown)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerUp)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerLeftUp)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft); break;
                    case 4: gridNode.currentObject.sprite = centerUp; break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerLeftDown)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerRightUp)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerUp); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerRightDown)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown); break;
                }
            }
        }
    }

    private void ChangeSprite(GridNode gridNode, SpriteRenderer sprite, FarmPlotHandler farmPlot)
    {
        if (gridNode.x + 1 < grid.gridArray.GetLength(0) && grid.gridArray[gridNode.x + 1, gridNode.y].canPlant == true)
        {
            if (gridNode.y + 1 < grid.gridArray.GetLength(1) && grid.gridArray[gridNode.x, gridNode.y + 1].canPlant == true)
            {
                if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
                {
                    if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                    {
                        farmPlot.ChangeSprites(center);
                    }
                    else
                    {
                        farmPlot.ChangeSprites(centerDown);
                    }
                }
                else
                {
                    if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                    {
                        farmPlot.ChangeSprites(centerLeft);
                    }
                    else
                    {
                        farmPlot.ChangeSprites(cornerLeftDown);
                    }
                }
            }
            else if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
            {
                if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                {
                    farmPlot.ChangeSprites(centerUp);
                }
                else
                {
                    farmPlot.ChangeSprites(leftright);
                }
            }
            else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                farmPlot.ChangeSprites(cornerLeftUp);
            }
            else
            {
                farmPlot.ChangeSprites(left);
            }
        }
        else if (gridNode.y + 1 < grid.gridArray.GetLength(1) && grid.gridArray[gridNode.x, gridNode.y + 1].canPlant == true)
        {
            if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
            {
                if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                {
                    farmPlot.ChangeSprites(centerRight);
                }
                else
                {
                    farmPlot.ChangeSprites(cornerRightDown);
                }
            }
            else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                farmPlot.ChangeSprites(updown);
            }
            else
            {
                farmPlot.ChangeSprites(down);
            }
        }
        else if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
        {
            if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                farmPlot.ChangeSprites(cornerRightUp);
            }
            else
            {
                farmPlot.ChangeSprites(right);
            }
        }
        else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
        {
            farmPlot.ChangeSprites(up);
        }
        else
        {
            farmPlot.ChangeSprites(full);
        }
    }

    public bool PlaceSoil(Hoe hoe)
    {
        if (buildSystem.canPlantGrid == true && nodeToSpawn != null)
        {
            GameObject soil = Spawn(grid.gridArray[nodeToSpawn.x, nodeToSpawn.y]);

            if (soil != null)
            {
                if (soil != gameObject)
                {
                    playerStats.DecreseStamina(hoe.Stamina);
                }

                return true;
            }
        }

        return false;
    }

    public bool DestroySoilMousePosition(Hoe hoe)
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);

        GridNode gridNode = grid.GetGridObject(mousePositionWorld);

        if (gridNode != null &&
            gridNode.objectInSpace != null &&
            gridNode.objectInSpace.CompareTag("FarmPlot") &&
            gridNode.crop == null)
        {
            ChangeDestroyedSoilState(gridNode);

            DestroySoil(gridNode);

            playerStats.DecreseStamina(hoe.Stamina);

            return true;
        }
        return false;
    }

    public void DestroySoil(GridNode gridNode)
    {
        if (gridNode != null)
        {
            for (int i = gridNode.x - 1; i <= gridNode.x + 1; i++)
            {
                for (int j = gridNode.y - 1; j <= gridNode.y + 1; j++)
                {
                    if (i >= 0 && j >= 0 &&
                        i < grid.gridArray.GetLength(0) &&
                        j < grid.gridArray.GetLength(1) &&
                        grid.gridArray[i, j] != null)
                    {
                        if (grid.gridArray[i, j].objectInSpace != null &&
                            grid.gridArray[i, j].objectInSpace.CompareTag("FarmPlot"))
                        {
                            ChangeDestroyedSoilState(grid.gridArray[i, j]);

                            Spawn(grid.GetWorldPosition(grid.gridArray[i, j]));
                        }
                    }
                }
            }
        }
    }

    public void DestroySoil(GridNode gridNode, Placeable placeable)
    {
        if (gridNode != null)
        {
            for (int i = gridNode.x + placeable.StartX - 1; i <= gridNode.x + placeable.SizeX + 1; i++)
            {
                for (int j = gridNode.y + placeable.StartY - 1; j <= gridNode.y + placeable.SizeY + 1; j++)
                {
                    if (i >= 0 && j >= 0 &&
                        i < grid.gridArray.GetLength(0) &&
                        j < grid.gridArray.GetLength(1) &&
                        grid.gridArray[i, j] != null)
                    {
                        if (grid.gridArray[i, j].objectInSpace != null &&
                            grid.gridArray[i, j].objectInSpace.CompareTag("FarmPlot"))
                        {
                            ChangeDestroyedSoilState(grid.gridArray[i, j]);

                            Spawn(grid.GetWorldPosition(grid.gridArray[i, j]));
                        }
                    }
                }
            }
        }
    }

    private void ChangeDestroyedSoilState(GridNode gridNode)
    {
        FarmPlotHandler farmPlotHandler = gridNode.objectInSpace.GetComponent<FarmPlotHandler>();

        if(farmPlotHandler != null)
        {
            GameObject.Find("Global/DayTimer").GetComponent<ChangeSoilsState>().RemoveSoil(farmPlotHandler);
        }

        Destroy(gridNode.objectInSpace);

        grid.ReinitializeGrid(gridNode);
    }

    private void ChangeHeadlightPosition(GridNode gridNode)
    {
        if (gridNode != null)
        {
            Vector3 location = grid.GetWorldPosition(gridNode.x, gridNode.y);

            location.x += grid.CellSize / 2;
            location.y += grid.CellSize / 2;

            location.z = 0;

            headlightObject.transform.position = location;

            if ((gridNode.isWalkable && gridNode.canPlant == false && gridNode.canPlace == true) ||
                 gridNode.crop != null && gridNode.crop.GetComponent<CropGrow>().Destroyed == true)
            {
                headlightObject.GetComponent<SpriteRenderer>().sprite = headlightGreen;
            }
            else
            {
                headlightObject.GetComponent<SpriteRenderer>().sprite = headlight;
            }

        }
        else
        {
            Destroy(headlightObject);
        }
    }

    public void HoeHeadlight(Vector3 playerPosition)
    {
        if (buildSystem.LocationGrid != null && buildSystem.canPlantGrid == true)
        {
            GridNode gridNodePlayerPosition = grid.GetGridObject(playerPosition);

            if (gridNodePlayerPosition != null)
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

                Vector3 mousePositionWorld = mainCamera.ScreenToWorldPoint(mousePosition);

                GridNode gridNode = grid.GetGridObject(mousePositionWorld);

                if (gridNode != null && grid.VerifyDistanceBetweenTwoNodes(gridNodePlayerPosition, gridNode, maxDistanceFromPlayer) == true)
                {
                    if (headlightObject == null)
                    {
                        headlightObject = Instantiate(prefabGameObject);

                        headlightObject.AddComponent<SpriteRenderer>().sprite = headlight;

                        Vector3 scale = Vector3.one;

                        headlightObject.transform.localScale = scale;
                    }

                    nodeToSpawn = gridNode;

                    ChangeHeadlightPosition(grid.gridArray[gridNode.x, gridNode.y]);
                }
                else
                {
                    nodeToSpawn = null;

                    Destroy(headlightObject);
                }
            }
            else
            {
                nodeToSpawn = null;
            }
        }
        else
        {
            nodeToSpawn = null;
        }
    }

    public void StopHeadlight()
    {
        if (headlightObject != null)
        {
            Destroy(headlightObject);
        }
    }

    public bool DestroyCrop(Hoe hoe)
    {
        if(harvestCrop.DestroyCropWithHoe() == true)
        {
            playerStats.DecreseStamina(hoe.Stamina);

            return true;
        }

        return false;
    }
}
