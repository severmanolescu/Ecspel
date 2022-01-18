using System.Collections.Generic;
using UnityEngine;

public class PlayerAchievements : MonoBehaviour
{
    private int cutTrees = 0;
    private int destroyStones = 0;
    private int smallSlimeKill = 0;
    private int largeSlimeKill = 0;
    private int potionKill = 0;

    public int Trees { get { return cutTrees; } set { cutTrees = value; } }
    public int Stones { get { return destroyStones; } set { destroyStones = value; } }
    public int SmallSlime { get { return smallSlimeKill; } set { smallSlimeKill = value; } }
    public int LargeSlime { get { return largeSlimeKill; } set { largeSlimeKill = value; } }
    public int PotionKill { get => potionKill; set => potionKill = value; }

    public List<int> GetAllAchievements()
    {
        List<int> achievements = new List<int>();

        achievements.Add(Trees);
        achievements.Add(Stones);
        achievements.Add(SmallSlime);
        achievements.Add(LargeSlime);
        achievements.Add(PotionKill);

        return achievements;
    }

    public void SetAllAchievements(List<int> achievements)
    {
        if(achievements != null && achievements.Count == 5)
        {
            Trees = achievements[0];
            Stones = achievements[1];
            SmallSlime = achievements[2];
            LargeSlime = achievements[3];
            PotionKill = achievements[4];
        }
    }
}
