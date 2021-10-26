using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAchievements : MonoBehaviour
{
    private int cutTrees = 0;
    private int destroyStones = 0;
    private int smallSlimeKill = 0;
    private int largeSlimeKill = 0;

    public int Trees { get { return cutTrees; } set { cutTrees = value; } }
    public int Stones { get { return destroyStones; } set { destroyStones = value; } }
    public int smallSlime { get { return smallSlimeKill; } set { smallSlimeKill = value; } }
    public int largeSlime { get { return largeSlimeKill; } set { largeSlimeKill = value; } }
}
