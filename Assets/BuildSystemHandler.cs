using UnityEngine;
using UnityEditor;

public class BuildSystemHandler : MonoBehaviour
{
    private Grid<GridNode> grid;

    private Item item;

    private bool startPlace = false;

    private GameObject @object;

    private GameObject prefabGameObject;

    private QuickSlotsChanger quickSlots;

    private bool canPlace = true;

    public Grid<GridNode> Grig { get { return grid; } }

    private void Start()
    {
        grid = new Grid<GridNode>(65, 80, .75f, new Vector3(-14f, -3f, 0f), (Grid < GridNode > g, int x, int y) => new GridNode(g, x, y));

        GetComponent<HoeSystemHandler>().Grid = grid;

        prefabGameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameObject.prefab", typeof(GameObject));
    }

    public void ChangeGridCropPlaced(GridNode gridNode, bool cropPlaced)
    {
        grid.gridArray[gridNode.x, gridNode.y].cropPlaced = canPlace;
    }

    public void SetQuickSlotHandler(QuickSlotsChanger quickSlots)
    {
        this.quickSlots = quickSlots;
    }

    public void StartPlace(Item item)
    {
        this.item = item;

        startPlace = true;

        if (@object == null)
        {
            @object = Instantiate(prefabGameObject);

            @object.AddComponent<SpriteRenderer>().sortingOrder = 1;

            if (item is Crop)
            {
                Crop crop = (Crop)item;

                @object.GetComponent<SpriteRenderer>().sprite = crop.levels[0];
            }
            else
            {
                @object.GetComponent<SpriteRenderer>().sprite = item.Sprite;
            }
        }
        else
        {
            if (item is Crop)
            {
                Crop crop = (Crop)item;

                @object.GetComponent<SpriteRenderer>().sprite = crop.levels[0];
            }
            else
            {
                @object.GetComponent<SpriteRenderer>().sprite = item.Sprite;
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

    private void PlaceObject(GridNode gridNode)
    {
        GameObject newObject = Instantiate(@object);

        newObject.GetComponent<SpriteRenderer>().sortingOrder = 0;

        if(item is Crop)
        {
            newObject.AddComponent<CropGrow>().SetItem(item, gridNode);

            newObject.tag = "Crop";

            newObject.GetComponent<SpriteRenderer>().sortingOrder = -1;

            gridNode.crop = newObject;
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
    }

    private bool VerifyCanPlace(GridNode gridNode)
    {
        Placeable placeable = (Placeable)item;

        if (gridNode.x + placeable.sizeX <= grid.gridArray.GetLength(0) &&
            gridNode.y + placeable.sizeY <= grid.gridArray.GetLength(1))
        {
            for (int i = gridNode.x; i < gridNode.x + placeable.sizeX; i++)
            {
                for (int j = gridNode.y; j < gridNode.y + placeable.sizeY; j++)
                {
                    if (grid.gridArray[i, j].canPlace == false)
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

        for (int i = gridNode.x; i < gridNode.x + placeable.sizeX; i++)
        {
            for (int j = gridNode.y; j < gridNode.y + placeable.sizeY; j++)
            {
                if (item is Crop)
                {
                    grid.gridArray[gridNode.x, gridNode.y].cropPlaced = true;
                    grid.gridArray[gridNode.x, gridNode.y].canPlant = true;
                    grid.gridArray[gridNode.x, gridNode.y].canPlace = false;
                }
                else
                {
                    grid.gridArray[i, j].canPlace = false;
                    grid.gridArray[i, j].canPlant = false;
                    grid.gridArray[i, j].cropPlaced = false;
                }
            }
        }
    }

    private void Update()
    {
        if(startPlace)
        {
            GridNode gridNode = grid.GetGridObject(Camera.main.ScreenToWorldPoint(Input.mousePosition));

            if (gridNode != null)
            {
                if((canPlace = VerifyCanPlace(gridNode)) == true && !(item is Crop))
                {
                    @object.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    if (item is Crop && grid.gridArray[gridNode.x, gridNode.y].cropPlaced == false && grid.gridArray[gridNode.x, gridNode.y].canPlant == true)
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

                Vector3 position = grid.GetWorldPosition(gridNode.x, gridNode.y);


                if (item is Crop)
                {
                    Crop aux = (Crop)item;

                    if(aux.centerX == true)
                    {
                        position.x += grid.CellSize / 2f;
                    }
                    if (aux.centerY == true)
                    {
                        position.y += grid.CellSize / 2f;
                    }
                }
                else
                {
                    position.x += grid.CellSize / 2f;
                    position.y += grid.CellSize / 2f;
                }

                position.z = 0;

                @object.transform.position = position; 

                if(Input.GetMouseButtonDown(0) && canPlace == true)
                {
                    PlaceObject(gridNode);
                }
            }
        }
    }
}
