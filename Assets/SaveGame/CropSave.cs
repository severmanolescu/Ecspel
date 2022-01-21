[System.Serializable]
public class CropSave
{
    private int cropID;

    private int currentSprite;

    private int startDay;

    private bool destroyed;

    private float positionX;

    private float positionY;

    public CropSave(int cropID, int currentSprite, int startDay, bool destroyed, float positionX, float positionY)
    {
        CropID = cropID;
        CurrentSprite = currentSprite;
        StartDay = startDay;
        Destroyed = destroyed;
        this.PositionX = positionX;
        this.PositionY = positionY;
    }

    public int CropID { get => cropID; set => cropID = value; }
    public int CurrentSprite { get => currentSprite; set => currentSprite = value; }
    public int StartDay { get => startDay; set => startDay = value; }
    public bool Destroyed { get => destroyed; set => destroyed = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
}
