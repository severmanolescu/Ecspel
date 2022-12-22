using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightHandler : MonoBehaviour
{
    [SerializeField] private Sprite lightOn;
    [SerializeField] private Sprite lightOff;

    private Light2D spotLight;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spotLight = GetComponentInChildren<Light2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject.Find("Global/DayTimer").GetComponent<SourceLightShadow>().AddLight(this);
    }

    public void TurnOnLight()
    {
        spotLight.enabled = true;

        if(lightOn != null)
        {
            spriteRenderer.sprite = lightOn;
        }
    }

    public void TurnOffLight()
    {
        spotLight.enabled = false;

        if (lightOn != null)
        {
            spriteRenderer.sprite = lightOff;
        }
    }
}
