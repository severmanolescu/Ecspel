using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegradeOverTime : MonoBehaviour
{
    [SerializeField] private float alphaDecrease = .1f;

    [SerializeField] private int decreaseSpeed = 1;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Degrade());
    }

    private IEnumerator Degrade()
    {
        while(true)
        {
            float alpha = spriteRenderer.color.a;

            if(alpha <= 0)
            {
                Destroy(gameObject);

                break;
            }
            else
            {
                Color newColor = spriteRenderer.color;

                newColor.a -= alphaDecrease;

                spriteRenderer.color = newColor;
            }

            yield return new WaitForSeconds(decreaseSpeed);
        }
    }
}
