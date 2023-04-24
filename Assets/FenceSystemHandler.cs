using UnityEngine;

public class FenceSystemHandler : MonoBehaviour
{
    [SerializeField] private Sprite fence;

    [Header("Sprites for up facing fence")]
    [SerializeField] private Sprite fenceUp_Left;
    [SerializeField] private Sprite fenceUp_Right;
    [SerializeField] private Sprite fenceUp_Up;
    [SerializeField] private Sprite fenceUp_Down;

    [SerializeField] private Sprite fenceUp_Left_Down;
    [SerializeField] private Sprite fenceUp_Right_Down;

    [SerializeField] private Sprite fenceUp_Left_Up;
    [SerializeField] private Sprite fenceUp_Right_Up;

    [SerializeField] private Sprite fenceUp_Left_Right;
    [SerializeField] private Sprite fenceUp_Left_Right_Down;
    [SerializeField] private Sprite fenceUp_Left_Right_Up;

    [Header("Sprites for down facing fence")]
    [SerializeField] private Sprite fenceDown_Left;
    [SerializeField] private Sprite fenceDown_Right;
    [SerializeField] private Sprite fenceDown_Up;
    [SerializeField] private Sprite fenceDown_Down;

    [SerializeField] private Sprite fenceDown_Left_Down;
    [SerializeField] private Sprite fenceDown_Right_Down;

    [SerializeField] private Sprite fenceDown_Left_Up;
    [SerializeField] private Sprite fenceDown_Right_Up;

    [SerializeField] private Sprite fenceDown_Left_Right;
    [SerializeField] private Sprite fenceDown_Left_Right_Down;
    [SerializeField] private Sprite fenceDown_Left_Right_Up;

    private Grid grid;

    private GridNode initialNode = null;

    //
    // Rotation:
    // false - Up
    // true - Down
    private bool rotation = true;

    public Grid Grid { set => grid = value; }

    private void CheckForNeighbors(GridNode node, out bool hasNeighborInLeft, out bool hasNeighborInRight, out bool hasNeighborInUp, out bool hasNeighborInDown)
    {
        hasNeighborInLeft = false;
        hasNeighborInRight = false;
        hasNeighborInUp = false;
        hasNeighborInDown = false;

        if (node.x - 1 >= 0)
        {
            if ((grid.gridArray[node.x - 1, node.y].objectInSpace != null && grid.gridArray[node.x - 1, node.y].objectInSpace.CompareTag("Fence"))
                || grid.gridArray[node.x - 1, node.y] == initialNode)
            {
                hasNeighborInLeft = true;
            }
        }
        if (node.x + 1 < grid.Width)
        {
            if ((grid.gridArray[node.x + 1, node.y].objectInSpace != null && grid.gridArray[node.x + 1, node.y].objectInSpace.CompareTag("Fence"))
                || grid.gridArray[node.x + 1, node.y] == initialNode)
            {
                hasNeighborInRight = true;
            }
        }
        if (node.y - 1 >= 0)
        {
            if ((grid.gridArray[node.x, node.y - 1].objectInSpace != null && grid.gridArray[node.x, node.y - 1].objectInSpace.CompareTag("Fence"))
                || grid.gridArray[node.x, node.y - 1] == initialNode)
            {
                hasNeighborInDown = true;
            }
        }
        if (node.y + 1 < grid.Height)
        {
            if ((grid.gridArray[node.x, node.y + 1].objectInSpace != null && grid.gridArray[node.x, node.y + 1].objectInSpace.CompareTag("Fence"))
                || grid.gridArray[node.x, node.y + 1] == initialNode)
            {
                hasNeighborInUp = true;
            }
        }
    }

    private void PlaceRightSprite(SpriteRenderer objectSprite, Sprite up, Sprite down)
    {
        if (rotation)
        {
            objectSprite.sprite = down;
        }
        else
        {
            objectSprite.sprite = up;
        }
    }

