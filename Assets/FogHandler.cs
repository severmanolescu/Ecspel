using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogHandler : MonoBehaviour
{
    public int timeToChange = 5;

    public float maxAlpha = 70f;

    private new ParticleSystem particleSystem;

    private int speedOfAlpha = 1;

    public void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();

        maxAlpha /= 255f;
    }

    public void Start()
    {
        StartParticle();
    }

    public void StartParticle()
    {
        particleSystem.Play();

        var main = particleSystem.main;

        main.startColor = new Color(1, 1, 1, 0);

        StopAllCoroutines();

        StartCoroutine(WaitForSeconds());
    }

    private IEnumerator WaitForSeconds()
    {
        var main = particleSystem.main;

        while (true)
        {
            yield return new WaitForSeconds(timeToChange);

            if(main.startColor.color.a < maxAlpha)
            {
                main.startColor = new Color(1f, 1f, 1, main.startColor.color.a + speedOfAlpha / 255f);
            }
        }
    }
}
