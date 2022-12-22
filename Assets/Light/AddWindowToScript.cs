using UnityEngine;

public class AddWindowToScript : MonoBehaviour
{
    private ChangeWindowLightIntensity change;

    private void Start()
    {
        change = GameObject.Find("Global/DayTimer").GetComponent<ChangeWindowLightIntensity>();

        if(change != null)
        {
            change.Renderers.Add(GetComponent<SpriteRenderer>());
        }
    }

    private void OnDestroy()
    {
         if(change != null)
         {
            change.Renderers.Remove(GetComponent<SpriteRenderer>());
         }
    }
}
