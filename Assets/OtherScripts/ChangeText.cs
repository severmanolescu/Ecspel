using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    public void Change(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
