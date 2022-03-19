using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PrincipalMenuHandler : MonoBehaviour
{
    [Header("Load game object")]
    [SerializeField] private LoadSceneHandler loadSceneHandler;

    [Header("Principal buttons")]
    [SerializeField] private GameObject principalMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject quitConsole;

    [Header("Settings")]
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Slider volumePrincipalSlider;
    [SerializeField] private Slider volumeEffectSlider;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropDown;

    [Header("Load save game")]
    [SerializeField] private GameObject loadGameMenu;
    [SerializeField] private GameObject deleteSave;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject deleteButton;

    [Header("Open principal scene")]
    [SerializeField] private GameObject openMenu;

    [Header("Audio effects")]
    [SerializeField] private AudioClip buttonClick;
    [SerializeField] private AudioMixer effectAudioMixer;
    [SerializeField] private AudioMixer principalAudioMixer;

    [SerializeField] private bool openQuitConsoleWithEsc = true;

    private CanvasTabsOpen canvasTabs;

    private Keyboard keyboard;

    private AudioSource audioSource;

    private string pathToSaveSettings;

    private Resolution[] resolutions;

    private void Awake()
    {
        settingsMenu.SetActive(false);
        quitConsole.SetActive(false);

        if (loadGameMenu != null)
        {
            loadGameMenu.SetActive(false);
        }

        if (openMenu != null)
        {
            openMenu.SetActive(false);
        }

        canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        audioSource = GetComponent<AudioSource>();

        keyboard = InputSystem.GetDevice<Keyboard>();
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

            ChangeFullScreed();
            ChangeResolution();
            ChangeQuality();
            ChangeVolume();
            ChangeEffectsVolume();
        }
        else
        {
            ReadDataFromIni();
        }
    }

    private void Update()
    {
        if(keyboard.escapeKey.wasPressedThisFrame)
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
            else if (deleteSave != null && deleteSave.activeSelf == true)
            {
                deleteSave.SetActive(false);
            }
            else if (loadGameMenu != null && loadGameMenu.activeSelf == true)
            {
                loadGameMenu.SetActive(false);
                principalMenu.SetActive(true);

                HideSaveButtons();
            }
            else if(openMenu != null && openMenu.activeSelf == true)
            {
                openMenu.SetActive(false);
                principalMenu.SetActive(true);
            }
            else
            {
                if (openQuitConsoleWithEsc == true)
                {
                    settingsMenu.SetActive(false);
                    principalMenu.SetActive(false);

                    quitConsole.SetActive(true);
                }
                else
                {
                    canvasTabs.CloseMenu();

                    CloseMenu();
                }
            }
        }
    }

    public void HideSaveButtons()
    {
        playButton.SetActive(false);
        deleteButton.SetActive(false);
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
                }
                catch
                {
                    fullScreenToggle.isOn = true; 
                }
            }
            else if (line.Contains("Resolution"))
            {
                try
                {
                    int index = int.Parse(line.Substring(line.IndexOf(":") + 1));

                    resolutionDropDown.value = index;
                    resolutionDropDown.RefreshShownValue();
                }
                catch
                {
                    int lastIndex = resolutionDropDown.options.Count - 1;

                    resolutionDropDown.value = lastIndex;
                    resolutionDropDown.RefreshShownValue();
                }
            }
            else if (line.Contains("Quality"))
            {
                try
                {
                    int index = int.Parse(line.Substring(line.IndexOf(":") + 1));

                    qualityDropdown.value = index;
                    qualityDropdown.RefreshShownValue();
                }
                catch
                {
                    int lastIndex = qualityDropdown.options.Count - 1;

                    qualityDropdown.value = lastIndex;
                    qualityDropdown.RefreshShownValue();
                }
            }
            else if (line.Contains("VolumePrincipal"))
            {
                try
                {
                    float value = float.Parse(line.Substring(line.IndexOf(":") + 1));

                    volumePrincipalSlider.value = value;
                }
                catch
                {
                    volumePrincipalSlider.value = volumePrincipalSlider.maxValue;
                }
            }
            else if (line.Contains("VolumeEffect"))
            {
                try
                {
                    float value = float.Parse(line.Substring(line.IndexOf(":") + 1));

                    volumeEffectSlider.value = value;
                }
                catch
                {
                    volumeEffectSlider.value = volumeEffectSlider.maxValue;
                }
            }
        }

        ChangeFullScreed();
        ChangeResolution();
        ChangeQuality();
        ChangeVolume();
        ChangeEffectsVolume();
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
            text += "VolumePrincipal:" + volumePrincipalSlider.value + "\n";
            text += "VolumeEffect:" + volumeEffectSlider.value + "\n";

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

        if (loadGameMenu != null)
        {
            loadGameMenu.SetActive(false);
        }
    }

    public void OpenLoadGame()
    {
        principalMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitConsole.SetActive(false);
        loadGameMenu.SetActive(true);
    }

    public void OpenPrincipalMenu()
    {        
        settingsMenu.SetActive(false);
        principalMenu.SetActive(true);
        quitConsole.SetActive(false);

        if (loadGameMenu != null)
        {
            loadGameMenu.SetActive(false);
        }

        if (openMenu != null)
        {
            openMenu.SetActive(false);
        }
    }

    public void PlayButton(int indexOfSaveGame)
    {
        loadGameMenu.gameObject.SetActive(true);

        loadSceneHandler.LoadScene(1, indexOfSaveGame);

        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
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

    public void ResumePlay()
    {
        principalMenu.SetActive(false);
        settingsMenu.SetActive(false);
        quitConsole.SetActive(false);
    }

    public void ChangeVolume()
    {
        principalAudioMixer.SetFloat("Volume", volumePrincipalSlider.value);

        WriteDataToIni();
    }

    public void ChangeEffectsVolume()
    {
        effectAudioMixer.SetFloat("Volume", volumeEffectSlider.value);

        WriteDataToIni();
    }

    public void PlayButtonClip()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
    }

    public void OpenMenuConsole()
    {
        openMenu.SetActive(true);

        principalMenu.SetActive(false);
    }

    public void CloseMenuConsole()
    {
        openMenu.SetActive(false);

        principalMenu.SetActive(true);
    }

    public void CloseMenu()
    {
        Time.timeScale = 1f;

        gameObject.SetActive(false);
    }
}
