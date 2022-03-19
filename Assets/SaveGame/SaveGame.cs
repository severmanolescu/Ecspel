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

    private List<FarmPlotSave> plots;

    private List<CropSave> cropSaves;

    private List<Tuple<int, int>> playerHouseChest;

    private List<ChestSave> chests;

    private List<SaplingSave> saplings;

    private List<int> npcDialogues;

    private List<GridSave[,]> gridSaves;

    private List<ForgeStorage> forgeStorages;

    private bool tipShowState;

    private List<TipsSave> tipsSaves;

    private List<DialogueAppearSave> dialogueAppear;

    private List<GrassSaveGame> grassSaveGames;

    private List<int> crafts;

    private int maxCaveIndex;

    private bool raining;
    private bool fog;

    private int fogIntensity;

    private List<int> skillsLevels;

    public int Days { get => days; set => days = value; }
    public ulong PlayedMinutes { get => playedMinutes; set => playedMinutes = value; }
    public List<Tuple<int, int>> PlayerInventory { get => playerInventory; set => playerInventory = value; }
    public List<int> PlayerAchievements { get => playerAchievements; set => playerAchievements = value; }
    public List<ObjectSaveGame> ObjectsInGame { get => objectsInGame; set => objectsInGame = value; }
    public List<int> PlayerQuests { get => playerQuests; set => playerQuests = value; }
    public List<ObjectSaveGame> ObjectsInPlayerGround { get => objectsInPlayerGround; set => objectsInPlayerGround = value; }
    public List<FarmPlotSave> Plots { get => plots; set => plots = value; }
    public List<CropSave> CropSaves { get => cropSaves; set => cropSaves = value; }
    public ushort PlayedSecundes { get => playedSecundes; set => playedSecundes = value; }
    public List<ChestSave> Chests { get => chests; set => chests = value; }
    public List<Tuple<int, int>> PlayerHouseChest { get => playerHouseChest; set => playerHouseChest = value; }
    public List<SaplingSave> Saplings { get => saplings; set => saplings = value; }
    public List<int> NpcDialogues { get => npcDialogues; set => npcDialogues = value; }
    public List<GridSave[,]> GridSaves { get => gridSaves; set => gridSaves = value; }
    public int Coins { get => coins; set => coins = value; }
    public List<ForgeStorage> ForgeStorages { get => forgeStorages; set => forgeStorages = value; }
    public bool TipShowState { get => tipShowState; set => tipShowState = value; }
    public List<TipsSave> TipsSaves { get => tipsSaves; set => tipsSaves = value; }
    public List<DialogueAppearSave> DialogueAppear { get => dialogueAppear; set => dialogueAppear = value; }
    public List<GrassSaveGame> GrassSaveGames { get => grassSaveGames; set => grassSaveGames = value; }
    public List<int> Crafts { get => crafts; set => crafts = value; }
    public int MaxCaveIndex { get => maxCaveIndex; set => maxCaveIndex = value; }
    public bool Raining { get => raining; set => raining = value; }
    public bool Fog { get => fog; set => fog = value; }
    public int FogIntensity { get => fogIntensity; set => fogIntensity = value; }
    public List<int> SkillsLevels { get => skillsLevels; set => skillsLevels = value; }
}
