using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New NPC dialogue", order = 3)]
[Serializable]
public class DialogueNPCs : ScriptableObject
{
    [Header("Dialogue between NPC's:")]
    [SerializeField] private List<DialogueNpc> dialogueNpc;

    public List<DialogueNpc> DialogueRespons { get => dialogueNpc; }

    public DialogueNPCs Copy()
    {
        return (DialogueNPCs)this.MemberwiseClone();
    }
}
