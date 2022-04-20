using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoeSystemHandler : MonoBehaviour
{
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

    private GameObject headlightObject;

    private BuildSystemHandler buildSystem;

    private Grid<GridNode> grid;

    public Grid<GridNode> Grid { set { grid = value; } }

    private void Start()
    {
        headlightObject = null;

        buildSystem = GetComponent<BuildSystemHandler>();
    }

    private GameObject Spawn(GridNode gridNode)
    {
        if(gridNode.isWalkable && gridNode.canPlant == false && gridNode.canPlace == true)
        {
            GameObject soil = Instantiate(prefabGameObject, spawnLocation);

            soil.tag = "FarmPlot";

            SpriteRenderer sprite = soil.AddComponent<SpriteRenderer>();

            sprite.sortingOrder = -2;

            Vector3 position = grid.GetWorldPosition(gridNode.x, gridNode.y);
            Vector3 scale = new Vector3(.75f, .75f, 0);

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

            if (gridNode.x - 1 >= 0)
            {
                ChangeNeighbor(grid.gridArray[gridNode.x - 1, gridNode.y], 3);
            }
            if (gridNode.x + 1 < grid.gridArray.GetLength(0))
            {
                ChangeNeighbor(grid.gridArray[gridNode.x + 1, gridNode.y], 4);
            }
            if (gridNode.y + 1 < grid.gridArray.GetLength(1))
            {
                ChangeNeighbor(grid.gridArray[gridNode.x, gridNode.y + 1], 1);
            }
            if (gridNode.y - 1 >= 0)
            {
                ChangeNeighbor(grid.gridArray[gridNode.x, gridNode.y - 1], 2);
            }

            return soil;
        }
        else if(gridNode.crop != null && gridNode.crop.GetComponent<CropGrow>().Destroyed == true)
        {
            Destroy(gridNode.crop);
            gridNode.crop = null;
            gridNode.isWalkable = true;
            gridNode.canPlant = true;
            gridNode.canPlace = false;
            gridNode.cropPlaced = false;

            return null;
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
        GridNode gridNode = grid.GetGridObject(position);

        if (gridNode != null)
        {
            gridNode.isWalkable = true;
            gridNode.canPlant = false;
            gridNode.canPlace = true;
            gridNode.cropPlaced = false;
            gridNode.currentObject = null;
        }
    }

    //1-up
    //2-down
    //3-left
    //4-right
   
    private void ChangeNeighbor(GridNode gridNode, int type)
    {
        if (gridNode.currentObject != null)
        {
            if(gridNode.currentObject.sprite == full)
            {
                switch(type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(up, upWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(down, downWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(left, leftWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(right, rightWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == up)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(updown, updownWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftUp, cornerLeftUpWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightUp, cornerRightUpWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == down)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(updown, updownWater); ; break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftDown, cornerLeftDownWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightDown, cornerRightDownWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == right)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightUp, cornerRightUpWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerRightDown, cornerRightDownWater);  break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(leftright, leftrightWater); ; break;
                }
            }
            else if (gridNode.currentObject.sprite == left)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftUp, cornerLeftUpWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(cornerLeftDown, cornerLeftDown); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(leftright, leftrightWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == updown)
            {
                switch (type)
                {
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft, centerLeftWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight, centerRightWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == leftright)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerUp, centerUpWater); break;
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown, centerDownWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerLeft)
            {
                switch (type)
                {
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerRight)
            {
                switch (type)
                {
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerDown)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == centerUp)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(center, centerWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerLeftUp)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft, centerLeftWater); break;
                    case 4: gridNode.currentObject.sprite = centerUp; break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerLeftDown)
            {
                switch (type)
                {
                    case 1: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerLeft, centerLeftWater); break;
                    case 4: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerDown, centerDownWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerRightUp)
            {
                switch (type)
                {
                    case 2: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerRight, centerRightWater); break;
                    case 3: gridNode.currentObject.GetComponent<FarmPlotHandler>().ChangeSprites(centerUp, centerUpWater); break;
                }
            }
            else if (gridNode.currentObject.sprite == cornerRightDown)
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
        if(gridNode.x + 1 < grid.gridArray.GetLength(0) && grid.gridArray[gridNode.x + 1, gridNode.y].canPlant == true)
        {
            if (gridNode.y + 1 < grid.gridArray.GetLength(1) && grid.gridArray[gridNode.x, gridNode.y + 1].canPlant == true)
            {
                if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
                {
                    if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                    {
                        sprite.sprite = center;

                        farmPlot.WetSoil = centerWater;
                        farmPlot.DrySoil = center;
                    }
                    else
                    {
                        sprite.sprite = centerDown;

                        farmPlot.WetSoil = centerDownWater;
                        farmPlot.DrySoil = centerDown;
                    }
                }
                else
                {
                    if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                    {
                        sprite.sprite = centerLeft;

                        farmPlot.WetSoil = centerLeftWater;
                        farmPlot.DrySoil = centerLeft;
                    }
                    else
                    {
                        sprite.sprite = cornerLeftDown;

                        farmPlot.WetSoil = cornerLeftDownWater;
                        farmPlot.DrySoil = cornerLeftDown;
                    }
                }
            }
            else if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
            {
                if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                {
                    sprite.sprite = centerUp;

                    farmPlot.WetSoil = centerUpWater;
                    farmPlot.DrySoil = centerUp;
                }
                else
                {
                    sprite.sprite = leftright;

                    farmPlot.WetSoil = leftrightWater;
                    farmPlot.DrySoil = leftright;
                }
            }
            else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                sprite.sprite = cornerLeftUp;

                farmPlot.WetSoil = cornerLeftUpWater;
                farmPlot.DrySoil = cornerLeftUp;
            }
            else
            {
                sprite.sprite = left;

                farmPlot.WetSoil = leftWater;
                farmPlot.DrySoil = left;
            }
        }
        else if (gridNode.y + 1 < grid.gridArray.GetLength(1) && grid.gridArray[gridNode.x, gridNode.y + 1].canPlant == true)
        {
            if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
            {
                if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
                {
                    sprite.sprite = centerRight;

                    farmPlot.WetSoil = centerRightWater;
                    farmPlot.DrySoil = centerRight;
                }
                else
                {
                    sprite.sprite = cornerRightDown;

                    farmPlot.WetSoil = cornerRightDownWater;
                    farmPlot.DrySoil = cornerRightDown;
                }
            }
            else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                sprite.sprite = updown;

                farmPlot.WetSoil = updownWater;
                farmPlot.DrySoil = updown;
            }
            else
            {
                sprite.sprite = down;

                farmPlot.WetSoil = downWater;
                farmPlot.DrySoil = down;
            }
        }
        else if (gridNode.x - 1 >= 0 && grid.gridArray[gridNode.x - 1, gridNode.y].canPlant == true)
        {
            if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
            {
                sprite.sprite = cornerRightUp;

                farmPlot.WetSoil = cornerRightUpWater;
                farmPlot.DrySoil = cornerRightUp;
            }
            else
            {
                sprite.sprite = right;

                farmPlot.WetSoil = rightWater;
                farmPlot.DrySoil = right;
            }
        }
        else if (gridNode.y - 1 >= 0 && grid.gridArray[gridNode.x, gridNode.y - 1].canPlant == true)
        {
            sprite.sprite = up;

            farmPlot.WetSoil = upWater;
            farmPlot.DrySoil = up;
        }
        else
        {
            sprite.sprite = full;

            farmPlot.WetSoil = fullWater;
            farmPlot.DrySoil = full;
        }
    }

    public void PlaceSoil(Vector3 playerPosition, int spawn, Hoe hoe)
    {
        if (buildSystem.canPlantGrid == true)
        {
            GridNode gridNode = grid.GetGridObject(playerPosition);

            if (gridNode != null)
            {
                switch (spawn)
                {
                    case 1: if (gridNode.x - 1 >= 0) Spawn(grid.gridArray[gridNode.x - 1, gridNode.y]); break;
                    case 2: if (gridNode.x + 1 < grid.gridArray.GetLength(0)) Spawn(grid.gridArray[gridNode.x + 1, gridNode.y]); break;
                    case 3: if (gridNode.y + 1 < grid.gridArray.GetLength(1)) Spawn(grid.gridArray[gridNode.x, gridNode.y + 1]); break;
                    case 4: if (gridNode.y - 1 >= 0) Spawn(grid.gridArray[gridNode.x, gridNode.y - 1]); break;
                }
            }

            GameObject.Find("Global/Player/Canvas/Field/QuickSlots/Stats").GetComponent<PlayerStats>().DecreseStamina(hoe.Stamina);
            //1 - Left
            //2 - Right
            //3 - Up
            //4 - Down
        }
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

    public void HoeHeadlight(Vector3 playerPosition, int spawn)
    {
        if (buildSystem.LocationGrid != null && buildSystem.canPlantGrid == true)
        {
            GridNode gridNode = grid.GetGridObject(playerPosition);

            if (gridNode != null)
            {
                if (headlightObject == null)
                {
                    headlightObject = Instantiate(prefabGameObject);

                    headlightObject.AddComponent<SpriteRenderer>().sprite = headlight;

                    Vector3 scale = new Vector3(.75f, .75f, 1f);

                    headlightObject.transform.localScale = scale;
                }

                switch (spawn)
                {
                    case 1:
                        {
                            if (gridNode.x - 1 >= 0)
                            {
                                ChangeHeadlightPosition(grid.gridArray[gridNode.x - 1, gridNode.y]);
                            }
                            else
                            {
                                Destroy(headlightObject);
                            }

                            break;
                        }
                    case 2:
                        {
                            if (gridNode.x + 1 < grid.gridArray.GetLength(0))
                            {
                                ChangeHeadlightPosition(grid.gridArray[gridNode.x + 1, gridNode.y]);
                            }
                            else
                            {
                                Destroy(headlightObject);
                            }

                            break;
                        }
                    case 3:
                        {
                            if (gridNode.y + 1 < grid.gridArray.GetLength(1))
                            {
                                ChangeHeadlightPosition(grid.gridArray[gridNode.x, gridNode.y + 1]);
                            }
                            else
                            {
                                Destroy(headlightObject);
                            }

                            break;
                        }
                    case 4:
                        {
                            if (gridNode.y - 1 >= 0)
                            {
                                ChangeHeadlightPosition(grid.gridArray[gridNode.x, gridNode.y - 1]);
                            }
                            else
                            {
                                Destroy(headlightObject);
                            }

                            break;
                        }
                }
            }
        }
    }

    public void StopHeadlight()
    {
        if (headlightObject != null)
        {
            Destroy(headlightObject);
        }
    }
}
