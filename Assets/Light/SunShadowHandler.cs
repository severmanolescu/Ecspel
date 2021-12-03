using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShadowHandler : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;

    private List<Transform> sunShadows = new List<Transform>();

    private DayTimerHandler dayTimerHandler;

    private float rotation;

    private int dayStart;
    private int dayEnd;
    private int dayNightCycleTime;

    private float alpha = 1f;

    private void Awake()
    {
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("SunShadow");

        foreach(GameObject shadow in shadows)
        {
            sunShadows.Add(shadow.transform);

            shadow.GetComponent<SpriteRenderer>().sortingOrder = -2;
        }

        dayTimerHandler = gameObject.GetComponent<DayTimerHandler>();
    }

    private void Start()
    {
        dayStart = DefaulData.dayStart;
        dayEnd = DefaulData.dayEnd;
        dayNightCycleTime = DefaulData.dayNightCycleTime;
    }

    private void Update()
    {
        dayTimerHandler.GetTimer(out float minutes, out int hours);
        dayTimerHandler.GetIntensity(out float intensity);

        if (hours >= dayStart && hours <= dayEnd + dayNightCycleTime)
        {
            rotation = Mathf.SmoothStep(-90, 90, (hours + minutes / 60f) / 25f);

            foreach(Transform shadow in sunShadows)
            {
                if (shadow != null)
                {
                    shadow.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotation);

                    shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, intensity);

                    shadow.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            alpha -= fadeSpeed * Time.deltaTime;

            Color color = new Color(1f, 1f, 1f, alpha);

            foreach (Transform shadow in sunShadows)
            {
                if (shadow != null)
                {
                    shadow.GetComponent<SpriteRenderer>().color = color;
                }
            }
        }
    }

    public void ChangeShadowAlpha(float alpha)
    {
        foreach(Transform shadow in sunShadows)
        {
            shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
        }
    }
    
    public void ReinitializeShadows()
    {
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("SunShadow");

        foreach (GameObject shadow in shadows)
        {
            sunShadows.Add(shadow.transform);

            shadow.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }
    }

    public void AddShadow(Transform shadow)
    {
        if (!sunShadows.Contains(shadow))
        {
            sunShadows.Add(shadow);
        }
    }
    public void RemoveShadow(Transform shadow)
    {
        if(sunShadows != null)
        {
            sunShadows.Remove(shadow);
        }
    }
}
