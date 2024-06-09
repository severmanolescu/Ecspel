using System.Collections.Generic;
using UnityEngine;

public class ChangeWindowLightIntensity : MonoBehaviour
{
    private List<SpriteRenderer> interiorWindows = new List<SpriteRenderer>();
    private List<SpriteRenderer> exteriorWindows = new List<SpriteRenderer>();

    public void AddWindows(SpriteRenderer window)
    {
        if (window != null && interiorWindows != null)
        {
            if (window.gameObject.CompareTag("InteriorWindows"))
            {
                interiorWindows.Add(window);
            }
            else
            {
                exteriorWindows.Add(window);
            }
        }
    }

    public void RemoveWindows(SpriteRenderer window)
    {
        if (window != null && interiorWindows != null)
        {
            if (window.gameObject.CompareTag("ExteriorWindows"))
            {
                interiorWindows.Remove(window);
            }
            else
            {
                exteriorWindows.Remove(window);
            }
        }
    }

    public void SetIntensity(Color interiorColor, Color exteriorColor)
    {
        foreach (SpriteRenderer window in interiorWindows)
        {
            window.color = interiorColor;
        }

        foreach (SpriteRenderer window in exteriorWindows)
        {
            window.color = exteriorColor;
        }
    }
}
