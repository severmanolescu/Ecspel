using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SaveGame
{
    private int days;

    private int playedMinutes;

    private List<Tuple<int, int>> playerInventory;

    private List<int> playerAchievements;

    //private List<Quest> playerQuests;

    private List<int> chestStorage;

    private List<GridNode[,]> gridNodes;

    public SaveGame(int days, int playedMinutes)
    {
        this.days = days;

        this.playedMinutes = playedMinutes;
    }

    public SaveGame()
    {

    }

    public int Days { get => days; set => days = value; }
    public int PlayedMinutes { get => playedMinutes; set => playedMinutes = value; }
    public List<Tuple<int, int>> PlayerInventory { get => playerInventory; set => playerInventory = value; }
    public List<int> ChestStorage { get => chestStorage; set => chestStorage = value; }
    public List<int> PlayerAchievements { get => playerAchievements; set => playerAchievements = value; }
    //public List<Quest> PlayerQuests { get => playerQuests; set => playerQuests = value; }
    public List<GridNode[,]> GridNodes { get => gridNodes; set => gridNodes = value; }
}
