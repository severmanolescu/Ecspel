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

    private int dotNo;

    public bool FinishGridSearchProcess { get => finishGridSearchProcess; set { finishGridSearchProcess = value; } }

    private void Awake()
    {
        loadText = loadObject.GetComponentInChildren<TextMeshProUGUI>();

        loadObject.SetActive(false);

        DontDestroyOnLoad(this);

        dotNo = 0;
    }

    public void LoadScene(int sceneIndex, int indexOfSaveGame)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex, indexOfSaveGame));
    }

    IEnumerator LoadAsynchronously(int sceneIndex, int indexOfSaveGame)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            yield return null;
        }

        NewGameLoadingHandler newGameLoading = GameObject.Find("GameLoading").GetComponent<NewGameLoadingHandler>();

        newGameLoading.StartNewGame(this);

        loadObject.SetActive(true);

        loadText.text = "Loading.";

        while(FinishGridSearchProcess == false)
        {
            yield return new WaitForSeconds(1);

            LoadingDotChange();
        }

        loadObject.SetActive(false);

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

        while (!operation.isDone)
        {
            yield return null;
        }

        SaveSystemHandler saveSystem = GameObject.Find("SaveSystem").GetComponent<SaveSystemHandler>();

        saveSystem.LoadSaveGame(indexOfSaveGame, this);

        loadText.text = "Loading.";

        dotNo = 0;

        while (FinishGridSearchProcess == false)
        {
            yield return new WaitForSeconds(1);

            LoadingDotChange();
        }

        yield return new WaitForSeconds(2);

        loadObject.SetActive(false);

        Destroy(gameObject);
    }
}
