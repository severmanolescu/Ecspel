using UnityEngine;

public class ClockHandler : MonoBehaviour
{
    private ClockTimeChange clockTimeChange;

    private SpriteRenderer minutes;
    private SpriteRenderer hours;

    private void Awake()
    {
        clockTimeChange = GameObject.Find("Global/DayTimer").GetComponent<ClockTimeChange>();

        clockTimeChange.AddClock(this);

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

        minutes = sprites[1];
        hours = sprites[2];
    }

    public void SetTime(Quaternion minute, Quaternion hour)
    {
        minutes.transform.rotation = minute;
        hours.transform.rotation = hour;
    }
}
