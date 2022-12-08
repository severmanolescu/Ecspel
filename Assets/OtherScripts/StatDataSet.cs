using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatDataSet : MonoBehaviour
{
    public void SetData(Sprite sprite, string amount)
    {
        Image image = GetComponentInChildren<Image>();

        image.sprite = sprite;

        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = amount;
    }
}
