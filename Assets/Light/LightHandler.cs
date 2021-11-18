using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightHandler : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("DayTimer").GetComponent<SourceLightShadow>().AddLight(gameObject.GetComponent<Light2D>());
    }
}
