using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIHandler : MonoBehaviour
{
    [SerializeField] private List<NpcTimeSchedule> npcTimeSchedules = new List<NpcTimeSchedule>();

    [SerializeField] private LocationGridSave locationGrid;

    private int scheduleIndex = 0;

    private void Awake()
    {
        scheduleIndex = 0;
    }

    private void Update()
    {
        if(scheduleIndex < npcTimeSchedules.Count)
        {

        }
    }

    public void ChangeLocationGrid(LocationGridSave locationGrid)
    {
        this.locationGrid = locationGrid;
    }

    public void DayChange()
    {
        scheduleIndex = 0;
    }
}
