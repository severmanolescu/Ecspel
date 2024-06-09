using UnityEngine;

public class EventEndOfDay : Event
{
    [SerializeField] private BoxCollider2D cantGoBack;

    [SerializeField] private float dayTimeSpeed = 2f;
    [SerializeField] private int endHour = 20;

    private DayTimerHandler dayTimerHandler;

    private bool started = false;

    private void Awake()
    {
        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision != null && 
            collision.CompareTag("Player") && 
            canTrigger)
        {
            cantGoBack.isTrigger = false;

            dayTimerHandler.TimeSpeed = dayTimeSpeed;

            started = true;

            Destroy(GetComponent<BoxCollider2D>());
        }
    }

    private void Update()
    {
        if( started &&
            dayTimerHandler.Hours >= endHour)
        {
            dayTimerHandler.TimeSpeed = 0;

            Destroy(gameObject);
        }
    }
}
