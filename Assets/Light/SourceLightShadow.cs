using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SourceLightShadow : MonoBehaviour
{
    [SerializeField] private Gradient gradientSourceLight;

    private bool lightStatus = false;

    private List<LightHandler> sourceLights = new List<LightHandler>();

    public void AddLight(LightHandler light)
    {
        if (!sourceLights.Contains(light))
        {
            sourceLights.Add(light);

            if(lightStatus)
            {
                light.TurnOnLight();
            }
            else
            {
                light.TurnOffLight();
            }
        }
    }

    public void RemoveLight(LightHandler light)
    {
        if (sourceLights != null)
        {
            sourceLights.Remove(light);
        }
    }

    public void ChangeLightsIntensity(float intensity)
    {
        if (lightStatus == true && !(intensity <= 0.23f || intensity >= 0.68f))
        {
            if (sourceLights != null)
            {
                foreach (LightHandler light in sourceLights)
                {
                    light.TurnOffLight();
                }

                lightStatus = false;
            }
        }
        else if (lightStatus == false && intensity <= 0.23f || intensity >= 0.68f)
        {
            if (sourceLights != null)
            {
                foreach (LightHandler light in sourceLights)
                {
                    light.TurnOnLight();
                }

                lightStatus = true;
            }
        }
    }
}
