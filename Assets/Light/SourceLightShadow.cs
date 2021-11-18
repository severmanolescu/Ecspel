using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class SourceLightShadow : MonoBehaviour
{
    List<Light2D> sourceLights = new List<Light2D>();

    public void AddLight(Light2D light)
    {
        if(!sourceLights.Contains(light))
        {
            sourceLights.Add(light);
        }
    }
    
    public void RemoveLight(Light2D light)
    {
        if(sourceLights != null)
        {
            sourceLights.Remove(light);
        }
    }

    public void ChangeLightsIntensity(float intensity)
    {
        if(sourceLights != null)
        {
            foreach(Light2D light in sourceLights)
            {
                light.intensity = intensity;
            }
        }
    }
}
