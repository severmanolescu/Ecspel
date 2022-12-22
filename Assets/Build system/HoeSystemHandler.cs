using UnityEngine;
using UnityEngine.InputSystem;

public class HoeSystemHandler : MonoBehaviour
{
    [Header("Farm dry:")]
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

    [Header("Farm wet:")]
    public Sprite centerWater;
    public Sprite leftWater;
    public Sprite upWater;
    public Sprite rightWater;
    public Sprite downWater;
    public Sprite fullWater;

    public Sprite centerDownWater;
    public Sprite centerLeftWater;
    public Sprite centerUpWater;
    public Sprite centerRightWater;

    public Sprite cornerLeftDownWater;
    public Sprite cornerLeftUpWater;
    public Sprite cornerRightDownWater;
    public Sprite cornerRightUpWater;

    public Sprite leftrightWater;
    public Sprite updownWater;

    public Sprite headlight;
    public Sprite headlightGreen;

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
            if (gridNode.currentObject.sprite == full || gridNode.currentObject.sprite == fullWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(up, upWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(down, downWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(left, leftWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(right, rightWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == up || gridNode.currentObject.sprite == upWater)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(updown, updownWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftUp, cornerLeftUpWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightUp, cornerRightUpWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == down || gridNode.currentObject.sprite == downWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(updown, updownWater); ; break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftDown, cornerLeftDownWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightDown, cornerRightDownWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == right || gridNode.currentObject.sprite == rightWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightUp, cornerRightUpWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightDown, cornerRightDownWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(leftright, leftrightWater); ; break;
                }
            }
            else if (gridNode.currentObject.sprite == left || gridNode.currentObject.sprite == leftWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftUp, cornerLeftUpWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftDown, cornerLeftDown); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(leftright, leftrightWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == updown || gridNode.currentObject.sprite == updownWater)
            {
                switch (type)
                {
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft, centerLeftWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight, centerRightWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == leftright || gridNode.currentObject.sprite == leftrightWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerUp, centerUpWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown, centerDownWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerLeft || gridNode.currentObject.sprite == centerLeftWater)
            {
                switch (type)
                {
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerRight || gridNode.currentObject.sprite == centerRightWater)
            {
                switch (type)
                {
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerDown || gridNode.currentObject.sprite == centerDownWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerUp || gridNode.currentObject.sprite == centerUpWater)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerLeftUp || gridNode.currentObject.sprite == cornerLeftUpWater)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft, centerLeftWater); break;
                    case 4: gridNode.currentObject.sprite = centerUp; break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerLeftDown || gridNode.currentObject.sprite == cornerLeftDownWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft, centerLeftWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown, centerDownWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerRightUp || gridNode.currentObject.sprite == cornerRightUpWater)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight, centerRightWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerUp, centerUpWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerRightDown || gridNode.currentObject.sprite == cornerRightDownWater)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight, centerRightWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown, centerDownWater); break;
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
                        farmPlot.ChangeSprites(center, centerWater);
                    }
                    else
                    {
                        farmPlot.ChangeSprites(centerDown, centerDownWater);
                    }
                }
                else
                {
                    if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                    {
                        farmPlot.ChangeSprites(centerLeft, centerLeftWater);
                    }
                    else
                    {
                        farmPlot.ChangeSprites(cornerLeftDown, cornerLeftDownWater);
                    }
                }
            }
            else if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
            {
                if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                {
                    farmPlot.ChangeSprites(centerUp, centerUpWater);
                }
                else
                {
                    farmPlot.ChangeSprites(leftright, leftrightWater);
                }
            }
            else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                farmPlot.ChangeSprites(cornerLeftUp, cornerLeftUpWater);
            }
            else
            {
                farmPlot.ChangeSprites(left, leftWater);
            }
        }
        else if (gridNode.y + 1 < grid.gridArray.GetLength(1) && grid.gridArray[gridNode.x, gridNode.y + 1].canPlant == true)
        {
            if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
            {
                if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                {
                    farmPlot.ChangeSprites(centerRight, centerRightWater);
                }
                else
                {
                    farmPlot.ChangeSprites(cornerRightDown, cornerRightDownWater);
                }
            }
            else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                farmPlot.ChangeSprites(updown, updownWater);
            }
            else
            {
                farmPlot.ChangeSprites(down, downWater);
            }
        }
        else if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
        {
            if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                farmPlot.ChangeSprites(cornerRightUp, cornerRightUpWater);
            }
            else
            {
                farmPlot.ChangeSprites(right, rightWater);
            }
        }
        else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
        {
            farmPlot.ChangeSprites(up, upWater);
        }
        else
        {
            farmPlot.ChangeSprites(full, fullWater);
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
