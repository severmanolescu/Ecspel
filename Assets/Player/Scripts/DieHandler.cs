using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects = new List<GameObject>();

    private Image image;

    private Animator animator;

    private SaveSystemHandler saveSystem;

    private PrincipalMenuHandler menuHandler;

    private PlayerStats playerStats;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        saveSystem = GameObject.Find("Global/SaveSystem").GetComponent<SaveSystemHandler>();
        menuHandler = GameObject.Find("Global/Menu").GetComponent<PrincipalMenuHandler>();
        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        playerStats.DieHandler = this;

        image = GetComponent<Image>();
    }

    private void Start()
    {
        DeactivateAllObjects();
    }

    private void DeactivateAllObjects()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        image.enabled = false;
    }

    public void ActivateAllObjects()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        image.enabled = true;
    }

    public void LoadSaveGame()
    {
        DeactivateAllObjects();

        saveSystem.LoadSaveGame();

        playerStats.Revive();
    }

    public void GoToMenu()
    {
        DeactivateAllObjects();

        menuHandler.BackToMenu();
    }

    public void Die()
    {
        animator.SetBool("Start", true);
    }
}
