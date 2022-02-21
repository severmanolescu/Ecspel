using System;
using System.Collections.Generic;

[System.Serializable]
public class ForgeStorage
{
    private float positionX;
    private float positionY;

    private int id;

    private List<Tuple<int, int>> storage = new List<Tuple<int, int>>();

    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public List<Tuple<int, int>> Storage { get => storage; set => storage = value; }
    public int Id { get => id; set => id = value; }
}
