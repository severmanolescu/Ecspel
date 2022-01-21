using System;
using System.Collections.Generic;

[Serializable]
public class ChestSave
{
    private int chestID;

    private float positionX;

    private float positionY;

    private List<Tuple<int, int>> items;

    public int ChestID { get => chestID; set => chestID = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public List<Tuple<int, int>> Items { get => items; set => items = value; }
}
