using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest", order = 1)]
public class Quest : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string npcName;
}
