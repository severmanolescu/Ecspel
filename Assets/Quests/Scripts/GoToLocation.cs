using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Go To Location", order = 1)]
public class GoToLocation : Quest
{
    [Header("Go to location quest:")]
    public List<Vector3> positions;

    public List<Vector3> Positions { get { return positions; } }
}
