using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SourceLightShadow : MonoBehaviour
{
    [SerializeField] private Gradient gradientSourceLight;

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
        if(sourceLights != null)
        {
            Color color = gradientSourceLight.Evaluate(intensity);

            foreach (LightHandler light in sourceLights)
            {
                light.ChangeIntensity(color);
            }
        }
    }
}
