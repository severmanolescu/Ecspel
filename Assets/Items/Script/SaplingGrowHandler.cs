using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaplingGrowHandler : MonoBehaviour
{
    private Sapling sapling;

    private int currentSprite = 0;

    private int startDay = 0;

    private SpriteRenderer spriteRenderer;

    private uint state;

    public Sapling Sapling { set { sapling = value; } }

    // State:
    // - 0: child
    // - 1: almost mature
    // - 2: mature

    private void Start()
    {
        startDay = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().Days;

        GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().SaplingAddList(this);

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetData(Sapling sapling, int currentSprite, int startDay, uint state)
    {
        this.sapling = sapling;
        this.currentSprite = currentSprite;
        this.startDay = startDay;
        this.state = state;
    }

    public void DayChange(int day)
    {
        if(day >= startDay + sapling.DayToGrow)
        {
            currentSprite++;

            if(currentSprite < sapling.Levels.Count)
            {
                spriteRenderer.sprite = sapling.Levels[currentSprite];

                startDay = day;
            }
            else
            {
                switch(state)
                {
                    case 0:
                        {
                            if(sapling.AlmostMature != null)
                            {
                                state = 1;

                                GameObject tree = Instantiate(sapling.AlmostMature, transform.position, transform.rotation);

                                tree.AddComponent<SaplingGrowHandler>().SetData(sapling, currentSprite, day, state);

                                GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().RemoveSaplingList(this);

                                DestroyObject();

                            }
                            else if(sapling.Mature != null)
                            {
                                state = 2;

                                GameObject tree = Instantiate(sapling.AlmostMature, transform.position, transform.rotation);

                                tree.GetComponent<PositionInGrid>().LocationGrid = GetComponentInParent<LocationGridSave>();

                                DestroyObject();
                            }

                            break;
                        }

                    case 1:
                        {
                            if (sapling.Mature != null)
                            {
                                state = 2;

                                GameObject tree = Instantiate(sapling.AlmostMature, transform.position, transform.rotation);

                                tree.GetComponent<PositionInGrid>().LocationGrid = GetComponentInParent<LocationGridSave>();

                                DestroyObject();
                            }

                            break;
                        }
                }
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
