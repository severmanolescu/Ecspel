using UnityEngine;

public class ObjectShadowHandler : MonoBehaviour
{
    private SunShadowHandler sunShadow;

    private void Awake()
    {
        sunShadow = GameObject.Find("Global/DayTimer").GetComponent<SunShadowHandler>();

        if (sunShadow != null)
        {
            sunShadow.AddShadow(this.transform);
        }
    }

    private void OnDestroy()
    {
        if (sunShadow != null)
        {
            sunShadow.RemoveShadow(this.transform);
        }
    }
}
