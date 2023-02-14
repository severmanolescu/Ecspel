using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftCanvasHandler : MonoBehaviour
{
    [SerializeField] private List<Craft> initialCrafts = new List<Craft>();

    [SerializeField] private GameObject craftPrefab;
    [SerializeField] private Transform spawnLocation;

    [SerializeField] private NewRecipeHandler newRecipeHandler;

    [SerializeField] private Color activeButtonColor;
    [SerializeField] private Color nonActiveButtonColor;

    [SerializeField] private List<Button> filterButtons;

    private List<CraftSetData> crafts = new List<CraftSetData>();

    private AudioSource audioSource;

    private CraftingHandler craftingHandler;

    public CraftingHandler CraftingHandler { get => craftingHandler; set => craftingHandler = value; }

    private void Start()
    {
        foreach (Craft craft in initialCrafts)
        {
            InstantiateCraft(craft);
        }

        ReinitializeAllCraftings();

        audioSource = GetComponent<AudioSource>();

        gameObject.SetActive(false);
    }

    public void ReinitializeAllCraftings()
    {
        foreach (CraftSetData craft in crafts)
        {
            craft.CheckIfItemsAreAvaible();
        }
    }

    public void PlaySoundEffect()
    {
        audioSource.Play();
    }

    private bool SearchIfCraftExist(Craft craft)
    {
        foreach (CraftSetData craftSet in crafts)
        {
            if (craftSet.Craft == craft)
            {
                return true;
            }
        }

        return false;
    }

    public void Close()
    {
        if(crafts != null)
        {
            ShowAllCrafts();
        }

        if (CraftingHandler != null)
        {
            craftingHandler.CloseCraft();
        }
    }

    public List<Craft> GetAllCrafts()
    {
        List<Craft> list = new List<Craft>();

        foreach (CraftSetData craftSet in crafts)
        {
            if (craftSet.Craft != null)
            {
                list.Add(craftSet.Craft);
            }
        }

        return list;
    }

    public void DeleteAllCrafts()
    {
        foreach (CraftSetData craftSet in crafts)
        {
            if (craftSet.Craft != null)
            {
                Destroy(craftSet.gameObject);
            }
        }

        crafts.Clear();
    }

    public bool AddCraft(Craft craft, bool showAnimation = true)
    {
        if (craft != null && SearchIfCraftExist(craft) == false)
        {
            InstantiateCraft(craft);

            if (showAnimation == true)
            {
                newRecipeHandler.AddNewRecipie(craft.ReceiveItem.Item.ItemSprite);
            }

            return true;
        }

        return false;
    }

    private void InstantiateCraft(Craft craft)
    {
        CraftSetData newCraft = Instantiate(craftPrefab, spawnLocation).GetComponent<CraftSetData>();

        newCraft.Craft = craft;

        newCraft.CanvasHandler = this;

        crafts.Add(newCraft);
    }

    public void ShowAllCrafts()
    {
        foreach (CraftSetData craft in crafts)
        {
            craft.gameObject.SetActive(true);
        }

        ChangeButtonColor(0);
    }

    private void ChangeButtonColor(int buttonNo)
    {
        if(filterButtons != null && filterButtons.Count > buttonNo)
        {
            for(ushort buttonIndex = 0; buttonIndex < filterButtons.Count; buttonIndex++)
            {
                ColorBlock colorBlock = filterButtons[buttonIndex].colors;

                if (buttonNo != buttonIndex)
                {
                    colorBlock.normalColor = nonActiveButtonColor;    
                    colorBlock.selectedColor = nonActiveButtonColor;    
                }
                else
                {
                    colorBlock.normalColor = activeButtonColor;
                    colorBlock.selectedColor = activeButtonColor;
                }

                filterButtons[buttonIndex].colors = colorBlock;
            }
        }
    }

    private void ShowOnlyCraftForFilter(int filter)
    {
        foreach (CraftSetData craft in crafts)
        {
            if (craft.Craft.FilterType == filter)
            {
                craft.gameObject.SetActive(true);
            }
            else
            {
                craft.gameObject.SetActive(false);
            }
        }

        ChangeButtonColor(filter + 1);
    }

    public void SelectFilter(int filter)
    {
        if (crafts != null)
        {
            if (filter == -1)
            {
                ShowAllCrafts();
            }
            else
            {
                ShowOnlyCraftForFilter(filter);
            }
        }
    }
}
