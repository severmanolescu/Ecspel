using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointData : MonoBehaviour
{
    [SerializeField] List<WaypointData> nextWaypoints = new List<WaypointData>();

    [Header("Actions:")]
    [SerializeField] private bool blacksmith = false;

    public List<WaypointData> NextWaypoints { get => nextWaypoints; }
    public bool Blacksmith { get => blacksmith; }
}
