using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SourceLightShadow : MonoBehaviour
{
    [SerializeField] private Gradient gradientSourceLight;

    private bool lightStatus = true;

    private List<LightHandler> sourceLights = new List<LightHandler>();

    public void AddLight(LightHandler light)
    {
        if(!sourceLights.Contains(light))
        {
            sourceLights.Add(light);

            light.Gradient = gradientSourceLight;
        }
    }
    
    public void RemoveLight(LightHandler light)
    {
        if(sourceLights != null)
        {
            sourceLights.Remove(light);
        }
    }

    public void ChangeLightsIntensity(float intensity)
    {
        if (!(intensity <= 0.23f || intensity >= 0.68f))
        {
            if (lightStatus == true)
            {
                foreach (LightHandler light in sourceLights)
                {
                    light.gameObject.SetActive(false);
                }

                lightStatus = false;
            }
        }
        else if (sourceLights != null)
        {
            Color color = gradientSourceLight.Evaluate(intensity);

            foreach (LightHandler light in sourceLights)
            {
                light.ChangeIntensity(color);

                if (lightStatus == false && light != null)
                {
                    light.gameObject.SetActive(true);
                }
            }

            lightStatus = true;
        }
    }
}
