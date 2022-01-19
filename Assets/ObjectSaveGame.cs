[System.Serializable]
public class ObjectSaveGame
{
    private int itemNo;

    private float positionX;
    private float positionY;

    private int areaIndex;

    private int locationIndex;

    public ObjectSaveGame(int itemNo, float positionX, float positionY, int areaIndex, int locationIndex)
    {
        this.itemNo = itemNo;
        this.positionX = positionX;
        this.positionY = positionY;
        this.areaIndex = areaIndex;
        this.locationIndex = locationIndex;
    }

    public int ItemNo { get => itemNo; set => itemNo = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public int AreaIndex { get => areaIndex; set => areaIndex = value; }
    public int LocationIndex { get => locationIndex; set => locationIndex = value; }
}
