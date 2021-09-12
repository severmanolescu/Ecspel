using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSourceShadowManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shadow;

    private Transform closestSource;
    private Vector3 position;

    private float maxDistance;

    private void Awake()
    {
        closestSource = null;

        position = transform.position;

        shadow = gameObject.GetComponent<SpriteRenderer>();  
    }

    private void Start()
    {
        maxDistance = DefaulData.maxLightSourceDistance;
    }

    private void Update()
    {
        if(closestSource != null)
        {
            shadow.enabled = true;

            Vector3 distance = closestSource.position - position;
            float currentSourceDistance = distance.sqrMagnitude;

            if (currentSourceDistance > maxDistance)
            {
                shadow.enabled = false;

                closestSource = null;
            }
        }
        else
        {
            shadow.enabled = false;
        }
    }

    public void SetSource(Transform newSource)
    {
        if (closestSource != null && newSource != closestSource)
        {
            Vector3 distance = closestSource.position - position;
            float currentSourceDistance = distance.sqrMagnitude;

            distance = newSource.position - position;
            float newSourceDistance = distance.sqrMagnitude;

            if (newSourceDistance < currentSourceDistance)
            {
                closestSource = newSource;
            }
        }
        else
        {
            closestSource = newSource;
        }
    }
}
