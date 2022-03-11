using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTimeChange : MonoBehaviour
{
    private DayTimerHandler dayTimer;

    private List<ClockHandler> clocks = new List<ClockHandler>();

    private void Awake()
    {
        dayTimer = GetComponent<DayTimerHandler>();
    }

    public void AddClock(ClockHandler clock)
    {
        clocks.Add(clock);
    }

    private void Update()
    {
        Quaternion minutes = Quaternion.Euler(new Vector3(0, 0, -dayTimer.Minutes.map(0, 60, 0, 360)));
        Quaternion hours = Quaternion.Euler(new Vector3(0, 0, -(dayTimer.Hours + dayTimer.Minutes / 60f).map(0, 24, 0, 360)));

        foreach (var clock in clocks)
        {
            clock.SetTime(minutes, hours);
        }
    }
}
