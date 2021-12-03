using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightHandler : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("DayTimer").GetComponent<SourceLightShadow>().AddLight(gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>());
    }
}
