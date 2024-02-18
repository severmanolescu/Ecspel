using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Monologue", order = 2)]
[Serializable]
public class Monolog : ScriptableObject
{
    [Header("Monologue:")]
    [TextArea(3, 3)]
    [SerializeField] private List<string> dialogueRespons;

    public List<string> DialogueRespons { get => dialogueRespons; }

    public Monolog Copy()
    {
        return (Monolog)this.MemberwiseClone();
    }
}
