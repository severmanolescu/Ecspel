using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Header("Who repond: \n\tfalse - Player\n\ttrue - NPC\n")]
    public bool whoRespond;

    [TextArea(3, 3)]
    public string dialogueText;
}
