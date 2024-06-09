using System.Collections.Generic;
using UnityEngine;

public class PlayerAchievements : MonoBehaviour
{
    [SerializeField] private int cutTrees = 0;
    [SerializeField] private int destroyStones = 0;

    [SerializeField] private int questCount = 0;

    public int QuestCount { get => questCount; set => questCount = value; }
    public int CutTrees { get => cutTrees; set => cutTrees = value; }
    public int DestroyStones { get => destroyStones; set => destroyStones = value; }

    public List<int> GetAllAchievements()
    {
        List<int> achievements = new List<int>();

        achievements.Add(CutTrees);
        achievements.Add(DestroyStones);
        achievements.Add(questCount);

        return achievements;
    }

    public void SetAllAchievements(List<int> achievements)
    {
        if (achievements != null && achievements.Count == 5)
        {
            CutTrees = achievements[0];
            DestroyStones = achievements[1];
            questCount = achievements[2];
        }
    }
}
