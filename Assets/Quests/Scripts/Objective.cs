using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Objective", order = 1)]
[Serializable]
public class Objective : ScriptableObject
{
    [SerializeField] private string objectiveName;

    [SerializeField] private bool completed = false;

    public Objective(string objectiveName, bool completed)
    {
        this.objectiveName = objectiveName;
        this.completed = completed;
    }

    public bool Completed { get => completed; set => completed = value; }
    public string ObjectiveName { get => objectiveName; set => objectiveName = value; }

    public Objective Copy()
    {
        return (Objective)this.MemberwiseClone();
    }
}
