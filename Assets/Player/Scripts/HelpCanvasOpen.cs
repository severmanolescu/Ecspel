using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCanvasOpen : MonoBehaviour
{
    private HelpHandler helpHandler;

    private void Awake()
    {
        helpHandler = GameObject.Find("Global/Player/Canvas/Help").GetComponent<HelpHandler>();
    }

    public void ShowHelp()
    {
        helpHandler.StartHelp();

        helpHandler.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
}
