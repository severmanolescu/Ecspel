using UnityEngine;

public class CropGrow : MonoBehaviour
{
    private GameObject itemWorldPrefab;

    private Crop item;

    private int startDay;

    private int currentSprite = 0;

    private GridNode gridNode;

    private SkillsHandler skillHandler;

    private bool destroyed = false;

    public bool Destroyed { get { return destroyed; } }

    public Crop Item { get => item; set => item = value; }
    public int StartDay { get => startDay; set => startDay = value; }
    public int CurrentSprite { get => currentSprite; set { currentSprite = value; ChangeSprite(); } }

    private void Awake()
    {
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

        this.itemWorldPrefab = itemWorldPrefab;
    }

    public void DayChange(int day)
    {
        if (day >= StartDay + Item.DayToGrow)
        {
            CurrentSprite++;

            if (Item.Refil == false)
            {
                if (CurrentSprite < Item.Levels.Count)
                {
                    ChangeSprite();
                }
                else
                {
                    CurrentSprite = Item.Levels.Count - 1;
                }
            }
            else
            {
                if (CurrentSprite == Item.Levels.Count - 1)
                {
                    CurrentSprite -= Item.RefilDecreseSpriteIndexStart;

                    ChangeSprite();
                }
                else if (CurrentSprite <= Item.Levels.Count - 2)
                {
                    ChangeSprite();

                    if (CurrentSprite == Item.Levels.Count - 2)
                    {
                        CurrentSprite--;
                    }
                }
                else
                {
                    CurrentSprite = Item.Levels.Count - 2;
                }
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
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.Copy();
            drop.Amount = 1;

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }
    }

    private void DestroyNotRefil()
    {
        if (CurrentSprite == Item.Levels.Count - 1)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.CropItem.Copy();
            drop.Amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (drop.Amount == 0)
            {
                drop.Amount = 1;
            }

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            SpawnSeeds();
        }
        else if (CurrentSprite == Item.Levels.Count - 2)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.CropItem.Copy();
            drop.Amount = 1;

            if (drop.Amount == 0)
            {
                drop.Amount = 1;
            }

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

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
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.CropItem.Copy();
            drop.Amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (drop.Amount == 0)
            {
                drop.Amount = 1;
            }

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            SpawnSeeds();
        }
        else if (CurrentSprite == Item.Levels.Count - 3)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.CropItem.Copy();
            drop.Amount = 1;

            if (drop.Amount == 0)
            {
                drop.Amount = 1;
            }

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

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
                ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

                itemWorld.transform.position = transform.position;

                Item drop = Item.Copy();
                drop.Amount = 1;

                itemWorld.SetItem(drop);
                itemWorld.MoveToPoint();
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
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.CropItem.Copy();
            drop.Amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (drop.Amount == 0)
            {
                drop.Amount = 1;
            }

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

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
        if (CurrentSprite == Item.Levels.Count - 3)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = Item.CropItem.Copy();
            drop.Amount = Random.Range(Item.MinDrop, Item.MaxDrop);

            if (drop.Amount == 0)
            {
                drop.Amount = 1;
            }

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            StartDay = GameObject.Find("DayTimer").GetComponent<DayTimerHandler>().Days;

            CurrentSprite = Item.Levels.Count - 1;

            GetComponent<SpriteRenderer>().sprite = Item.Levels[CurrentSprite];

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
