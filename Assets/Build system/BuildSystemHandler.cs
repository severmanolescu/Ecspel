using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystemHandler : MonoBehaviour
{
    [SerializeField] private LocationGridSave locationGrid;
    [SerializeField] private GameObject prefabGameObject;
    [SerializeField] private GameObject itemWorldPrefab;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Camera mainCamera;

    private Item item;

    private bool startPlace = false;

    private GameObject toPlaceObject = null;

    private QuickSlotsChanger quickSlots;

    private HoeSystemHandler hoeSystem;

    private bool canPlace = true;

    private AudioSource audioSource;

    private GridNode lastGridNode = null;

    private PlayerMovement playerMovement;

    private FenceSystemHandler fenceSystem;

    public Grid Grid { get { return LocationGrid.Grid; } }
    public bool canPlantGrid { get { return LocationGrid.CanPlantToGrid; } }

    public LocationGridSave LocationGrid { get => locationGrid; set { locationGrid = value; ChangeGridToSystems(Grid); StopPlace(); } }

    private void Awake()
    {
        hoeSystem = GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>();

        audioSource = GetComponent<AudioSource>();

        audioSource.Stop();

        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

//        fenceSystem = GetComponent<FenceSystemHandler>();
    }

    private void Start()
    {
        ChangeGridToSystems(Grid);
    }

    private void ChangeGridToSystems(Grid grid)
    {
        GetComponent<HoeSystemHandler>().Grid = Grid;
        GetComponent<PickaxeHandler>().Grid = Grid;
        GetComponent<AxeHandler>().Grid = Grid;
        GetComponent<WateringCanHandler>().Grid = Grid;

//        fenceSystem.Grid = Grid;
    }

    public void ChangeGridCropPlaced(GridNode gridNode, bool cropPlaced)
    {
        Grid.gridArray[gridNode.x, gridNode.y].cropPlaced = cropPlaced;
    }

    public void SetQuickSlotHandler(QuickSlotsChanger quickSlots)
    {
        this.quickSlots = quickSlots;
    }

    private void CreateObject()
    {
        toPlaceObject = Instantiate(prefabGameObject, spawnPosition);

        toPlaceObject.AddComponent<SpriteRenderer>().sortingOrder = 1;

        switch (item)
        {
            case Crop:
                {
                    Crop crop = (Crop)item;

                    toPlaceObject.GetComponent<SpriteRenderer>().sprite = crop.Levels[0];

                    break;
                }
            case Sapling:
                {
                    Sapling sapling = (Sapling)item;

                    toPlaceObject.GetComponent<SpriteRenderer>().sprite = sapling.SaplingSprite;

                    break;
                }
            default:
                {
                    toPlaceObject.GetComponent<SpriteRenderer>().sprite = item.ItemSprite;

                    break;
                }
        }
    }

    public void StartPlace(Item item)
    {
        if (item != this.item)
        {
            StopPlace();

            if (LocationGrid != null && canPlantGrid == true)
            {
                this.item = item;

                if (toPlaceObject == null)
                {
                    CreateObject();
                }

                switch (item)
                {
                    case Crop:
                        {
                            Crop crop = (Crop)item;

                            toPlaceObject.GetComponent<SpriteRenderer>().sprite = crop.Levels[0];

                            break;
                        }
                    case Sapling:
                        {
                            Sapling sapling = (Sapling)item;

                            toPlaceObject.GetComponent<SpriteRenderer>().sprite = sapling.SaplingSprite;

                            break;
                        }
                    default:
                        {
                            if (item is Placeable)
                            {
                                Placeable placeable = (Placeable)item;

                                if (placeable.Prefab != null)
                                {
                                    toPlaceObject.GetComponent<SpriteRenderer>().sprite = placeable.Prefab.GetComponent<SpriteRenderer>().sprite;
                                }
                            }
                            else
                            {
                                toPlaceObject.GetComponent<SpriteRenderer>().sprite = item.ItemSprite;
                            }

                            break;
                        }
                }

                startPlace = true;
            }
        }
    }

    public void StopPlace()
    {
        startPlace = false;

        lastGridNode = null;

        item = null;

        if (toPlaceObject != null)
        {
            Destroy(toPlaceObject);

            toPlaceObject = null;
        }
    }

    private GameObject PlaceObject(GridNode gridNode)
    {
        Placeable placeable = (Placeable)item;

        GameObject newObject;

        if (placeable != null && placeable.Prefab != null)
        {
            newObject = Instantiate(placeable.Prefab, spawnPosition);

            newObject.transform.position = toPlaceObject.transform.position;

            PlaceableDataSave placeableData = newObject.GetComponent<PlaceableDataSave>();

            if(placeableData != null)
            {
                placeableData.Placeable = placeable;

            }
        }
        else
        {
            newObject = Instantiate(toPlaceObject, spawnPosition);
        }

        newObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        ChangeCanPlace(gridNode, newObject);

        SpriteRenderer spriteRenderer = newObject.GetComponent<SpriteRenderer>();

        switch (item)
        {
            case Crop:
                {
                    newObject.AddComponent<CropGrow>().SetItem(item, gridNode, itemWorldPrefab);

                    newObject.tag = "Crop";

                    Crop crop = (Crop)item;

                    if (crop.TallCrop)
                    {
                        spriteRenderer.sortingOrder = 0;
                    }
                    else
                    {
                        spriteRenderer.sortingOrder = -1;
                    }

                    spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;

                    gridNode.crop = newObject;

                    break;
                }
            case Sapling:
                {
                    newObject.tag = "TreeSapling";

                    Sapling sapling = (Sapling)item;

                    newObject.AddComponent<SaplingGrowHandler>().Sapling = sapling;

                    spriteRenderer.sortingOrder = 0;
                    spriteRenderer.sprite = sapling.SaplingSprite;
                    spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;

                    break;
                }
            case Placeable:
                {
                    spriteRenderer.sortingOrder = 0;
                    spriteRenderer.spriteSortPoint = SpriteSortPoint.Pivot;

                    if (item.Name.Contains("Fence"))
                    {
                        fenceSystem.CheckFencePlacement(gridNode);
                    }

                    break;
                }
            default:
                {
                    gridNode.objectInSpace = newObject;

                    break;
                }
        }

        item.Amount -= 1;

        quickSlots.Reinitialize();

        if (item.Amount <= 0)
        {
            StopPlace();
        }

        audioSource.Play();

        CheckGridNode(gridNode);

        return newObject;
    }

    public GameObject PlaceObject(Vector3 position, Item item)
    {
        this.item = item;

        if (toPlaceObject == null)
        {
            CreateObject();
        }

        return PlaceObject(Grid.GetGridObject(position));
    }

    private void CheckGridNode(GridNode gridNode)
    {
        if (toPlaceObject != null)
        {
            if ((canPlace = VerifyCanPlace(gridNode)) == true && !(item is Crop))
            {
                toPlaceObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                if (item is Crop && Grid.gridArray[gridNode.x, gridNode.y].cropPlaced == false && Grid.gridArray[gridNode.x, gridNode.y].canPlant == true)
                {
                    toPlaceObject.GetComponent<SpriteRenderer>().color = Color.white;

                    canPlace = true;
                }
                else
                {
                    toPlaceObject.GetComponent<SpriteRenderer>().color = Color.red;

                    canPlace = false;
                }
            }
        }
    }

    private bool VerifyCanPlace(GridNode gridNode)
    {
        Placeable placeable = (Placeable)item;

        return Grid.CheckCanPlaceBuildSystem(placeable, gridNode);
    }

    private void ChangeCanPlace(GridNode gridNode, GameObject newObject)
    {
        if (item is Crop)
        {
            Grid.gridArray[gridNode.x, gridNode.y].cropPlaced = true;
            Grid.gridArray[gridNode.x, gridNode.y].canPlace = false;
        }
        else
        {
            Placeable placeable = (Placeable)item;

            List<GameObject> objectsToDestroy = Grid.PlaceObjectInGrid(placeable, gridNode, newObject);

            if (objectsToDestroy != null)
            {
                foreach (GameObject gameObject in objectsToDestroy)
                {
                    Destroy(gameObject);
                }
            }

            hoeSystem.DestroySoil(gridNode, placeable);
        }
    }

    public GameObject GetCropFromPosition(Vector3 position)
    {
        GridNode gridNode = Grid.GetGridObject(position);

        if (gridNode != null)
        {
            return gridNode.crop;
        }

        return null;
    }

    private void ChangeObjectGridCellPosition(GridNode gridNode)
    {
        Vector3 position = Grid.GetWorldPosition(gridNode.x, gridNode.y);

        if (item is Crop)
        {
            Crop aux = (Crop)item;

            if (aux.CenterX == true)
            {
                position.x += Grid.CellSize / 2f;
            }
            if (aux.CenterY == true)
            {
                position.y += Grid.CellSize / 2f;
            }
        }
        else if (item is Placeable)
        {
            Placeable placeable = (Placeable)item;

            if (placeable != null)
            {
                if (placeable.PositionInCenter == true)
                {
                    position.x += Grid.CellSize / 2f;
                    position.y += Grid.CellSize / 2f;
                }
                else
                {
                    position.x += Grid.CellSize / 2f;
                }
            }
        }

        position.z = 0;

        toPlaceObject.transform.position = position;
    }

    public void ChangeNodeData(Vector3 position, bool isWalkable, bool canPlant, bool canPlace, bool cropPlaced)
    {
        if(position != DefaulData.nullVector)
        {
            GridNode gridNode = Grid.GetGridObject(position);

            if(gridNode != null)
            {
                gridNode.isWalkable = isWalkable;
                gridNode.canPlant = canPlant;
                gridNode.canPlace = canPlace;
                gridNode.cropPlaced = cropPlaced;
            }
        }
    }

    private void Update()
    {
        if (LocationGrid != null && startPlace && playerMovement.TabOpen == false && playerMovement.CanMove == true && playerMovement.Dialogue == false)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

            GridNode gridNode = Grid.GetGridObject(mainCamera.ScreenToWorldPoint(mousePosition));

            if (gridNode != null && (lastGridNode == null || lastGridNode != gridNode))
            {
                if (item.Name.Contains("Fence"))
                {
                    fenceSystem.CheckFencePlacement(lastGridNode);
                    fenceSystem.CheckFencePlacement(gridNode, toPlaceObject.GetComponent<SpriteRenderer>());
                }

                lastGridNode = gridNode;

                CheckGridNode(lastGridNode);

                ChangeObjectGridCellPosition(gridNode);
            }
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && canPlace == true)
            {
                PlaceObject(gridNode);
            }
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                if (item.Name.Contains("Fence"))
                {
                    fenceSystem.Rotate();

                    fenceSystem.CheckFencePlacement(lastGridNode, toPlaceObject.GetComponent<SpriteRenderer>());
                }
            }
        }
    }
}
