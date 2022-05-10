[System.Serializable]
public class FarmPlotSave 
{
    private float positionX;
    private float positionY;

    private int noOfDryDays;

    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public int NoOfDryDays { get => noOfDryDays; set => noOfDryDays = value; }
}
