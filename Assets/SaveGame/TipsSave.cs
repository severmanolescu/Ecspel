[System.Serializable]
public class TipsSave
{
    private int tipID;

    private float positionX;
    private float positionY;

    private float colliderSizeX;
    private float colliderSizeY;

    public int TipID { get => tipID; set => tipID = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public float ColliderSizeX { get => colliderSizeX; set => colliderSizeX = value; }
    public float ColliderSizeY { get => colliderSizeY; set => colliderSizeY = value; }
}
