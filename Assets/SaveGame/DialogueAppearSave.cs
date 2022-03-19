using System.Collections.Generic;

[System.Serializable]
public class DialogueAppearSave
{
    private int dialogueID;

    private float positionX;
    private float positionY;

    private int idToAnotherObject;

    // To set to NPC new path
    private List<NpcPathSave> path;

    private int secondsToDestroy;
    private bool stopPlayerForMoving;

    public int DialogueID { get => dialogueID; set => dialogueID = value; }
    public float PositionX { get => positionX; set => positionX = value; }
    public float PositionY { get => positionY; set => positionY = value; }
    public int IdToAnotherObject { get => idToAnotherObject; set => idToAnotherObject = value; }
    public List<NpcPathSave> Path { get => path; set => path = value; }
    public int SecondsToDestroy { get => secondsToDestroy; set => secondsToDestroy = value; }
    public bool StopPlayerForMoving { get => stopPlayerForMoving; set => stopPlayerForMoving = value; }
}
