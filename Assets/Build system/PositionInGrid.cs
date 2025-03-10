using UnityEngine;

public class PositionInGrid : MonoBehaviour
{
    [SerializeField] private int startScaleX;
    [SerializeField] private int startScaleY;

    [SerializeField] private int scaleX;
    [SerializeField] private int scaleY;

    [SerializeField] private bool isWalkable = false;

    [SerializeField] private bool positionInCenterOfGridNode = true;

    [SerializeField] private bool setObjectOnlyToMiddleGridNode = false;
    
    private LocationGridSave locationGrid;

    public bool inDrawDistance = false;

    public LocationGridSave LocationGrid { get => locationGrid; set => locationGrid = value; }
    public bool InDrawDistance { set => inDrawDistance = value; }

    private void Update()
    {
        if (locationGrid == null)
        {
            locationGrid = GetComponentInParent<LocationGridSave>();

            if (locationGrid == null)
            {
                locationGrid = GameObject.Find("AI_Grid").GetComponent<LocationGridSave>();
            }
        }

        if (locationGrid != null)
        {
            Grid grid = LocationGrid.Grid;

            GridNode gridNode = grid.GetGridObject(transform.position);

            if (gridNode != null)
            {
                Vector3 position = grid.GetWorldPosition(gridNode.x, gridNode.y);

                if (positionInCenterOfGridNode == true)
                {
                    position.y += grid.CellSize / 2;

                    transform.position = position;
                }

                position.x += grid.CellSize / 2;
                position.z = 0;

                transform.position = position;

                for (int i = gridNode.x + startScaleX; i <= gridNode.x + scaleX; i++)
                {
                    for (int j = gridNode.y + startScaleY; j <= gridNode.y + scaleY; j++)
                    {
                        if (i < grid.gridArray.GetLength(0) && j < grid.gridArray.GetLength(1))
                        {
                            if (i >= 0 && i < locationGrid.Grid.gridArray.GetLength(0) &&
                                j >= 0 && j < locationGrid.Grid.gridArray.GetLength(1))
                            {
                                grid.gridArray[i, j].canPlace = false;
                                grid.gridArray[i, j].canPlant = false;
                                grid.gridArray[i, j].isWalkable = isWalkable;

                                if (setObjectOnlyToMiddleGridNode == false)
                                {
                                    grid.gridArray[i, j].objectInSpace = gameObject;
                                }
                            }
                        }
                    }
                }

                if (setObjectOnlyToMiddleGridNode == true)
                {
                    grid.gridArray[gridNode.x, gridNode.y].objectInSpace = gameObject;
                }

                DamageTree damageTree = GetComponent<DamageTree>();

                if (damageTree != null)
                {
                    damageTree.GetDataFromPosition(startScaleX, startScaleY, scaleX, scaleY);
                }
                else
                {
                    StoneDamage stoneDamage = GetComponent<StoneDamage>();

                    if (stoneDamage != null)
                    {
                        stoneDamage.GetDataFromPosition(startScaleX, startScaleY, scaleX, scaleY);
                    }
                }

            }
        }

        if(!inDrawDistance)
        {
            gameObject.SetActive(false);
        }

        Destroy(this);
    }
}
