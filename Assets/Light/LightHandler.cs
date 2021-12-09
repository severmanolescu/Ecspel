using UnityEngine;


public class LightHandler : MonoBehaviour
{
    private Gradient gradient;

    private UnityEngine.Rendering.Universal.Light2D lightSource;

    public Gradient Gradient { get => gradient; set => gradient = value; }

    private void Awake()
    {
        GameObject.Find("DayTimer").GetComponent<SourceLightShadow>().AddLight(this);

        lightSource = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    public void ChangeIntensity(Color color)
    {
        lightSource.color = color;
    }
}
