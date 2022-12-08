using System.Collections.Generic;
using UnityEngine;

public class CraftCanvasHandler : MonoBehaviour
{
    private List<CraftSetData> crafts = new List<CraftSetData>();

    [SerializeField] private List<Craft> initialCrafts = new List<Craft>();

    [SerializeField] private GameObject craftPrefab;
    [SerializeField] private Transform spawnLocation;

    [SerializeField] private NewRecipeHandler newRecipeHandler;
    [SerializeField] private ItemSprites itemSprites;

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
}
