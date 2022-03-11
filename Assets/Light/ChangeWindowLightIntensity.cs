using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWindowLightIntensity : MonoBehaviour
{
    private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    public List<SpriteRenderer> Renderers { get => renderers; set => renderers = value; }

    public void SetIntensity(Color color)
    {
        foreach(SpriteRenderer renderer in Renderers)
        {
            if(renderer != null)
            {
                renderer.color = color;
            }
        }
    }
}
