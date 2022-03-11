using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcTimeSchedule
{
    [SerializeField] private LocationGridSave locationGrid;

    [Header("Where to go:")]
    [SerializeField] private Transform location;

    [Header("Idle facing: 0 - left; 1 - up; 2 - right; 3 - down")]
    [Range(0, 3)]
    [SerializeField] private int idleDirection;

    [Header("Wait for seconds:")]
    [SerializeField] private int seconds;

    [Header("Wait for specific hour(-1 not wait):")]
    [Range(-1, 23)]
    [SerializeField] private int hours = -1;
    [Range(0, 59)]
    [SerializeField] private int minutes;

    [Header("Move to point:")]
    [SerializeField] private Transform point = null;

    public Transform Location { get => location; set => location = value; }
    public LocationGridSave LocationGrid { get => locationGrid; set => locationGrid = value; }
    public int IdleDirection { get => idleDirection; set => idleDirection = value; }
    public int Seconds { get => seconds; set => seconds = value; }
    public Transform Point { get => point; set => point = value; }
    public int Hours { get => hours; set => hours = value; }
    public int Minutes { get => minutes; set => minutes = value; }
}
