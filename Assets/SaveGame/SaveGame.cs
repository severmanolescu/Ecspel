using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SaveGame
{
    private int days;

    private ulong playedMinutes;

    private ushort playedSecundes;

    private int coins;

    private List<Tuple<int, int>> playerInventory;

    private List<int> playerAchievements;

    private List<int> playerQuests;

    private List<ObjectSaveGame> objectsInGame;

    private List<ObjectSaveGame> objectsInPlayerGround;

    private List<Tuple<float, float>> plots;

    private List<CropSave> cropSaves;

    private List<Tuple<int, int>> playerHouseChest;

    private List<ChestSave> chests;

    private List<SaplingSave> saplings;

    private List<int> npcDialogues;

    private List<GridSave[,]> gridSaves;

    public int Days { get => days; set => days = value; }
    public ulong PlayedMinutes { get => playedMinutes; set => playedMinutes = value; }
    public List<Tuple<int, int>> PlayerInventory { get => playerInventory; set => playerInventory = value; }
    public List<int> PlayerAchievements { get => playerAchievements; set => playerAchievements = value; }
    public List<ObjectSaveGame> ObjectsInGame { get => objectsInGame; set => objectsInGame = value; }
    public List<int> PlayerQuests { get => playerQuests; set => playerQuests = value; }
    public List<ObjectSaveGame> ObjectsInPlayerGround { get => objectsInPlayerGround; set => objectsInPlayerGround = value; }
    public List<Tuple<float, float>> Plots { get => plots; set => plots = value; }
    public List<CropSave> CropSaves { get => cropSaves; set => cropSaves = value; }
    public ushort PlayedSecundes { get => playedSecundes; set => playedSecundes = value; }
    public List<ChestSave> Chests { get => chests; set => chests = value; }
    public List<Tuple<int, int>> PlayerHouseChest { get => playerHouseChest; set => playerHouseChest = value; }
    public List<SaplingSave> Saplings { get => saplings; set => saplings = value; }
    public List<int> NpcDialogues { get => npcDialogues; set => npcDialogues = value; }
    public List<GridSave[,]> GridSaves { get => gridSaves; set => gridSaves = value; }
    public int Coins { get => coins; set => coins = value; }
}
