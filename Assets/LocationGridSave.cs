using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGridSave : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int weight;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 position = new Vector3();

    [SerializeField] private bool canPlantToGrid;

    private Grid<GridNode> grid;

    public Grid<GridNode> Grid { get { return grid; } }

    public bool CanPlantToGrid { get => canPlantToGrid; set => canPlantToGrid = value; }

    private void Awake()
    {
        grid = new Grid<GridNode>(height, weight, cellSize, position, (Grid<GridNode> g, int x, int y) => new GridNode(g, x, y));
    }
}
