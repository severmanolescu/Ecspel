using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightHandler : MonoBehaviour
{
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float speedIntensityChange;

    private bool direction = false;

    private Gradient gradient;

    private Light2D lightSource;

    public Gradient Gradient { get => gradient; set => gradient = value; }

    private void Awake()
    {
        GameObject.Find("DayTimer").GetComponent<SourceLightShadow>().AddLight(this);

        lightSource = GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        lightSource.pointLightInnerRadius = minRadius;
    }

    public void ChangeIntensity(Color color)
    {
        lightSource.color = color;
    }

    public void Update()
    {
        if(direction)
        {
            if(lightSource.pointLightInnerRadius >= maxRadius)
            {
                direction = false;
            }
            else
            {
                lightSource.pointLightInnerRadius += speedIntensityChange * Time.deltaTime;
            }
        }
        else
        {
            if (lightSource.pointLightInnerRadius <= minRadius)
            {
                direction = true;
            }
            else
            {
                lightSource.pointLightInnerRadius -= speedIntensityChange * Time.deltaTime;
            }
        }
    }
}
