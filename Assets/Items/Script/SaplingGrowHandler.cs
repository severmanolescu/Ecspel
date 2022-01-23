using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaplingGrowHandler : MonoBehaviour
{
    private Transform newLocationAfterFullGrow;

    private Sapling sapling;

    private int currentSprite = -1;

    private int startDay = 0;

    private SpriteRenderer spriteRenderer;

    private ushort state = 0;

    public Sapling Sapling { set { sapling = value; } get { return sapling; } }
    public int CurrentSprite { get => currentSprite; set => currentSprite = value; }
    public int StartDay { get => startDay; set => startDay = value; }
    public ushort State { get => state; set => state = value; }

    // State:
    // - 0: child
    // - 1: almost mature
    // - 2: mature

    private void Start()
    {
        StartDay = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().Days;

        GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().SaplingAddList(this);

        spriteRenderer = GetComponent<SpriteRenderer>();

        newLocationAfterFullGrow = GameObject.Find("PlayerHouseGround/Environment").transform;
    }

    public void SetData(Sapling sapling, int currentSprite, int startDay, ushort state)
    {
        Sapling = sapling;
        CurrentSprite = currentSprite;
        StartDay = startDay;
        State = state;
    }

    public void SetDataAtLoad(Sapling sapling, int currentSprite, int startDay, ushort state, int day, bool destroyed)
    {
        Sapling = sapling;
        CurrentSprite = currentSprite;
        StartDay = startDay;

        GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().SaplingAddList(this);

        if(state > 0)
        {
            state--;
        }

        State = state;

        if (CurrentSprite < sapling.Levels.Count)
        {
            if(spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (currentSprite == -1)
            {
                spriteRenderer.sprite = Sapling.SaplingSprite;
            }
            else
            {
                spriteRenderer.sprite = sapling.Levels[CurrentSprite];
            }

            state = 0;
        }
        else
        {
            switch (State)
            {
                case 0:
                    {
                        if (sapling.AlmostMature != null)
                        {
                            State = 1;

                            GameObject tree = Instantiate(sapling.AlmostMature, transform.position, transform.rotation);

                            tree.AddComponent<SaplingGrowHandler>().SetData(sapling, CurrentSprite, day, State);

                            tree.transform.SetParent(transform.parent);

                            GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().RemoveSaplingList(this);

                            if (destroyed == true)
                            {
                                DamageTree damageTree = tree.GetComponent<DamageTree>();

                                if (damageTree != null && destroyed == true)
                                {
                                    damageTree.Destroyed = destroyed;

                                    damageTree.ChangeCrowDesroy();
                                }
                            }

                            DestroyObject();

                        }
                        else if (sapling.Mature != null)
                        {
                            SaplingFullGrowAtLoad(destroyed);
                        }

                        break;
                    }

                case 1:
                    {
                        if (sapling.Mature != null)
                        {
                            SaplingFullGrowAtLoad(destroyed);
                        }

                        break;
                    }
            }
        }
    }

    private void SaplingFullGrowAtLoad(bool destroyed)
    {
        State = 2;

        GameObject tree = Instantiate(sapling.Mature, transform.position, transform.rotation);

        if (destroyed == true)
        {
            DamageTree newTree = tree.GetComponent<DamageTree>();

            newTree.Destroyed = true;

            newTree.ChangeCrowDesroy();
        }

        tree.GetComponent<PositionInGrid>().LocationGrid = GetComponentInParent<LocationGridSave>();

        tree.transform.SetParent(newLocationAfterFullGrow);

        tree.tag = "Tree";

        DestroyObject();
    }

    public void DayChange(int day)
    {
        if(day >= StartDay + sapling.DayToGrow)
        {
            CurrentSprite++;

            if(CurrentSprite < sapling.Levels.Count)
            {
                spriteRenderer.sprite = sapling.Levels[CurrentSprite];

                StartDay = day;

                state = 0;
            }
            else
            {
                switch(State)
                {
                    case 0:
                        {
                            if(sapling.AlmostMature != null)
                            {
                                State = 1;

                                GameObject tree = Instantiate(sapling.AlmostMature, transform.position, transform.rotation);

                                tree.AddComponent<SaplingGrowHandler>().SetData(sapling, CurrentSprite, day, State);

                                tree.transform.SetParent(transform.parent);

                                GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().RemoveSaplingList(this);

                                DestroyObject();

                            }
                            else if(sapling.Mature != null)
                            {
                                SaplingFullGrow();
                            }

                            break;
                        }

                    case 1:
                        {
                            if (sapling.Mature != null)
                            {
                                SaplingFullGrow();
                            }

                            break;
                        }
                }
            }
        }
    }

    private void SaplingFullGrow()
    {
        State = 2;

        GameObject tree = Instantiate(sapling.Mature, transform.position, transform.rotation);

        DamageTree damageTree = GetComponent<DamageTree>();

        if(damageTree != null)
        {
            if(damageTree.Destroyed == true)
            {
                DamageTree newTree = tree.GetComponent<DamageTree>();

                newTree.Destroyed = true;

                newTree.ChangeCrowDesroy();
            }
        }

        tree.GetComponent<PositionInGrid>().LocationGrid = GetComponentInParent<LocationGridSave>();

        tree.transform.SetParent(newLocationAfterFullGrow);

        tree.tag = "Tree";

        DestroyObject();
    }

    public void DestroyObject()
    {
        GameObject.Find("Global/DayTimer").GetComponent<CropGrowHandler>().RemoveSaplingList(this);

        Destroy(gameObject);
    }
}
