using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystemHandler : MonoBehaviour
{
    [SerializeField] private List<LocationGridSave> locationGridSaves = new List<LocationGridSave>();

    private PlayerInventory playerInventory;

    private PlayerAchievements playerAchievements;

    private QuestTabHandler questTab;

    private DayTimerHandler dayTimerHandler;

    private LoadSceneHandler loadSceneHandler;

    private GetItemFromNO getItem;

    private int indexOfSaveGame;

    private string pathToSaves;

    private void Awake()
    {
        getItem = GameObject.Find("Global").GetComponent<GetItemFromNO>();

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        playerAchievements = GameObject.Find("Global/Player").GetComponent<PlayerAchievements>();

        questTab = GameObject.Find("Global/Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();

        pathToSaves = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        pathToSaves = Path.Combine(pathToSaves, @"Sooth\Saves");
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

        if (loadSceneHandler != null)
        {
            loadSceneHandler.FinishGridSearchProcess = true;
        }
    }

    private void VerifyFiles(string pathToSaveGame)
    {
        if(File.Exists(pathToSaves))
        {
            if(File.Exists(pathToSaveGame))
            {
                File.Delete(pathToSaveGame);
            }
        }
        else
        {
            Directory.CreateDirectory(pathToSaves);
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

    private List<GridNode[,]> GetAllGridNodes()
    {
        List<GridNode[,]> gridNodes = new List<GridNode[,]>();

        foreach (LocationGridSave locationGrid in locationGridSaves)
        {
            if (locationGrid != null)
            {
                if (locationGrid.Grid.gridArray != null)
                {
                    gridNodes.Add(locationGrid.Grid.gridArray);
                }
            }
        }

        return gridNodes;
    }

    private SaveGame GetDataFromGame()
    {
        SaveGame saveGame = new SaveGame();

        saveGame.Days = dayTimerHandler.Days;

        saveGame.PlayerInventory = playerInventory.GetAllItemsNo();

        saveGame.PlayerAchievements = playerAchievements.GetAllAchievements();

        //saveGame.PlayerQuests = questTab.GetAllQuests();

        saveGame.GridNodes = GetAllGridNodes();

        return saveGame;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            LoadSaveGame(0, null);
        }
    }
}
