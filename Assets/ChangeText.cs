using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeText : MonoBehaviour
{
    public void Change(string text)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
}
