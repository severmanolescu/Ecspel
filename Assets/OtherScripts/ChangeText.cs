using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    public void Change(string text)
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        textMesh.text = text;
    }

    public void HideAmount()
    {
        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
        else
        {
            GetComponentInChildren<ChangeText>().gameObject.SetActive(false);
        }
    }

    public void ChangeColor(Color color)
    {
        if (textMesh != null)
        {
            textMesh.color = color;
        }
    }
}
