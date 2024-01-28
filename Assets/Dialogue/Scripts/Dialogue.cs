using System;
using UnityEngine;

[Serializable]
public class Dialogue
{
    [Header("Who repond: \n\tfalse - Player or first NPC\n\ttrue - NPC or second NPC\n")]
    public bool whoRespond;

    [TextArea(3, 3)]
    public string dialogueText;
}
