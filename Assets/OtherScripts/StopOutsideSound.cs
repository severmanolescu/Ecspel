using UnityEngine;

public class StopOutsideSound : MonoBehaviour
{
    private DayTimerHandler dayTimerHandler;

    private void Awake()
    {
        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (dayTimerHandler != null && collision.CompareTag("Player"))
        {
            dayTimerHandler.StopSoundEffects();
        }
    }
}
