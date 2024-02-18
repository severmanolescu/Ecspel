using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointData : MonoBehaviour
{
    [SerializeField] List<WaypointData> nextWaypoints = new List<WaypointData>();

    [Header("Actions:")]
    [Header("Blacksmith lcoation:")]
    [SerializeField] private bool blacksmith = false;

    [Header("Construction site:")]
    [SerializeField] private ConstructionData construction = null;
    
    [Header("Construction site:")]
    [SerializeField] private HouseSweeping houseSweeping = null;

    [Header("Disapear at location:")]
    [SerializeField] private int disapear = 0;

    [Header("Get Wood:")]
    [SerializeField] private bool wood = false;

    [Header("Set Aniamtor boolean to true:")]
    [SerializeField] private string startAnimation;

    [Header("Go to waypoint when all the actions are done:")]
    [SerializeField] private WaypointData goToWaypoint = null;

    [Header("Wait for a period of time in a direction:")]
    [SerializeField] private int timeToWait = 0;
    [SerializeField] private Direction directionToWait = 0;

    [Header("Dialogues for when the NPC needs to wait:")]
    [SerializeField] private List<Monolog> monolog;

    public List<WaypointData> NextWaypoints { get => nextWaypoints; }
    public bool Blacksmith { get => blacksmith; }
    public int Disapear { get => disapear; }
    public bool Wood { get => wood; }
    public WaypointData GoToWaypoint { get => goToWaypoint; }
    public string StartAnimation { get => startAnimation; }
    public ConstructionData Construction { get => construction; }
    public int TimeToWait { get => timeToWait; }
    public Direction DirectionToWait { get => directionToWait;}
    public List<Monolog> Monolog { get => monolog; }
    public HouseSweeping HouseSweeping { get => houseSweeping; }
}
