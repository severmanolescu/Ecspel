using UnityEngine;

public class CropGrow : MonoBehaviour
{
    private GameObject itemWorldPrefab;

    private Crop item;

    private int startDay;

    private int currentSprite = 0;

    private GridNode gridNode;

    private bool destroyed = false;

    public bool Destroyed { get { return destroyed; } }

    private void Start()
    {
        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().CropAddList(this);

        gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
    }

    public void SetItem(Item item, GridNode gridNode, GameObject itemWorldPrefab)
    {
        this.item = (Crop)item;

        this.gridNode = gridNode;

        this.itemWorldPrefab = itemWorldPrefab;
    }

    public void DayChange(int day)
    {
        if(day >= startDay + item.DayToGrow)
        {
            currentSprite++;

            if (item.Refil == false)
            {
                if (currentSprite < item.Levels.Count)
                {
                    GetComponent<SpriteRenderer>().sprite = item.Levels[currentSprite];
                }
                else
                {
                    currentSprite = item.Levels.Count - 1;
                }
            }
            else
            {
                if (currentSprite == item.Levels.Count - 1)
                {
                    currentSprite -= item.RefilDecreseSpriteIndexStart;

                    GetComponent<SpriteRenderer>().sprite = item.Levels[currentSprite];
                }
                else if (currentSprite <= item.Levels.Count - 2)
                {
                    GetComponent<SpriteRenderer>().sprite = item.Levels[currentSprite];

                    if(currentSprite == item.Levels.Count - 2)
                    {
                        currentSprite--;
                    }
                }
                else
                {
                    currentSprite = item.Levels.Count - 2;
                }
            }

            if(currentSprite == 2)
            {
                GetComponent<SpriteRenderer>().sortingOrder = 0;
            }

            startDay = day;
        }
    }

    private void DestroyNotRefil()
    {
        if (currentSprite == item.Levels.Count - 1)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.CropItem.Copy();
            drop.Amount = Random.Range(item.MinDrop, item.MaxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }
        else if (currentSprite == item.Levels.Count - 2)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.CropItem.Copy();
            drop.Amount = 1;

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }

        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

        if (currentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = item.Destroy1;
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
        if (currentSprite == item.Levels.Count - 2)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.CropItem.Copy();
            drop.Amount = Random.Range(item.MinDrop, item.MaxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }
        else if (currentSprite == item.Levels.Count - 3)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.CropItem.Copy();
            drop.Amount = 1;

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }

        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

        if (currentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = item.Destroy1;
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

        if (item.Refil == false)
        {
            DestroyNotRefil();
        }
        else
        {
            DestroyRefil();
        }
    }

    private void HarvestNotRefil()
    {
        if (currentSprite == item.Levels.Count - 1)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.CropItem.Copy();
            drop.Amount = Random.Range(item.MinDrop, item.MaxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

            GetComponent<SpriteRenderer>().sprite = item.Destroy1;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            destroyed = true;
        }
    }

    private void HarvestRefil()
    {
        if (currentSprite == item.Levels.Count - 3)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.CropItem.Copy();
            drop.Amount = Random.Range(item.MinDrop, item.MaxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            startDay = GameObject.Find("DayTimer").GetComponent<DayTimerHandler>().Days;

            currentSprite = item.Levels.Count - 1;

            GetComponent<SpriteRenderer>().sprite = item.Levels[currentSprite];
        }
    }

    public void HarverstCrop()
    {
        if(item.Refil == false)
        {
            HarvestNotRefil();
        }
        else
        {
            HarvestRefil();
        }
    }
}
