using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadSceneHandler : MonoBehaviour
{
    [SerializeField] private GameObject loadObject;

    private bool finishGridSearchProcess = false;

    private TextMeshProUGUI loadText;

    private static LoadSceneHandler loadSceneHandler;

    private int dotNo;

    public bool FinishGridSearchProcess { get => finishGridSearchProcess; set { finishGridSearchProcess = value; } }

    private void Awake()
    {
        loadText = loadObject.GetComponentInChildren<TextMeshProUGUI>();

        loadObject.SetActive(false);

        DontDestroyOnLoad(this);

        if (loadSceneHandler == null)
        {
            loadSceneHandler = this;
        }
        else
        {
            Destroy(gameObject);
        }

        dotNo = 0;
    }

    public void LoadScene(int sceneIndex, int indexOfSaveGame)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex, indexOfSaveGame));
    }

    IEnumerator LoadAsynchronously(int sceneIndex, int indexOfSaveGame)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadObject.SetActive(true);

        loadText.text = "Loading.";

        while (!operation.isDone)
        {
            yield return new WaitForSeconds(1);

            LoadingDotChange();
        }

        PlayerMovement playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
        CanvasTabsOpen canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        playerMovement.TabOpen = true;

        canvasTabs.canOpenTabs = false;

        NewGameLoadingHandler newGameLoading = GameObject.Find("GameLoading").GetComponent<NewGameLoadingHandler>();

        newGameLoading.StartNewGame(this);

        while(FinishGridSearchProcess == false)
        {
            yield return new WaitForSeconds(1);

            LoadingDotChange();
        }

        loadObject.SetActive(false);

        SaveSystemHandler saveSystem = GameObject.Find("SaveSystem").GetComponent<SaveSystemHandler>();

        saveSystem.IndexOfSaveGame = indexOfSaveGame;

        playerMovement.TabOpen = false;

        canvasTabs.canOpenTabs = true;

        GameObject.Find("Global/Player/Canvas/Help").GetComponent<HelpHandler>().StartHelp();

        Destroy(gameObject);
    }

    public void LoadSaveGame(int sceneIndex, int indexOfSaveGame)
    {
        StartCoroutine(LoadAsynchronouslySaveGame(sceneIndex, indexOfSaveGame));
    }

    private void LoadingDotChange()
    {
        if (dotNo >= 2)
        {
            loadText.text = "Loading.";

            dotNo = 0;
        }
        else
        {
            loadText.text += ".";

            dotNo++;
        }
    }

    IEnumerator LoadAsynchronouslySaveGame(int sceneIndex, int indexOfSaveGame)
    {
        loadObject.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadText.text = "Loading.";

        while (!operation.isDone)
        {
            yield return new WaitForSeconds(1);

            LoadingDotChange();
        }

        PlayerMovement playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
        CanvasTabsOpen canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        playerMovement.TabOpen = true;
        canvasTabs.canOpenTabs = false;

        SaveSystemHandler saveSystem = GameObject.Find("SaveSystem").GetComponent<SaveSystemHandler>();

        saveSystem.LoadSaveGame(indexOfSaveGame, this);

        dotNo = 0;

        while (FinishGridSearchProcess == false)
        {
            yield return new WaitForSeconds(1);

            LoadingDotChange();
        }

        yield return new WaitForSeconds(2);

        loadObject.SetActive(false);

        playerMovement.TabOpen = false;

        canvasTabs.canOpenTabs = true;

        Destroy(gameObject);
    }
}
