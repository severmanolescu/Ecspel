using UnityEngine;

public class AddWindowToScript : MonoBehaviour
{
    void Start()
    {
        GameObject.Find("Global/DayTimer").GetComponent<ChangeWindowLightIntensity>().Renderers.Add(GetComponent<SpriteRenderer>());

        Destroy(this);
    }
}
