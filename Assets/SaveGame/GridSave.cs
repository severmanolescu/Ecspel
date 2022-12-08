[System.Serializable]
public class GridSave
{
    private bool isWalkable;
    private bool canPlant;
    private bool canPlace;
    private bool cropPlaced;

    public bool IsWalkable { get => isWalkable; set => isWalkable = value; }
    public bool CanPlant { get => canPlant; set => canPlant = value; }
    public bool CanPlace { get => canPlace; set => canPlace = value; }
    public bool CropPlaced { get => cropPlaced; set => cropPlaced = value; }
}

