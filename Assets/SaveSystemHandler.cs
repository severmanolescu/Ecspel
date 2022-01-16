using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystemHandler : MonoBehaviour
{
    private LoadSceneHandler loadSceneHandler;

    private int indexOfSaveGame;

    private string pathToSaves;

    private void Awake()
    {
        pathToSaves = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        pathToSaves = Path.Combine(pathToSaves, @"Sooth\Saves");
    }

    public void LoadSaveGame(int indexOfSaveGame, LoadSceneHandler loadSceneHandler)
    {
        this.loadSceneHandler = loadSceneHandler;
        this.indexOfSaveGame = indexOfSaveGame;

        pathToSaves += @"\SaveGame" + indexOfSaveGame + ".svj";

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
        GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().Days = saveGame.Days;

        loadSceneHandler.FinishGridSearchProcess = true;
    }
}
