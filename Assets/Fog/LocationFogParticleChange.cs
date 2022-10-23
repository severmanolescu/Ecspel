using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationFogParticleChange : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;

    public void StartParticles(float time, float alpha)
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Simulate(time, true, true);

            particle.Play();

            var main = particle.main;

            main.startColor = new Color(1, 1, 1, alpha);
        }
    }

    public void StopParticles()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.Stop();

            particle.Clear();
        }
    }

    public void AlphaChange(float alpha)
    {
        foreach (ParticleSystem particle in particles)
        {
            var main = particle.main;

            main.startColor = new Color(1, 1, 1, alpha);
        }
    }

    public float GetAlpha()
    {
        if (particles.Count > 0)
        {
            var main = particles[0].main;

            return main.startColor.color.a;
        }

        return 1;
    }

    public float GetTime()
    {
        if (particles.Count > 0)
        {
            return particles[0].time;
        }

        return 1;
    }
}
