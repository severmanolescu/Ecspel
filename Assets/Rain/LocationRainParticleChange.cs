using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRainParticleChange : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;

    public void StartParticles(float time)
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Simulate(time, true, true);

            particle.Play();
        }
    }

    public void StopParticles()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.Stop();
        }
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
