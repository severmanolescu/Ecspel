[System.Serializable]
public class DialogueAppearSave
{
    private int dialogueID;

    private float positionX;
    private float positionY;

    private int idToAnotherObject;

    public int DialogueID { get => dialogueID; set => dialogueID = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public int IdToAnotherObject { get => idToAnotherObject; set => idToAnotherObject = value; }
}
