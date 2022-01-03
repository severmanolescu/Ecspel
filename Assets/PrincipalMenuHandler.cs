using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;

public class PrincipalMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject principalMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject quitConsole;

    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Slider volumeSlider;

    [SerializeField] private TMP_Dropdown resolutionDropDown;

    private string pathToSaveSettings;

    private Resolution[] resolutions;

    private void Awake()
    {
        settingsMenu.SetActive(false);
        quitConsole.SetActive(false);
    }

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.RefreshShownValue();

        pathToSaveSettings = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        pathToSaveSettings = Path.Combine(pathToSaveSettings, @"Sooth\Sooth.ini");

        if(!File.Exists(pathToSaveSettings))
        {
            CreateDirectoryIni(currentResolutionIndex);
        }
        else
        {
            ReadDataFromIni();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(settingsMenu.activeSelf == true)
            {
                settingsMenu.SetActive(false);
                principalMenu.SetActive(true);
            }
            else if(quitConsole.activeSelf == true)
            {
                quitConsole.SetActive(false);
                principalMenu.SetActive(true);
            }
            else
            {
                settingsMenu.SetActive(false);
                principalMenu.SetActive(false);
                quitConsole.SetActive(true);
            }
        }
    }

    private void ReadDataFromIni()
    {
        string[] iniLines = File.ReadAllLines(pathToSaveSettings);

        foreach (string line in iniLines)
        {
            if (line.Contains("FullScreen"))
            {
                try
                {
                    bool fullScreen = bool.Parse(line.Substring(line.IndexOf(":") + 1));

                    fullScreenToggle.isOn = fullScreen;

                    ChangeFullScreed();
                }
                catch
                {
                    fullScreenToggle.isOn = true;

                    ChangeFullScreed();
                }
            }
            else if (line.Contains("Resolution"))
            {
                try
                {
                    int index = int.Parse(line.Substring(line.IndexOf(":") + 1));

                    resolutionDropDown.value = index;
                    resolutionDropDown.RefreshShownValue();

                    ChangeResolution();
                }
                catch
                {
                    int lastIndex = resolutionDropDown.options.Count - 1;

                    resolutionDropDown.value = lastIndex;
                    resolutionDropDown.RefreshShownValue();

                    ChangeResolution();
                }
            }
            else if (line.Contains("Quality"))
            {
                try
                {
                    int index = int.Parse(line.Substring(line.IndexOf(":") + 1));

                    qualityDropdown.value = index;
                    qualityDropdown.RefreshShownValue();

                    ChangeQuality();
                }
                catch
                {
                    int lastIndex = qualityDropdown.options.Count - 1;

                    qualityDropdown.value = lastIndex;
                    qualityDropdown.RefreshShownValue();

                    ChangeQuality();
                }
            }
            else if (line.Contains("Volume"))
            {
                try
                {
                    float value = float.Parse(line.Substring(line.IndexOf(":") + 1));

                    volumeSlider.value = value;

                    ChangeVolume();
                }
                catch
                {
                    volumeSlider.value = 1f;

                    ChangeVolume();
                }
            }
        }
    }

    private void CreateDirectoryIni(int currentResolutionIndex)
    {
        resolutionDropDown.value = currentResolutionIndex;

        string directoryPath = pathToSaveSettings.Substring(0, pathToSaveSettings.Length - 10);

        if (!System.IO.Directory.Exists(directoryPath))
        {
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        FileStream file =  File.Create(pathToSaveSettings);

        file.Close();

        WriteDataToIni();
    }

    private void WriteDataToIni()
    {
        if (File.Exists(pathToSaveSettings))
        {

            string text = "";

            text += "FullScreen:" + fullScreenToggle.isOn + "\n";
            text += "Resolution:" + resolutionDropDown.value + "\n";
            text += "Quality:" + qualityDropdown.value + "\n";
            text += "Volume:" + volumeSlider.value + "\n";

            File.WriteAllText(pathToSaveSettings, text);
        }
        else
        {
            CreateDirectoryIni(resolutionDropDown.options.Count - 1);
        }
    }

    public void OpenSettings()
    {
        principalMenu.SetActive(false);
        settingsMenu.SetActive(true);
        quitConsole.SetActive(false);
    }

    public void OpenPrincipalMenu()
    {        
        settingsMenu.SetActive(false);
        principalMenu.SetActive(true);
        quitConsole.SetActive(false);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenQuitConcole()
    {
        settingsMenu.SetActive(false);
        principalMenu.SetActive(false);

        quitConsole.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ChangeFullScreed()
    {
        Screen.fullScreen = fullScreenToggle.isOn;

        WriteDataToIni();
    }

    public void ChangeResolution()
    {
        Screen.SetResolution(resolutions[resolutionDropDown.value].width, resolutions[resolutionDropDown.value].height, fullScreenToggle.isOn);

        WriteDataToIni();
    }

    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value);

        WriteDataToIni();
    }

    public void ChangeVolume()
    {

    }

    public void ResumePlay()
    {
        principalMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitConsole.SetActive(false);
    }
}
