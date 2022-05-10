using System.Collections.Generic;
using UnityEngine;

public class PlayerAchievements : MonoBehaviour
{
    [Header("Enemy sprite:\n" +
        "1. Slime\n" +
        "2. Potion\n" +
        "3. Puddle")]
    [SerializeField] private List<Sprite> enemySprites = new();

    private int cutTrees = 0;
    private int destroyStones = 0;
    private int slimeKill = 0;
    private int potionKill = 0;
    private int puddleKill = 0;

    public int Trees { get { return cutTrees; } set { cutTrees = value; } }
    public int Stones { get { return destroyStones; } set { destroyStones = value; } }
    public int SlimeKill { get { return slimeKill; } set { slimeKill = value; } }
    public int PotionKill { get => potionKill; set => potionKill = value; }
    public int PuddleKill { get => puddleKill; set => puddleKill = value; }

    public List<int> GetAllAchievements()
    {
        List<int> achievements = new List<int>();

        achievements.Add(Trees);
        achievements.Add(Stones);
        achievements.Add(SlimeKill);
        achievements.Add(PotionKill);
        achievements.Add(PuddleKill);

        return achievements;
    }

    public void SetAllAchievements(List<int> achievements)
    {
        if(achievements != null && achievements.Count == 5)
        {
            Trees = achievements[0];
            Stones = achievements[1];
            SlimeKill = achievements[2];
            PotionKill = achievements[3];
            PuddleKill = achievements[4];
        }
    }

    public int GetEnemyKills()
    {
        return SlimeKill + PotionKill + PuddleKill;
    }

    public bool CheckForEnemyKill(KillEnemy killEnemy, int inialAmount)
    {
        int kills =  GetEnemyKills();

        if(kills >= inialAmount + killEnemy.number)
        {
            return true;
        }

        return false;
    }

    public Sprite GetEnemySprite(int enemyId)
    {
        switch (enemyId)
        {
            case 1: return enemySprites[0];
            case 2: return enemySprites[1];
            case 3: return enemySprites[2];
            default: return null;
        }
    }
}
