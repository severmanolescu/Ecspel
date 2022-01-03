using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadSceneHandler : MonoBehaviour
{
    [SerializeField] private GameObject loadObject;

    private int finishGridSearchProcess = 0;

    private TextMeshProUGUI loadText;

    public int FinishGridSearchProcess { get => finishGridSearchProcess; set { finishGridSearchProcess = value; Debug.Log(FinishGridSearchProcess); } }

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

        loadObject.SetActive(true);

        loadText.text = "Loading.";

        int dotNo = 0;

        while(FinishGridSearchProcess < 5)
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
