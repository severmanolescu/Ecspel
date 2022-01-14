using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIHandler : MonoBehaviour
{
    [SerializeField] private List<NpcTimeSchedule> npcTimeSchedules = new List<NpcTimeSchedule>();

    private NpcPathFinding npcPath;

    private int scheduleIndex = 0;

    private void Awake()
    {
        scheduleIndex = 0;

        npcPath = GetComponent<NpcPathFinding>();
    }

    private void Start()
    {
        if (scheduleIndex < npcTimeSchedules.Count)
        {
            npcPath.ChangeLocation(npcTimeSchedules[scheduleIndex].LocationGrid,
                                   npcTimeSchedules[scheduleIndex].Location.position);
        }

        npcPath.CanWalk = true;
    }

    private IEnumerator WaitForSeconds(int seconds)
    {
        npcPath.CanWalk = false;

        if (npcTimeSchedules[scheduleIndex].Point.position != transform.position)
        {
            Vector3 currentPosition = transform.position;

            transform.position = Vector3.MoveTowards(transform.position, npcTimeSchedules[scheduleIndex].Point.position, 5f);

            npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex].IdleDirection);

            yield return new WaitForSeconds(seconds);

            transform.position = Vector3.MoveTowards(transform.position, currentPosition, 5f);
        }
        else
        {
            yield return new WaitForSeconds(seconds);
        }

        ChangeScheduleIndex();

        npcPath.CanWalk = true;
    }

    private void ChangeScheduleIndex()
    {
        scheduleIndex++;

        if (scheduleIndex < npcTimeSchedules.Count)
        {
            npcPath.ChangeLocation(npcTimeSchedules[scheduleIndex].LocationGrid,
                                    npcTimeSchedules[scheduleIndex].Location.position);
        }
        else
        {
            npcPath.CanWalk = false;

            npcPath.MoveIdleAnimation(npcTimeSchedules[scheduleIndex - 1].IdleDirection);
        }
    }

    public void ArrivedAtLocation()
    {
        if (npcTimeSchedules[scheduleIndex].Seconds != 0)
        {
            StartCoroutine(WaitForSeconds(npcTimeSchedules[scheduleIndex].Seconds));
        }
        else
        {
            ChangeScheduleIndex();
        }
    }

    public void LocationChange()
    {
        ArrivedAtLocation();
    }

    public void DayChange()
    {
        scheduleIndex = 0;
    }
}
