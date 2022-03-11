[System.Serializable]
public class GrassSaveGame 
{
    private int objectID;

    private float positionX;
    private float positionY;

    public int ObjectID { get => objectID; set => objectID = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
}
