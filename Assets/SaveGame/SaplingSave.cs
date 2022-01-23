[System.Serializable]
public class SaplingSave
{
    private int sapling;

    private float positionX;
    private float positionY;

    private int currentSprite;

    private int startDay;

    private ushort state;

    private bool destroyed;

    public int Sapling { get => sapling; set => sapling = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public int CurrentSprite { get => currentSprite; set => currentSprite = value; }
    public int StartDay { get => startDay; set => startDay = value; }
    public ushort State { get => state; set => state = value; }
    public bool Destroyed { get => destroyed; set => destroyed = value; }
}
