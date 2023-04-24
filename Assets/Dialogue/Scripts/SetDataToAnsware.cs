using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetDataToAnsware : MonoBehaviour
{
    [SerializeField] private GameObject questImage;
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color selectedColor;

    public void SetDataToAnswer(string text, bool quest)
    {
        questImage.SetActive(quest);

        answerText.text = text;
    }

    public void ChangeBackgroundColor()
    {
        backgroundImage.color = selectedColor;
    }
}
