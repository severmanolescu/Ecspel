using UnityEngine;

public class PositionInGridHarvest : MonoBehaviour
{
    [SerializeField] private int startScaleX;
    [SerializeField] private int startScaleY;

    [SerializeField] private int scaleX;
    [SerializeField] private int scaleY;

    [SerializeField] LocationGridSave locationGrid;

    [SerializeField] private bool isWalkable = false;

    [SerializeField] private bool positionInCenterOfGridNode = true;

    public LocationGridSave LocationGrid { get => locationGrid; set => locationGrid = value; }

    private void Update()
    {
        if (locationGrid == null)
        {
            locationGrid = GetComponentInParent<LocationGridSave>();
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
                                grid.gridArray[i, j].crop = gameObject;
                            }
                        }
                    }
                }

                if (isWalkable == false)
                {
                    gameObject.AddComponent<BoxCollider2D>();
                }

                Destroy(this);
            }
        }
    }
}
