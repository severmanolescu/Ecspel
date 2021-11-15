using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Destroy Stones", order = 1)]
public class DestroyStone : Quest
{
    [Header("the number of stones to be broken:")]
    public int number;

    public int Number { get { return number; } }
}