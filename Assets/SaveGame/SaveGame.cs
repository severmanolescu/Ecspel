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

    private List<int> playerQuests;

    private List<int> chestStorage;

    private List<ObjectSaveGame> objectsInGame;

    private List<ObjectSaveGame> objectsInPlayerGround;

    private List<Tuple<float, float>> plots;

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
    public List<ObjectSaveGame> ObjectsInGame { get => objectsInGame; set => objectsInGame = value; }
    public List<int> PlayerQuests { get => playerQuests; set => playerQuests = value; }
    public List<ObjectSaveGame> ObjectsInPlayerGround { get => objectsInPlayerGround; set => objectsInPlayerGround = value; }
    public List<Tuple<float, float>> Plots { get => plots; set => plots = value; }
}
