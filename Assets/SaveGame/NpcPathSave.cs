[System.Serializable]
public class NpcPathSave
{
    private float locationX;
    private float locationY;

    private int locationID;
    private int idleDirection;
    private int waitForSeconds;
    private int waitForHour;
    private int waitForMinute;

    public NpcPathSave(float locationX, float locationY, int locationID, int idleDirection, int waitForSeconds, int waitForHour, int waitForMinute)
    {
        this.locationX = locationX;
        this.locationY = locationY;
        this.locationID = locationID;
        this.idleDirection = idleDirection;
        this.waitForSeconds = waitForSeconds;
        this.waitForHour = waitForHour;
        this.waitForMinute = waitForMinute;
    }

    public float LocationX { get => locationX; set => locationX = value; }
    public float LocationY { get => locationY; set => locationY = value; }
    public int LocationID { get => locationID; set => locationID = value; }
    public int IdleDirection { get => idleDirection; set => idleDirection = value; }
    public int WaitForSeconds { get => waitForSeconds; set => waitForSeconds = value; }
    public int WaitForHour { get => waitForHour; set => waitForHour = value; }
    public int WaitForMinute { get => waitForMinute; set => waitForMinute = value; }
}
