using TMPro;
using UnityEngine;

public class SetDataToAnsware : MonoBehaviour
{
    [SerializeField] private GameObject questImage;
    [SerializeField] private TextMeshProUGUI answerText;

    public void SetDataToAnswer(string text, bool quest)
    {
        questImage.SetActive(quest);

        answerText.text = text;
    }
}
