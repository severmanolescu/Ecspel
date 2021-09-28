using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDegradation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private float alpha;

    private Color color;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        alpha = spriteRenderer.color.a;

        color = spriteRenderer.color;
    }

    private void Update()
    {
        alpha -= DefaulData.degradation * Time.deltaTime;

        color.a = alpha;

        spriteRenderer.color = color;

        if(color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
