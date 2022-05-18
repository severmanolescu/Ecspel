using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FinalTextHandler : MonoBehaviour
{
    private TextMeshProUGUI text;

    private Button[] buttons;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        buttons = GetComponentsInChildren<Button>();

        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void StartShowText()
    {
        text.text = "Va urma";

        Color newColor = Color.white;
        newColor.a = 0f;
        text.color = newColor;

        text.gameObject.SetActive(true);

        StartCoroutine(WaitForText());
    }

    private IEnumerator WaitForText()
    {
        Color startColor = text.color;

        while (startColor.a < 1f)
        {
            startColor.a += 0.01f;

            text.color = startColor;

            yield return new WaitForSeconds(0.01f);
        }

        text.text += ".";
        yield return new WaitForSeconds(1f);
        text.text += ".";
        yield return new WaitForSeconds(1f);
        text.text += ".";
        yield return new WaitForSeconds(1f);

        while (startColor.a >= 0f)
        {
            startColor.a -= 0.01f;

            text.color = startColor;

            yield return new WaitForSeconds(0.01f);
        }

        text.text = "Sooth";

        while (startColor.a < 1f)
        {
            startColor.a += 0.01f;

            text.color = startColor;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);

        foreach (var button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void LoadMainScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
