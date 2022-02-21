using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTip : MonoBehaviour
{
    [SerializeField] private TipsCanvas tipCanvas;

    public void ShowTip(string tipText)
    {
        tipCanvas.gameObject.SetActive(true);

        tipCanvas.SetTip(tipText);
    }
}
