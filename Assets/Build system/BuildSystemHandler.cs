using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystemHandler : MonoBehaviour
{
    [SerializeField] private LocationGridSave locationGrid;
    [SerializeField] private GameObject prefabGameObject;
    [SerializeField] private GameObject itemWorldPrefab;
    [SerializeField] private Transform spawnPosition;

    private ItemSprites itemSprites;

    private Item item;

    private bool startPlace = false;

    private GameObject @object;    

    private QuickSlotsChanger quickSlots;

    private bool canPlace = true;

    private AudioSource audioSource;

    public Grid<GridNode> Grid { get { return LocationGrid.Grid; } }
    public bool canPlantGrid { get { return LocationGrid.CanPlantToGrid; } }

    public LocationGridSave LocationGrid { get => locationGrid; set { locationGrid = value; ChangeGridToSystems(Grid); StopPlace(); } }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.Stop();
    }

    private void Start()
    {
        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        ChangeGridToSystems(Grid);
    }

    private void ChangeGridToSystems(Grid<GridNode> grid)
    {
        GetComponent<HoeSystemHandler>().Grid = Grid;
        GetComponent<PickaxeHandler>().Grid = Grid;
        GetComponent<AxeHandler>().Grid = Grid;
    }

    public void ChangeGridCropPlaced(GridNode gridNode, bool cropPlaced)
    {
        Grid.gridArray[gridNode.x, gridNode.y].cropPlaced = canPlace;
    }

    public void SetQuickSlotHandler(QuickSlotsChanger quickSlots)
    {
        this.quickSlots = quickSlots;
    }

    private void CreateObject()
    {
        @object = Instantiate(prefabGameObject, spawnPosition);

        @object.AddComponent<SpriteRenderer>().sortingOrder = 1;

        if (item is Crop)
        {
            Crop crop = (Crop)item;

            @object.GetComponent<SpriteRenderer>().sprite = crop.Levels[0];
        }
        else if (item is Sapling)
        {
            Sapling sapling = (Sapling)item;

            @object.GetComponent<SpriteRenderer>().sprite = sapling.SaplingSprite;
        }
        else
        {
            @object.GetComponent<SpriteRenderer>().sprite = itemSprites.GetItemSprite(item.ItemNO);
        }
    }

    public void StartPlace(Item item)
    {
        if (LocationGrid != null && canPlantGrid == true)
        {
            this.item = item;

            startPlace = true;

            if(@object == null)
            {
                CreateObject();
            }            
            else
            {
                if (item is Crop)
                {
                    Crop crop = (Crop)item;

                    @object.GetComponent<SpriteRenderer>().sprite = crop.Levels[0];
                }
                else if (item is Sapling)
                {
                    Sapling sapling = (Sapling)item;

                    @object.GetComponent<SpriteRenderer>().sprite = sapling.SaplingSprite;
                }
                else
                {
                    if(item is Placeable)
                    {
                        Placeable placeable = (Placeable)item;

                        if(placeable.Prefab != null)
                        {
                            @object.GetComponent<SpriteRenderer>().sprite = placeable.Prefab.GetComponent<SpriteRenderer>().sprite;
                        }
                    }
                    else
                    {
                        @object.GetComponent<SpriteRenderer>().sprite = itemSprites.GetItemSprite(item.ItemNO);
                    }
                }
            }
        }
    }

    public void StopPlace()
    {
        startPlace = false;

        if(@object != null)
        {
            Destroy(@object);

            @object = null;
        }
    }

    private GameObject PlaceObject(GridNode gridNode)
    {
        Placeable placeable = (Placeable)item;

        GameObject newObject;

        if (placeable != null && placeable.Prefab != null)
        {
            newObject = Instantiate(placeable.Prefab, spawnPosition);

            newObject.transform.position = @object.transform.position;

            newObject.GetComponent<PlaceableDataSave>().Placeable = placeable;
        }
        else
        {
            newObject = Instantiate(@object, spawnPosition);
        }

        newObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        if(item is Crop)
        {
            newObject.AddComponent<CropGrow>().SetItem(item, gridNode, itemWorldPrefab);

            newObject.tag = "Crop";

            newObject.GetComponent<SpriteRenderer>().sortingOrder = -1;

            gridNode.crop = newObject;
        }
        else if(item is Sapling)
        {
            newObject.tag = "TreeSapling";

            SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();

            Sapling sapling = (Sapling)item;

            newObject.AddComponent<SaplingGrowHandler>().Sapling = sapling;

            spriteRenderer.sortingOrder = 0;
            spriteRenderer.sprite = sapling.SaplingSprite;

            for (int i = gridNode.x - 1; i <= gridNode.x + 1; i++)
            {
                for (int j = gridNode.y - 1; j <= gridNode.y + 1; j++)
                {
                    if (i >= 0 && i < Grid.gridArray.GetLength(0) &&
                        j >= 0 && j < Grid.gridArray.GetLength(1))
                    {
                        Grid.gridArray[i, j].canPlace = false;
                        Grid.gridArray[i, j].canPlant = false;
                        Grid.gridArray[i, j].isWalkable = false;
                        Grid.gridArray[i, j].objectInSpace = newObject;
                    }
                }
            }
        }
        else if(item is Placeable)
        {
            SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();

            spriteRenderer.sortingOrder = 0;

            if (gridNode.x + placeable.StartX >= 0 &&
            gridNode.x + placeable.StartY >= 0 &&
            gridNode.x + placeable.SizeX <= Grid.gridArray.GetLength(0) &&
            gridNode.y + placeable.SizeY <= Grid.gridArray.GetLength(1))
            {
                for (int i = gridNode.x + placeable.StartX; i <= gridNode.x + placeable.SizeX; i++)
                {
                    for (int j = gridNode.y + placeable.StartY; j <= gridNode.y + placeable.SizeY; j++)
                    {
                        if (i < Grid.gridArray.GetLength(0) && j < Grid.gridArray.GetLength(1) &&
                            Grid.gridArray[i, j] != null)
                        {
                            Grid.gridArray[i, j].canPlace = false;
                            Grid.gridArray[i, j].canPlant = false;
                            Grid.gridArray[i, j].isWalkable = false;
                            Grid.gridArray[i, j].objectInSpace = newObject;
                        }
                    }
                }
            }
        }        
        else
        {
            gridNode.objectInSpace = newObject;
        }

        item.Amount -= 1;

        quickSlots.Reinitialize();

        if (item.Amount <= 0)
        {
            StopPlace();
        }

        ChangeCanPlace(gridNode);

        audioSource.Play();

        return newObject;
    }

    public GameObject PlaceObject(Vector3 position, Item item)
    {
        this.item = item;

        if (@object == null)
        {
            CreateObject();
        }

        return PlaceObject(Grid.GetGridObject(position));
    }

    private bool VerifyCanPlace(GridNode gridNode)
    {
        Placeable placeable = (Placeable)item;

        if (gridNode.x + placeable.StartX >=0 &&
            gridNode.x + placeable.StartY >=0 &&
            gridNode.x + placeable.SizeX <= Grid.gridArray.GetLength(0) &&
            gridNode.y + placeable.SizeY <= Grid.gridArray.GetLength(1))
        {
            for (int i = gridNode.x + placeable.StartX; i <= gridNode.x + placeable.SizeX; i++)
            {
                for (int j = gridNode.y + placeable.StartY; j <= gridNode.y + placeable.SizeY; j++)
                {
                    if (Grid.gridArray[i, j].canPlace == false || Grid.gridArray[i, j].isWalkable == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        return false;
    }

    private void ChangeCanPlace(GridNode gridNode)
    {
        Placeable placeable = (Placeable)item;

        for (int i = gridNode.x; i < gridNode.x + placeable.SizeX; i++)
        {
            for (int j = gridNode.y; j < gridNode.y + placeable.SizeY; j++)
            {
                if (item is Crop)
                {
                    Grid.gridArray[gridNode.x, gridNode.y].cropPlaced = true;
                    Grid.gridArray[gridNode.x, gridNode.y].canPlant = true;
                    Grid.gridArray[gridNode.x, gridNode.y].canPlace = false;
                }
                else
                {
                    Grid.gridArray[i, j].canPlace = false;
                    Grid.gridArray[i, j].canPlant = false;
                    Grid.gridArray[i, j].cropPlaced = false;
                }
            }
        }
    }

    private void Update()
    {
        if(LocationGrid != null && startPlace)
        {
            GridNode gridNode = Grid.GetGridObject(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));

            if (gridNode != null)
            {
                if((canPlace = VerifyCanPlace(gridNode)) == true && !(item is Crop))
                {
                    @object.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    if (item is Crop && Grid.gridArray[gridNode.x, gridNode.y].cropPlaced == false && Grid.gridArray[gridNode.x, gridNode.y].canPlant == true)
                    {
                        @object.GetComponent<SpriteRenderer>().color = Color.white;

                        canPlace = true;
                    }
                    else
                    {
                        @object.GetComponent<SpriteRenderer>().color = Color.red;

                        canPlace = false;
                    }
                }

                Vector3 position = Grid.GetWorldPosition(gridNode.x, gridNode.y);


                if (item is Crop)
                {
                    Crop aux = (Crop)item;

                    if(aux.CenterX == true)
                    {
                        position.x += Grid.CellSize / 2f;
                    }
                    if (aux.CenterY == true)
                    {
                        position.y += Grid.CellSize / 2f;
                    }
                }
                else
                {
                    position.x += Grid.CellSize / 2f;
                    position.y += Grid.CellSize / 2f;
                }

                position.z = 0;

                @object.transform.position = position; 

                if(Mouse.current.leftButton.wasPressedThisFrame && canPlace == true)
                {
                    PlaceObject(gridNode);
                }
            }
        }
    }
}
