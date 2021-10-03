using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest", order = 1)]
public class Quest : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private string npcName;
    [SerializeField] private string details;
    [SerializeField] private List<QuestRequirements> questRequirements;
    [SerializeField] private List<Item> rewards;

    public string Title { get { return title; } }
    public string NPCName { get { return npcName; } }
    public string Details { get { return details; } }
    public List<QuestRequirements> QuestRequirements { get { return questRequirements; } }
    public List<Item> Rewards { get { return rewards; } }
}
