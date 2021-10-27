using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Cut Trees", order = 1)]
public class CutTrees : Quest
{
    [Header("Number of trees cut down:")]
    public int number;

    public int Number { get { return number; } }
}