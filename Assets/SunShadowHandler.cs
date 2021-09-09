using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunShadowHandler : MonoBehaviour
{
    private List<Transform> sunShadows = new List<Transform>();

    private DayTimerHandler dayTimerHandler;

    private float rotation;

    private bool active = true;

    private void Awake()
    {
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("SunShadow");

        foreach(GameObject shadow in shadows)
        {
            sunShadows.Add(shadow.transform);

            shadow.GetComponent<SpriteRenderer>().sortingOrder = -1;
        }

        dayTimerHandler = gameObject.GetComponent<DayTimerHandler>();
    }

    private void Update()
    {
        dayTimerHandler.GetTimer(out float minutes, out int hours);
        dayTimerHandler.GetIntensity(out float intensity);

        if (hours >= DefaulData.dayStart && hours <= DefaulData.dayEnd + DefaulData.dayNightCycleTime)
        {
            rotation = Mathf.SmoothStep(-90, 90, (hours + minutes / 60f) / 25f);

            foreach(Transform shadow in sunShadows)
            {
                if (shadow != null)
                {
                    shadow.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, rotation);

                    shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, intensity / 3f);

                    shadow.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Transform shadow in sunShadows)
            {
                if (shadow != null)
                {
                    shadow.gameObject.SetActive(false);
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
}
