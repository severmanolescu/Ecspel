using UnityEngine;

public class RainHandler : MonoBehaviour
{
    public int timeToChange = 5;

    private bool rainStart = false;

    [SerializeField] private LocationRainParticleChange locationRain;

    public LocationRainParticleChange LocationRain { get => locationRain; set => locationRain = value; }

    public void StartParticle()
    {
        locationRain.StartParticles(0);

        rainStart = true;
    }

    public void StopPArticles()
    {
        StopAllCoroutines();

        locationRain.StopParticles();

        rainStart = false;
    }

    public void ChangeRainLocation(LocationRainParticleChange locationRain)
    {
        if (locationRain != null && rainStart == true)
        {
            float time = this.locationRain.GetTime();

            this.locationRain.StopParticles();

            this.locationRain = locationRain;

            locationRain.StartParticles(time);
        }
    }
}
