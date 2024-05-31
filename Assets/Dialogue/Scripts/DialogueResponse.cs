using System;
using UnityEngine;

[Serializable]
public class DialogueResponse
{
    [Header("Who repond: \n\tfalse - Player or first NPC\n\ttrue - NPC or second NPC\n")]
    public bool whoRespond;

    [TextArea(3, 3)]
    public string dialogueText;
}

[Serializable]
public class DialogueNpc
{
    [Header("Who repond: \n\ttrue - First NPC\n\tfalse - Second NPC\n")]
    public bool whoRespond;

    [TextArea(3, 3)]
    public string dialogueText;
}
