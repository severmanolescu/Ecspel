using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveGamesLoad : MonoBehaviour
{
    [SerializeField] private GameObject deleteSave;

    [SerializeField] private LoadSceneHandler loadScene;

    [SerializeField] private GameObject saveGamePrefab;

    [SerializeField] private Transform spawnLocation;

    [SerializeField] private int maxSaveGames;

    [SerializeField] private Button playButton;
    [SerializeField] private Button deleteButton;

    private PrincipalMenuHandler principalMenu;

    private string path;

    private void InstantiateLoadButton(string filePath, int indexOfSaveGame)
    {
        FileInfo fileInfo = new FileInfo(filePath);

        GameObject save = Instantiate(saveGamePrefab, spawnLocation);

        SaveGame saveGame = ReadData(path + @"\" + fileInfo.Name);

        TextMeshProUGUI[] text = save.GetComponentsInChildren<TextMeshProUGUI>();

        text[0].text = "Slot " + indexOfSaveGame;

        text[1].text = "Zile: " + saveGame.Days;

        int hours = 0;
        int minutes = 0;

        if (saveGame.PlayedMinutes > 0)
        {
            hours = saveGame.PlayedMinutes / 60;
            minutes = saveGame.PlayedMinutes % 60;
        }

        text[2].text = "Timp: " + hours + "h " + minutes + "m";

        int auxiliarForButtonIndex = indexOfSaveGame;
        save.GetComponent<Button>().onClick.AddListener(delegate { SelectSaveGame(auxiliarForButtonIndex); });
        save.GetComponent<Button>().onClick.AddListener(delegate { principalMenu.PlayButtonClip(); });
    }

    private void InstantiateNewGameButton(int indexOfSaveGame)
    {
        GameObject save = Instantiate(saveGamePrefab, spawnLocation);

        TextMeshProUGUI[] text = save.GetComponentsInChildren<TextMeshProUGUI>();

        text[0].text = "Joc nou";

        text[1].gameObject.SetActive(false);
        text[2].gameObject.SetActive(false);

        int auxiliarForButtonIndex = indexOfSaveGame;
        save.GetComponent<Button>().onClick.AddListener(delegate { SelectNewSaveGame(auxiliarForButtonIndex); });
        save.GetComponent<Button>().onClick.AddListener(delegate { principalMenu.PlayButtonClip(); });
    }

    private void Start()
    {
        principalMenu = GetComponent<PrincipalMenuHandler>();

        deleteSave.SetActive(false);

        playButton.gameObject.SetActive(false);
        deleteButton.gameObject.SetActive(false);

        path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        path = Path.Combine(path, @"Sooth\Saves");

        if (Directory.Exists(path))
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            for(int indexOfSaveGame = 1; indexOfSaveGame <= maxSaveGames; indexOfSaveGame++)
            {
                string filePath = path + @"\SaveGame" + indexOfSaveGame + ".svj";

                if (File.Exists(filePath))
                {
                    InstantiateLoadButton(filePath, indexOfSaveGame);
                }
                else
                {
                    InstantiateNewGameButton(indexOfSaveGame);
                }
            }
        }
        else
        {
            System.IO.Directory.CreateDirectory(path);

            string filePath = path + @"\SaveGame1.svj";

            SaveGame saveGame = new SaveGame(10, 0);

            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(filePath, FileMode.Create);

            formatter.Serialize(stream, saveGame);
            stream.Close();
        }
    }

    private SaveGame ReadData(string path)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Open);

        SaveGame data = formatter.Deserialize(stream) as SaveGame;
        stream.Close();

        return data;
    }

    private void SelectNewSaveGame(int indexOfSaveGame)
    {
        playButton.gameObject.SetActive(true);
        deleteButton.gameObject.SetActive(false);

        playButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(delegate { PlayNewGame(indexOfSaveGame); });
    }

    public void SelectSaveGame(int indexOfSaveGame)
    {
        playButton.gameObject.SetActive(true);
        deleteButton.gameObject.SetActive(true);

        playButton.onClick.RemoveAllListeners();
        deleteButton.onClick.RemoveAllListeners();

        playButton.onClick.AddListener(delegate { PlaySaveGame(indexOfSaveGame); });
        playButton.onClick.AddListener(delegate { principalMenu.PlayButtonClip(); });
        deleteButton.onClick.AddListener(delegate { OpenDeleteSaveGame(indexOfSaveGame); });
        deleteButton.onClick.AddListener(delegate { principalMenu.PlayButtonClip(); });
    }

    public void PlayNewGame(int indexOfSaveGame)
    {
        GetComponent<PrincipalMenuHandler>().PlayButton(indexOfSaveGame);
    }

    public void PlaySaveGame(int indexOfSaveGame)
    {
        loadScene.LoadSaveGame(1, indexOfSaveGame);
    }

    public void OpenDeleteSaveGame(int indexOfSaveGame)
    {
        deleteSave.SetActive(true);

        Button yesButton = deleteSave.GetComponentsInChildren<Button>()[0];

        yesButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(delegate { DeleteSaveGame(indexOfSaveGame); });
        yesButton.onClick.AddListener(delegate { principalMenu.PlayButtonClip(); });
    }

    public void CloseDeleteSaveGame()
    {
        deleteSave.SetActive(false);
    }

    public void DeleteSaveGame(int indexOfSaveGame)
    {
        deleteSave.SetActive(false);

        Button[] saves = spawnLocation.GetComponentsInChildren<Button>();

        TextMeshProUGUI[] text = saves[indexOfSaveGame - 1].GetComponentsInChildren<TextMeshProUGUI>();

        text[0].text = "Joc nou";

        text[1].gameObject.SetActive(false);
        text[2].gameObject.SetActive(false);

        saves[indexOfSaveGame - 1].onClick.RemoveAllListeners();
        saves[indexOfSaveGame - 1].GetComponent<Button>().onClick.AddListener(delegate { SelectNewSaveGame(indexOfSaveGame); });

        string pathToSaveGame = path + @"\SaveGame" + indexOfSaveGame + ".svj";

        File.Delete(pathToSaveGame);
    }
}
