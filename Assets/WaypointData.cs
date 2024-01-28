using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointData : MonoBehaviour
{
    [SerializeField] List<WaypointData> nextWaypoints = new List<WaypointData>();

    [Header("Actions:")]
    [SerializeField] private bool blacksmith = false;
    [SerializeField] private int disapear = 0;
    [SerializeField] private bool wood = false;
    [SerializeField] private string startAnimation;
    [SerializeField] private WaypointData goToWaypoint = null;

    [SerializeField] private ConstructionData construction = null;

    public List<WaypointData> NextWaypoints { get => nextWaypoints; }
    public bool Blacksmith { get => blacksmith; }
    public int Disapear { get => disapear; }
    public bool Wood { get => wood; }
    public WaypointData GoToWaypoint { get => goToWaypoint; }
    public string StartAnimation { get => startAnimation; }
    public ConstructionData Construction { get => construction; }
}
