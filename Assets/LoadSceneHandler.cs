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

    public bool FinishGridSearchProcess { get => finishGridSearchProcess; set { finishGridSearchProcess = value; } }

    private void Awake()
    {
        loadText = loadObject.GetComponentInChildren<TextMeshProUGUI>();

        loadObject.SetActive(false);

        DontDestroyOnLoad(this);
    }

    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            yield return null;
        }

        NewGameLoadingHandler newGameLoading = GameObject.Find("NewGameLoading").GetComponent<NewGameLoadingHandler>();

        newGameLoading.StartNewGame(this);

        loadObject.SetActive(true);

        loadText.text = "Loading.";

        int dotNo = 0;

        while(FinishGridSearchProcess == false)
        {
            yield return new WaitForSeconds(1);

            if(dotNo >= 2)
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

        loadObject.SetActive(false);

        Destroy(gameObject);
    }
}
