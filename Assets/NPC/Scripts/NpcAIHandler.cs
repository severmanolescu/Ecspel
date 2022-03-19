using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIHandler : MonoBehaviour
{
    [SerializeField] private List<NpcTimeSchedule> npcTimeSchedules = new List<NpcTimeSchedule>();

    private DayTimerHandler dayTimerHandler;

    private NpcPathFinding npcPath;

    public int scheduleIndex = 0;

    public int ScheduleIndex { get => scheduleIndex; set => scheduleIndex = value; }
    public List<NpcTimeSchedule> NpcTimeSchedules { get => npcTimeSchedules; set => npcTimeSchedules = value; }
    public NpcPathFinding NpcPath { get => npcPath; set => npcPath = value; }

    private void Awake()
    {
        scheduleIndex = 0;

        npcPath = GetComponent<NpcPathFinding>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    public void GetNpcPath()
    {
        npcPath = GetComponent<NpcPathFinding>();
    }

    private void Start()
    {
        if (scheduleIndex < npcTimeSchedules.Count)
        {
            if (npcTimeSchedules[scheduleIndex].Location == transform)
            {
                StartCoroutine(WaitForSeconds(npcTimeSchedules[scheduleIndex].Seconds));
            }
            else
            {
                npcPath.ChangeLocation(npcTimeSchedules[scheduleIndex].LocationGrid,
                                    npcTimeSchedules[scheduleIndex].Location.position);
            }
        }
        else
        {
            npcPath.CanWalk = false;

            if (npcTimeSchedules.Count > 0)
            {
                npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex - 1].IdleDirection);
            }
        }

        npcPath.CanWalk = true;
    }

    private IEnumerator WaitForSeconds(int seconds)
    {
        npcPath.CanWalk = false;

        if (scheduleIndex < npcTimeSchedules.Count && 
            npcTimeSchedules[scheduleIndex].Point.position != transform.position)
        {
            npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex].IdleDirection);

            yield return new WaitForSeconds(seconds);

            transform.position = Vector3.MoveTowards(transform.position, npcTimeSchedules[scheduleIndex].Point.position, 5f);
        }
        else
        {
            npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex].IdleDirection);

            yield return new WaitForSeconds(seconds);
        }

        ChangeScheduleIndex();

        npcPath.CanWalk = true;
    }

    private IEnumerator WaitForHour()
    {
        npcPath.CanWalk = false;

        npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex].IdleDirection);

        while (dayTimerHandler.Hours < npcTimeSchedules[scheduleIndex].Hours ||
             (dayTimerHandler.Hours == npcTimeSchedules[scheduleIndex].Hours &&
              dayTimerHandler.Minutes <= npcTimeSchedules[scheduleIndex].Minutes))
        {
            yield return new WaitForSeconds(2);
        }

        ChangeScheduleIndex();

        npcPath.CanWalk= true;
    }

    public void ChangeScheduleIndex()
    {
        scheduleIndex++;
        
        if (scheduleIndex < npcTimeSchedules.Count)
        {
            if (npcTimeSchedules[scheduleIndex].Location == transform)
            {
                StartCoroutine(WaitForSeconds(npcTimeSchedules[scheduleIndex].Seconds));
            }
            else
            {
                npcPath.ChangeLocation(npcTimeSchedules[scheduleIndex].LocationGrid,
                                    npcTimeSchedules[scheduleIndex].Location.position);
            }
        }
        else
        {
            npcPath.CanWalk = false;
        }
    }

    public void ArrivedAtLocation()
    {
        if (npcPath.CanWalk == true && scheduleIndex < npcTimeSchedules.Count && npcTimeSchedules.Count > 0)
        {
            if (npcTimeSchedules[scheduleIndex].Hours > -1)
            {
                StartCoroutine(WaitForHour());
            }
            else if(npcTimeSchedules[scheduleIndex].Location == transform)
            {
                if (npcTimeSchedules[scheduleIndex].Seconds != 0)
                {
                    StartCoroutine(WaitForSeconds(npcTimeSchedules[scheduleIndex].Seconds));
                }
            }
            else if (npcTimeSchedules[scheduleIndex].Seconds != 0)
            {
                StartCoroutine(WaitForSeconds(npcTimeSchedules[scheduleIndex].Seconds));
            }
            else
            {
                ChangeScheduleIndex();
            }
        }
        else
        {
            npcPath.CanWalk = false;
            if (scheduleIndex < npcTimeSchedules.Count && 
                npcTimeSchedules[scheduleIndex].Hours > -1)
            {
                StartCoroutine(WaitForHour());
            }
            else if (scheduleIndex - 1 < npcTimeSchedules.Count && npcTimeSchedules.Count > 0)
            {
                npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex - 1].IdleDirection);
            }
        }
    }

    public void LocationChange()
    {
        ArrivedAtLocation();
    }

    public void DayChange()
    {
        scheduleIndex = -1;

        StopAllCoroutines();

        npcPath.CanWalk = true;

        ChangeScheduleIndex();
    }
}
