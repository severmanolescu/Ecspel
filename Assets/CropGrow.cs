using UnityEngine;
using UnityEditor;

public class CropGrow : MonoBehaviour
{
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

    public void SetItem(Item item, GridNode gridNode)
    {
        this.item = (Crop)item;

        this.gridNode = gridNode;
    }

    public void DayChange(int day)
    {
        if(day >= startDay + item.dayToGrow)
        {
            currentSprite++;

            if (item.refil == false)
            {
                if (currentSprite < item.levels.Count)
                {
                    GetComponent<SpriteRenderer>().sprite = item.levels[currentSprite];
                }
                else
                {
                    currentSprite = item.levels.Count - 1;
                }
            }
            else
            {
                if (currentSprite == item.levels.Count - 1)
                {
                    currentSprite -= item.refilDecreseSpriteIndexStart;

                    GetComponent<SpriteRenderer>().sprite = item.levels[currentSprite];
                }
                else if (currentSprite <= item.levels.Count - 2)
                {
                    GetComponent<SpriteRenderer>().sprite = item.levels[currentSprite];

                    if(currentSprite == item.levels.Count - 2)
                    {
                        currentSprite--;
                    }
                }
                else
                {
                    currentSprite = item.levels.Count - 2;
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
        if (currentSprite == item.levels.Count - 1)
        {
            ItemWorld itemWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject))).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.crop.Copy();
            drop.Amount = Random.Range(item.minDrop, item.maxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }
        else if (currentSprite == item.levels.Count - 2)
        {
            ItemWorld itemWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject))).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.crop.Copy();
            drop.Amount = 1;

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }

        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

        if (currentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = item.destroy;
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
        if (currentSprite == item.levels.Count - 2)
        {
            ItemWorld itemWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject))).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.crop.Copy();
            drop.Amount = Random.Range(item.minDrop, item.maxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }
        else if (currentSprite == item.levels.Count - 3)
        {
            ItemWorld itemWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject))).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.crop.Copy();
            drop.Amount = 1;

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();
        }

        GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

        if (currentSprite >= 2)
        {
            GetComponent<SpriteRenderer>().sprite = item.destroy;
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

        if (item.refil == false)
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
        if (currentSprite == item.levels.Count - 1)
        {
            ItemWorld itemWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject))).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.crop.Copy();
            drop.Amount = Random.Range(item.minDrop, item.maxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            GameObject.Find("DayTimer").GetComponent<CropGrowHandler>().RemoveCropList(this);

            GetComponent<SpriteRenderer>().sprite = item.destroy;
            GetComponent<SpriteRenderer>().sortingOrder = -1;
            destroyed = true;
        }
    }

    private void HarvestRefil()
    {
        Debug.Log(item.levels.Count - 3 + " " + currentSprite);

        if (currentSprite == item.levels.Count - 3)
        {
            ItemWorld itemWorld = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject))).GetComponent<ItemWorld>();

            itemWorld.transform.position = transform.position;

            Item drop = item.crop.Copy();
            drop.Amount = Random.Range(item.minDrop, item.maxDrop);

            itemWorld.SetItem(drop);
            itemWorld.MoveToPoint();

            startDay = GameObject.Find("DayTimer").GetComponent<DayTimerHandler>().Days;

            currentSprite = item.levels.Count - 1;

            GetComponent<SpriteRenderer>().sprite = item.levels[currentSprite];
        }
    }

    public void HarverstCrop()
    {
        if(item.refil == false)
        {
            HarvestNotRefil();
        }
        else
        {
            HarvestRefil();
        }
    }
}
