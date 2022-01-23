using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSystemHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> deactivateObjectsAtLoad = new List<GameObject>();

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

    private Keyboard keyboard;

    private int indexOfSaveGame;

    private string pathToSaveGameFolder;

    private string pathToSaves;

    private void Awake()
    {
        getObjects = GetComponent<GetObjectsFromWorld>();

        getAllObjectsInPlayerGround = GameObject.Find("PlayerHouseGround").GetComponent<GetAllObjectsInPlayerGround>();

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        playerAchievements = GameObject.Find("Global/Player").GetComponent<PlayerAchievements>();

        questTab = GameObject.Find("Global/Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();

        countPlayedMinutes = GameObject.Find("Global").GetComponent<CountPlayedMinutes>();

        npcDialogue = countPlayedMinutes.GetComponent<NPCDialogueSave>();

        gridSave = countPlayedMinutes.GetComponent<GridSaveHadler>();

        sunShadowHandler = dayTimerHandler.GetComponent<SunShadowHandler>();

        pathToSaves = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        pathToSaves = Path.Combine(pathToSaves, @"Sooth\Saves");

        pathToSaveGameFolder = pathToSaves;

        positionNpcAtLoad = GameObject.FindGameObjectsWithTag("NPC");

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void VerifyPathToSave()
    {
        if(pathToSaves != null && !pathToSaves.Contains(".svj"))
        {
            pathToSaves += @"\SaveGame" + indexOfSaveGame + ".svj";
        }
    }

    public void LoadSaveGame(int indexOfSaveGame, LoadSceneHandler loadSceneHandler)
    {
        this.loadSceneHandler = loadSceneHandler;
        this.indexOfSaveGame = indexOfSaveGame;

        VerifyPathToSave();

        SetDataToGame(ReadDataFromSave(pathToSaves));

        SetLocation(false);

        PositionNpcAtStartLocation();

        playerAchievements.GetComponent<PositionPlayerAtLoad>().PositionPlayer();
    }

    private void PositionNpcAtStartLocation()
    {
        foreach(GameObject npc in positionNpcAtLoad)
        {
            npc.GetComponent<LoadGamePositionNPC>().LoadGame();
        }
    }

    private void SetLocation(bool active)
    {
        foreach (GameObject location in deactivateObjectsAtLoad)
        {
            location.SetActive(active);
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
        dayTimerHandler.Days = saveGame.Days;

        playerInventory.SetInventoryFromSave(saveGame.PlayerInventory);

        getObjects.SetObjectsToWorld(saveGame.ObjectsInGame);

        questTab.SetQuestsWithID(saveGame.PlayerQuests);

        getAllObjectsInPlayerGround.SetObjectsInArea(saveGame.ObjectsInPlayerGround);

        npcDialogue.SetNpcsDialogue(saveGame.NpcDialogues);

        GetComponent<GetAllChestsStorage>().SetAllChestToWorld(saveGame.Chests);

        GetComponent<GetAllChestsStorage>().SetItemsToPlayerHouseChest(saveGame.PlayerHouseChest);

        GetComponent<GetAllSapling>().SetSaplingsToWorld(saveGame.Saplings);

        GetComponent<GetFarmPlots>().PositionFarmingPlots(saveGame.Plots);

        GetComponent<GetFarmPlots>().SetCroptsToWorld(saveGame.CropSaves);

        gridSave.SetDataToGridLocations(saveGame.GridSaves);

        if (loadSceneHandler != null)
        {
            loadSceneHandler.FinishGridSearchProcess = true;
        }

        sunShadowHandler.ReinitializeShadows();

        countPlayedMinutes.Minutes = saveGame.PlayedMinutes;
        countPlayedMinutes.Seconds = saveGame.PlayedSecundes;
    }

    private void VerifyFiles(string pathToSaveGame)
    {
        if(File.Exists(pathToSaveGameFolder))
        {
            if(File.Exists(pathToSaveGame))
            {
                File.Delete(pathToSaveGame);
            }
        }
        else
        {
            Directory.CreateDirectory(pathToSaveGameFolder);
        }    
    }

    public void SaveGame()
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
        SetLocation(false);

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

        return saveGame;
    }

    private void Update()
    {
        if(keyboard.pKey.isPressed)
        {
            SaveGame();
        }
        else if (keyboard.lKey.isPressed)
        {
            LoadSaveGame(0, null);
        }
    }
}
