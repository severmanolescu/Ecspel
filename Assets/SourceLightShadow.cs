using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceLightShadow : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Shadow"))
        {
            LightSourceShadowManager lightSource = collision.GetComponent<LightSourceShadowManager>();

            if(lightSource != null)
            {
                lightSource.SetSource(transform);
            }
        }
    }
}
