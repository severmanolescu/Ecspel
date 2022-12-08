using UnityEngine;

public class PlaceableDataSave : MonoBehaviour
{
    [Header("If item sprite is to big change below variable to something alright")]
    [Range(.01f, .7f)]
    [SerializeField] private float itemWorldSize = .7f;

    private Placeable placeable;

    public Placeable Placeable { get => placeable; set => placeable = value; }
    public float ItemWorldSize { get => itemWorldSize; }
}
