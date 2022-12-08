using System.Collections.Generic;
using UnityEngine;

public class GetAllSapling : MonoBehaviour
{
    [SerializeField] private GameObject placedObjects;

    [SerializeField] private LocationGridSave locationGrid;

    private DayTimerHandler dayTimerHandler;

    private BuildSystemHandler buildSystem;

    private GetItemFromNO getItem;

    private GetItemWorld getItemWorld;

    private void Awake()
    {
        getItemWorld = GameObject.Find("Global").GetComponent<GetItemWorld>();

        getItem = getItemWorld.GetComponent<GetItemFromNO>();

        buildSystem = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    public List<SaplingSave> GetAllObjects()
    {
        List<SaplingSave> saplings = new List<SaplingSave>();

        SaplingGrowHandler[] saplingGrowHandlers = placedObjects.GetComponentsInChildren<SaplingGrowHandler>();

        foreach (SaplingGrowHandler sapling in saplingGrowHandlers)
        {
            if (sapling.CompareTag("TreeSapling"))
            {
                SaplingSave saplingSave = new SaplingSave();

                saplingSave.Sapling = sapling.Sapling.ItemNO;

                saplingSave.PositionX = sapling.transform.position.x;
                saplingSave.PositionY = sapling.transform.position.y;

                saplingSave.CurrentSprite = sapling.CurrentSprite;

                saplingSave.StartDay = sapling.StartDay;

                saplingSave.State = sapling.State;

                DamageTree damageTree = sapling.GetComponent<DamageTree>();

                if (damageTree != null)
                {
                    saplingSave.Destroyed = damageTree.Destroyed;
                }

                saplings.Add(saplingSave);
            }
        }

        return saplings;
    }

    public void SetSaplingsToWorld(List<SaplingSave> saplings)
    {
        SaplingGrowHandler[] saplingGrowHandlers = placedObjects.GetComponentsInChildren<SaplingGrowHandler>();

        foreach (SaplingGrowHandler saplingGrow in saplingGrowHandlers)
        {
            saplingGrow.DestroyObject();
        }

        foreach (SaplingSave sapling in saplings)
        {
            GameObject instantiateSapling = buildSystem.PlaceObject(new Vector3(sapling.PositionX, sapling.PositionY),
                                                                    getItem.ItemFromNo(sapling.Sapling));

            PositionInGrid positionInGrid = instantiateSapling.GetComponent<PositionInGrid>();

            if (positionInGrid != null)
            {
                positionInGrid.LocationGrid = locationGrid;
            }

            SaplingGrowHandler growHandler = instantiateSapling.GetComponent<SaplingGrowHandler>();

            instantiateSapling.transform.SetParent(placedObjects.transform);

            instantiateSapling.transform.position = new Vector3(sapling.PositionX, sapling.PositionY);

            if (growHandler != null)
            {
                growHandler.SetDataAtLoad((Sapling)getItem.ItemFromNo(sapling.Sapling),
                                    sapling.CurrentSprite,
                                    sapling.StartDay,
                                    sapling.State,
                                    dayTimerHandler.Days,
                                    sapling.Destroyed);
            }
            else
            {
                Destroy(instantiateSapling);
            }
        }
    }
}
