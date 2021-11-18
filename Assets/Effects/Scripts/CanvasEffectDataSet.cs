using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasEffectDataSet : MonoBehaviour
{
    private TextMeshProUGUI duration;

    public void DataSet(Effect effect)
    {
        gameObject.GetComponentsInChildren<Image>()[1].sprite = effect.EffectSprite;

        TextMeshProUGUI[] textMesh = GetComponentsInChildren<TextMeshProUGUI>();

        textMesh[0].text = effect.EffectType.ToString();
        textMesh[1].text = effect.Duration.ToString();

        duration = textMesh[1];
    }

    public void ChangeDuration(float duration)
    {
        int minutes = (int)duration / 60;

        int seconds = (int) duration - minutes * 60;

        if(minutes != 0)
        {
            this.duration.text = minutes.ToString() + " : " + seconds.ToString();
        }
        else
        {
            this.duration.text = seconds.ToString();
        }
    }
}