    private void ChangeFenceSprite(GridNode fence, SpriteRenderer fenceSprite = null)
    {
        if ((fence.objectInSpace != null && fence.objectInSpace.CompareTag("Fence")) ||
            fenceSprite != null)
        {
            CheckForNeighbors(fence,
                out bool hasNeighborInLeft,
                out bool hasNeighborInRight,
                out bool hasNeighborInUp,
                out bool hasNeighborInDown);

            if (fenceSprite == null)
            {
                fenceSprite = fence.objectInSpace.GetComponent<SpriteRenderer>();
            }

            if (hasNeighborInLeft)
            {
                if (hasNeighborInRight)
                {
                    if (hasNeighborInDown)
                    {
                        PlaceRightSprite(fenceSprite, fenceUp_Left_Right_Down, fenceDown_Left_Right_Down);
                    }
                    else if (hasNeighborInUp)
                    {
                        fenceSprite.sprite = fenceUp_Left_Right_Up;
                        PlaceRightSprite(fenceSprite, fenceUp_Left_Right_Up, fenceDown_Left_Right_Up);
                    }
                    else
                    {
                        fenceSprite.sprite = fenceUp_Left_Right;
                        PlaceRightSprite(fenceSprite, fenceUp_Left_Right, fenceDown_Left_Right);
                    }
                }
                else
                {
                    if (hasNeighborInDown)
                    {
                        fenceSprite.sprite = fenceUp_Left_Down;
                        PlaceRightSprite(fenceSprite, fenceUp_Left_Down, fenceDown_Left_Down);
                    }
                    else if (hasNeighborInUp)
                    {
                        fenceSprite.sprite = fenceUp_Left_Up;
                        PlaceRightSprite(fenceSprite, fenceUp_Left_Up, fenceDown_Left_Up);
                    }
                    else
                    {
                        fenceSprite.sprite = fenceUp_Left;
                        PlaceRightSprite(fenceSprite, fenceUp_Left, fenceDown_Left);
                    }
                }
            }
            else if (hasNeighborInRight)
            {
                if (hasNeighborInDown)
                {
                    fenceSprite.sprite = fenceUp_Right_Down;
                    PlaceRightSprite(fenceSprite, fenceUp_Right_Down, fenceDown_Right_Down);
                }
                else if (hasNeighborInUp)
                {
                    fenceSprite.sprite = fenceUp_Right_Up;
                    PlaceRightSprite(fenceSprite, fenceUp_Right_Up, fenceDown_Right_Up);
                }
                else
                {
                    fenceSprite.sprite = fenceUp_Right;
                    PlaceRightSprite(fenceSprite, fenceUp_Right, fenceDown_Right);
                }
            }
            else if (hasNeighborInUp)
            {
                if (hasNeighborInDown)
                {
                    fenceSprite.sprite = fenceUp_Down;
                    PlaceRightSprite(fenceSprite, fenceUp_Down, fenceDown_Down);
                }
                else
                {
                    fenceSprite.sprite = fenceUp_Up;
                    PlaceRightSprite(fenceSprite, fenceUp_Up, fenceDown_Up);
                }
            }
            else if (hasNeighborInDown)
            {
                fenceSprite.sprite = fenceUp_Down;
                PlaceRightSprite(fenceSprite, fenceUp_Down, fenceDown_Down);
            }
            else
            {
                fenceSprite.sprite = this.fence;
                PlaceRightSprite(fenceSprite, this.fence, this.fence);
            }
        }
    }

    public void CheckFencePlacement(GridNode fence, SpriteRenderer fenceSprite = null)
    {
        if (grid != null && fence != null)
        {
            if (fenceSprite != null)
            {
                initialNode = fence;
            }

            ChangeFenceSprite(fence, fenceSprite);

            if (fence.x - 1 >= 0)
            {
                ChangeFenceSprite(grid.gridArray[fence.x - 1, fence.y]);
            }
            if (fence.x + 1 < grid.Width)
            {
                ChangeFenceSprite(grid.gridArray[fence.x + 1, fence.y]);
            }
            if (fence.y - 1 >= 0)
            {
                ChangeFenceSprite(grid.gridArray[fence.x, fence.y - 1]);
            }
            if (fence.y + 1 < grid.Height)
            {
                ChangeFenceSprite(grid.gridArray[fence.x, fence.y + 1]);
            }

            initialNode = null;
        }
    }

    public void Rotate()
    {
        rotation = !rotation;
    }
}
