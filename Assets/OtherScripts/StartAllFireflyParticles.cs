using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAllFireflyParticles : MonoBehaviour
{
    [SerializeField] private GameObject particlesLocation;

    private ParticleSystem[] particles;

    private bool started = false;

    private void Awake()
    {
        particles = particlesLocation.GetComponentsInChildren<ParticleSystem>();
    }

    public void StartParticles()
    {
        if (started == false)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Play();
            }

            started = true;
        }
    }

    public void StopParticles()
    {
        if (started == true)
        {
            foreach (ParticleSystem particle in particles)
            {
                particle.Stop();
            }

            started = false;
        }
    }
}
