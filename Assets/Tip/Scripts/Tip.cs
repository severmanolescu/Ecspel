using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Tip", menuName = "Tip/New Tip", order = 1)]
public class Tip : ScriptableObject
{
    [TextArea(1, 10)]
    public string tipDetails;

    public int tipNo;
}
