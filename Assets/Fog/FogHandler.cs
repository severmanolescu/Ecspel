using System.Collections;
using UnityEngine;

public class FogHandler : MonoBehaviour
{
    public int timeToChange = 5;

    public float maxAlpha = 70f;

    private bool fogStart = false;

    [SerializeField] private LocationFogParticleChange locationFog;

    private int speedOfAlpha = 1;

    public LocationFogParticleChange LocationFog { get => locationFog; set => locationFog = value; }

    public void Awake()
    {
        maxAlpha /= 255f;
    }

    public void StartParticle(float maxAlpha)
    {
        this.maxAlpha = maxAlpha / 255f;

        locationFog.StartParticles(0, 0);

        fogStart = true;

        StopAllCoroutines();

        StartCoroutine(WaitForSeconds());
    }

    public void StopPArticles()
    {
        StopAllCoroutines();

        locationFog.StopParticles();

        fogStart = false;
    }

    private IEnumerator WaitForSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeToChange);

            if (locationFog.GetAlpha() < maxAlpha)
            {
                locationFog.AlphaChange(locationFog.GetAlpha() + speedOfAlpha / 255f);
            }
        }
    }

    public void ChangeFogLocation(LocationFogParticleChange locationFog)
    {
        if (locationFog != null && fogStart == true)
        {
            float time = this.locationFog.GetTime();
            float alpha = this.locationFog.GetAlpha();

            this.locationFog.StopParticles();

            this.locationFog = locationFog;

            locationFog.StartParticles(time, alpha);

            StopAllCoroutines();

            StartCoroutine(WaitForSeconds());
        }
    }
}
