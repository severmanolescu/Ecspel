using UnityEngine;
using UnityEngine.InputSystem;

public class test : MonoBehaviour
{
    public LocationGridSave locationGrid;

    public Sprite headlight;

    Vector3 scale = new Vector3(.5f, .5f);

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            for (int i = 0; i < locationGrid.Grid.gridArray.GetLength(0) - 1; i++)
                for (int j = 0; j < locationGrid.Grid.gridArray.GetLength(1) - 1; j++)
                {
                    if (locationGrid.Grid.gridArray[i, j].isWalkable == false)
                    {
                        GameObject test = new GameObject("Node: x:" + i + " y:" + j);

                        test.transform.localScale = scale;

                        test.AddComponent<SpriteRenderer>().color = Color.red;
                        test.GetComponent<SpriteRenderer>().sprite = headlight;

                        Vector3 position = locationGrid.Grid.GetWorldPosition(locationGrid.Grid.gridArray[i, j].x, locationGrid.Grid.gridArray[i, j].y);

                        position.x += locationGrid.Grid.CellSize / 2f;
                        position.y += locationGrid.Grid.CellSize / 2f;

                        test.transform.position = position;
                    }
                }

            this.enabled = false;
        }
    }
}
