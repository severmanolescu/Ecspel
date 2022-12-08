using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSystemHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> deactivateObjectsAtLoad = new List<GameObject>();
    [SerializeField] private List<GameObject> listOfCameras = new List<GameObject>();

    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject playerGround;

    [SerializeField] private QuickSlotsChanger quickSlots;

    private GameObject[] positionNpcAtLoad;

    private PlayerInventory playerInventory;

    private PlayerAchievements playerAchievements;

    private QuestTabHandler questTab;

    private DayTimerHandler dayTimerHandler;

    private LoadSceneHandler loadSceneHandler;

    private GetAllObjectsInPlayerGround getAllObjectsInPlayerGround;

    private GetObjectsFromWorld getObjects;

    private SunShadowHandler sunShadowHandler;

    private CountPlayedMinutes countPlayedMinutes;

    private NPCDialogueSave npcDialogue;

    private GridSaveHadler gridSave;

    private TipsCanvas tipsCanvas;

    private SkillsHandler skillsHandler;

    private Keyboard keyboard;

    private ChangeDataFromQuest changeDataFromQuest;

    private SpecialFlowerHandler specialFlowerHandler;

    private int indexOfSaveGame;

    private string pathToSaveGameFolder;

    private string pathToSaves;

    public int IndexOfSaveGame { get => indexOfSaveGame; set => indexOfSaveGame = value; }

    private void Awake()
    {
        getObjects = GetComponent<GetObjectsFromWorld>();

        getAllObjectsInPlayerGround = GameObject.Find("PlayerHouseGround").GetComponent<GetAllObjectsInPlayerGround>();

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        playerAchievements = GameObject.Find("Global/Player").GetComponent<PlayerAchievements>();

        questTab = GameObject.Find("Global/Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        tipsCanvas = GameObject.Find("Global/Player/Canvas/Tips").GetComponent<TipsCanvas>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();

        countPlayedMinutes = GameObject.Find("Global").GetComponent<CountPlayedMinutes>();
        changeDataFromQuest = countPlayedMinutes.GetComponent<ChangeDataFromQuest>();

        npcDialogue = countPlayedMinutes.GetComponent<NPCDialogueSave>();

        gridSave = countPlayedMinutes.GetComponent<GridSaveHadler>();

        sunShadowHandler = dayTimerHandler.GetComponent<SunShadowHandler>();

        pathToSaves = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        pathToSaves = Path.Combine(pathToSaves, @"Sooth\Saves");

        pathToSaveGameFolder = pathToSaves;

        positionNpcAtLoad = GameObject.FindGameObjectsWithTag("NPC");

        skillsHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();

        keyboard = InputSystem.GetDevice<Keyboard>();

        specialFlowerHandler = GameObject.Find("RoadCave/StoryPlace").GetComponent<SpecialFlowerHandler>();
    }

    private void VerifyPathToSave()
    {
        if (pathToSaves != null || !pathToSaves.Contains(".svj"))
        {
            pathToSaves = pathToSaveGameFolder + @"\SaveGame" + IndexOfSaveGame + ".svj";
        }
    }

    public void LoadSaveGame()
    {
        VerifyPathToSave();

        SetDataToGame(ReadDataFromSave(pathToSaves));

        SetLocation(false);

        PositionNpcAtStartLocation();

        playerAchievements.GetComponent<PositionPlayerAtLoad>().PositionPlayer();

        playerGround.SetActive(true);
    }

    public void LoadSaveGame(int indexOfSaveGame, LoadSceneHandler loadSceneHandler)
    {
        this.loadSceneHandler = loadSceneHandler;
        this.IndexOfSaveGame = indexOfSaveGame;

        VerifyPathToSave();

        SetDataToGame(ReadDataFromSave(pathToSaves));

        SetLocation(false);

        PositionNpcAtStartLocation();

        playerAchievements.GetComponent<PositionPlayerAtLoad>().PositionPlayer();

        playerGround.SetActive(true);
    }

    private void PositionNpcAtStartLocation()
    {
        foreach (GameObject npc in positionNpcAtLoad)
        {
            LoadGamePositionNPC loadGame = npc.GetComponent<LoadGamePositionNPC>();

            if (loadGame != null)
            {
                loadGame.LoadGame();
            }
        }
    }

    private void SetLocation(bool active)
    {
        foreach (GameObject location in deactivateObjectsAtLoad)
        {
            if (location != null)
            {
                location.SetActive(active);

                if (active == false)
                {
                    location.GetComponent<DeactivateCamera>().Deactivate();
                }
            }
        }
    }

    private SaveGame ReadDataFromSave(string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Open);

        SaveGame data = formatter.Deserialize(stream) as SaveGame;
        stream.Close();

        return data;
    }

    private void SetDataToGame(SaveGame saveGame)
    {
        foreach (GameObject camera in listOfCameras)
        {
            camera.SetActive(false);
        }
        mainCamera.SetActive(true);

        dayTimerHandler.Days = saveGame.Days;
        dayTimerHandler.LoadGameSetWakeUpHour();

        dayTimerHandler.GetComponent<CropGrowHandler>().ReinitializeLists();

        playerInventory.SetInventoryFromSave(saveGame.PlayerInventory);

        SetLocation(true);

        getObjects.SetObjectsToWorld(saveGame.ObjectsInGame);

        getAllObjectsInPlayerGround.SetObjectsInArea(saveGame.ObjectsInPlayerGround);

        SetLocation(false);

        questTab.SetQuestsWithID(saveGame.PlayerQuests);

        npcDialogue.SetNpcsDialogue(saveGame.NpcDialogues);

        GetComponent<GetAllChestsStorage>().SetAllChestToWorld(saveGame.Chests);

        GetComponent<GetAllChestsStorage>().SetItemsToPlayerHouseChest(saveGame.PlayerHouseChest);

        GetComponent<GetAllSapling>().SetSaplingsToWorld(saveGame.Saplings);

        GetComponent<GetFarmPlots>().PositionFarmingPlots(saveGame.Plots);

        GetComponent<GetFarmPlots>().SetCroptsToWorld(saveGame.CropSaves);

        GetComponent<GetAllForgesStorage>().SetAllForges(saveGame.ForgeStorages);

        GetComponent<GetAllTipsLocation>().SetTipToWorld(saveGame.TipsSaves);

        GetComponent<GetAllDialogueAppear>().SetDialogueToWorld(saveGame.DialogueAppear);

        GetComponent<GetAllGrass>().SetGrassToWorld(saveGame.GrassSaveGames);

        GetComponent<GetAllCrafts>().SetCrafts(saveGame.Crafts);

        specialFlowerHandler.SetFlowersStatus(saveGame.FlowersPlaced, saveGame.CollectedItemTable);

        skillsHandler.SetSkillsLevels(saveGame.SkillsLevels);

        dayTimerHandler.SetWeatherAtLoad(saveGame.Raining, saveGame.Fog, saveGame.FogIntensity);

        GameObject.Find("Caves").GetComponent<CaveSystemHandler>().MaxCaveIndex = saveGame.MaxCaveIndex;

        tipsCanvas.NotShow = saveGame.TipShowState;

        playerInventory.CoinsHandler.Amount = saveGame.Coins;

        gridSave.SetDataToGridLocations(saveGame.GridSaves);

        if (loadSceneHandler != null)
        {
            loadSceneHandler.FinishGridSearchProcess = true;
        }

        sunShadowHandler.ReinitializeShadows();

        countPlayedMinutes.Minutes = saveGame.PlayedMinutes;
        countPlayedMinutes.Seconds = saveGame.PlayedSecundes;

        changeDataFromQuest.SetData(saveGame.Bonuses);

        quickSlots.Reinitialize();
    }

    private void VerifyFiles(string pathToSaveGame)
    {
        if (!File.Exists(pathToSaveGameFolder))
        {
            Directory.CreateDirectory(pathToSaveGameFolder);

            FileStream file = File.Create(pathToSaveGame);

            file.Close();
        }
    }

    private void SaveGame()
    {
        SaveGame saveGame = GetDataFromGame();

        VerifyPathToSave();

        VerifyFiles(pathToSaves);

        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(pathToSaves, FileMode.Create);

        formatter.Serialize(stream, saveGame);
        stream.Close();
    }

    private SaveGame GetDataFromGame()
    {
        SaveGame saveGame = new SaveGame();

        saveGame.Days = dayTimerHandler.Days;

        saveGame.PlayerInventory = playerInventory.GetAllItemsNo();

        saveGame.PlayerAchievements = playerAchievements.GetAllAchievements();

        saveGame.PlayerQuests = questTab.GetAllQuestID();

        SetLocation(true);
        saveGame.ObjectsInGame = getObjects.GetAllObjectsFromArea();

        saveGame.ObjectsInPlayerGround = getAllObjectsInPlayerGround.GetAllObjects();

        saveGame.Plots = GetComponent<GetFarmPlots>().GetAllFarmingPlots();

        saveGame.CropSaves = GetComponent<GetFarmPlots>().GetAllCrops();

        saveGame.PlayedMinutes = countPlayedMinutes.Minutes;

        saveGame.PlayedSecundes = countPlayedMinutes.Seconds;

        saveGame.Chests = GetComponent<GetAllChestsStorage>().GetAllChestStorage();

        saveGame.PlayerHouseChest = GetComponent<GetAllChestsStorage>().GetPlayerHouseChestStorage();

        saveGame.Saplings = GetComponent<GetAllSapling>().GetAllObjects();

        saveGame.NpcDialogues = npcDialogue.GetNpcsDialogue();

        saveGame.GridSaves = gridSave.GetAllGridLocationData();

        saveGame.ForgeStorages = GetComponent<GetAllForgesStorage>().GetAllForges();

        saveGame.Coins = playerInventory.CoinsHandler.Amount;

        saveGame.TipShowState = tipsCanvas.NotShow;

        saveGame.TipsSaves = GetComponent<GetAllTipsLocation>().GetAllTips();

        saveGame.DialogueAppear = GetComponent<GetAllDialogueAppear>().GetAllDialogue();

        saveGame.GrassSaveGames = GetComponent<GetAllGrass>().GetAll();

        saveGame.Crafts = GetComponent<GetAllCrafts>().GetCrafts();

        saveGame.MaxCaveIndex = GameObject.Find("Caves").GetComponent<CaveSystemHandler>().MaxCaveIndex;

        saveGame.Raining = dayTimerHandler.Raining;
        saveGame.Fog = dayTimerHandler.Fog;
        saveGame.FogIntensity = dayTimerHandler.FogAlpha;

        saveGame.SkillsLevels = skillsHandler.GetAllSkillsLevels();

        saveGame.Bonuses = changeDataFromQuest.GetData();

        saveGame.FlowersPlaced = specialFlowerHandler.GetFlowerStatus();
        saveGame.CollectedItemTable = specialFlowerHandler.CollectedItem;

        return saveGame;
    }

    public void StartSaveGame()
    {
        SaveGame();

        PositionNpcAtStartLocation();

        playerGround.SetActive(true);
    }

    private void Update()
    {
        if (keyboard.lKey.isPressed)
        {
            LoadSaveGame(0, null);
        }
        else if (keyboard.pKey.isPressed)
        {
            StartSaveGame();
        }
    }
}
