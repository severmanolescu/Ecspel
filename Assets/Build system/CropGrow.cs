using UnityEngine;

public class CropGrow : MonoBehaviour
{
    private Crop item;

    private int startDay;

    private int currentSprite = 0;

    private GridNode gridNode;

    private SkillsHandler skillHandler;

    private SpawnItem spawnItem;

    private bool destroyed = false;

    public bool Destroyed { get { return destroyed; } }

    public Crop Item { get => item; set => item = value; }
    public int StartDay { get => startDay; set => startDay = value; }
    public int CurrentSprite { get => currentSprite; set { currentSprite = value; ChangeSprite(); } }

    private void Awake()
    {
        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

        skillHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();
    }

    private void Start()
    {
        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().CropAddList(this);

        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    public void SetItem(Item item, GridNode gridNode, GameObject itemWorldPrefab)
    {
        this.Item = (Crop)item;

        this.gridNode = gridNode;
    }

    public void DayChange(int day)
    {
        if (day >= StartDay + Item.DayToGrow)
        {
            CurrentSprite++;

            if (CurrentSprite < Item.Levels.Count)
            {
                ChangeSprite();
            }
            else
            {
                CurrentSprite = Item.Levels.Count - 1;
            }

            if (CurrentSprite == 2)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }

            StartDay = day;
        }
    }

    private void SpawnSeeds()
    {
        int chance = Random.Range(0, 100);

        if (chance <= skillHandler.FarmingLevel * 5)
        {
            spawnItem.SpawnItems(Item, 1, transform.position);
        }
    }

    private void DestroyNotRefil()
    {
        if (CurrentSprite == Item.Levels.Count - 1)
        {
            int amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (amount == 0)
            {
                amount = 1;
            }

            spawnItem.SpawnItems(Item.CropItem, amount, transform.position);

            SpawnSeeds();
        }
        else if (CurrentSprite == Item.Levels.Count - 2)
        {
            spawnItem.SpawnItems(Item.CropItem, 1, transform.position);

            SpawnSeeds();
        }

        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

        if (CurrentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = Item.Destroy1;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            destroyed = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void DestroyRefil()
    {
        if (CurrentSprite == Item.Levels.Count - 2)
        {
            int amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (amount == 0)
            {
                amount = 1;
            }

            spawnItem.SpawnItems(Item.CropItem, amount, transform.position);

            SpawnSeeds();
        }
        else if (CurrentSprite == Item.Levels.Count - 3)
        {
            spawnItem.SpawnItems(Item.CropItem, 1, transform.position);

            SpawnSeeds();
        }

        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

        if (CurrentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = Item.Destroy1;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            destroyed = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Destroy()
    {
        GameObject.Find("BuildSystem").GetComponent<BuildSystemHandler>().ChangeGridCropPlaced(gridNode, true);

        if (Item.Refil == false)
        {
            DestroyNotRefil();
        }
        else
        {
            DestroyRefil();
        }
    }

    public bool DestroyCropByHoe()
    {
        if (HarverstCrop() == false)
        {
            if (currentSprite == 0)
            {
                spawnItem.SpawnItems(Item, 1, transform.position);
            }

            return true;
        }

        return false;
    }

    public void DestroyCropOnLoadGame()
    {
        GameObject.Find("BuildSystem").GetComponent<BuildSystemHandler>().ChangeGridCropPlaced(gridNode, true);

        if (CurrentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = Item.Destroy1;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            destroyed = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void DryCrop()
    {
        GameObject.Find("BuildSystem").GetComponent<BuildSystemHandler>().ChangeGridCropPlaced(gridNode, true);

        GetComponent<SpriteRenderer>().sprite = Item.Destroy1;
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        destroyed = true;
    }

    private bool HarvestNotRefil()
    {
        if (CurrentSprite == Item.Levels.Count - 1)
        {
            int amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (amount == 0)
            {
                amount = 1;
            }

            spawnItem.SpawnItems(Item.CropItem, amount, transform.position);

            GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

            GetComponent<SpriteRenderer>().sprite = Item.Destroy1;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            destroyed = true;

            SpawnSeeds();

            return true;
        }

        return false;
    }

    private bool HarvestRefil()
    {
        if (CurrentSprite == Item.Levels.Count - 1)
        {
            int amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (amount == 0)
            {
                amount = 1;
            }

            spawnItem.SpawnItems(Item.CropItem, amount, transform.position);

            CurrentSprite = Item.Levels.Count - Item.RefilDecreseSpriteIndexStart - 1;

            GetComponent<SpriteRenderer>().sprite = Item.Levels[currentSprite];

            SpawnSeeds();

            return true;
        }

        return false;
    }

    public bool HarverstCrop()
    {
        if (Destroyed == false)
        {
            if (Item.Refil == false)
            {
                return HarvestNotRefil();
            }
            else
            {
                return HarvestRefil();
            }
        }

        return false;
    }

    private void ChangeSprite()
    {
        if (currentSprite < Item.Levels.Count)
        {
            GetComponent<SpriteRenderer>().sprite = Item.Levels[CurrentSprite];
        }
    }
}
