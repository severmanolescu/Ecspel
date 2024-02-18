using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanielBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> danielBehavior;

    public override void StartBehavior()
    {
        base.CheckForBehavior(danielBehavior);
    }
}
