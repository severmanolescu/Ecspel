using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllForgesStorage : MonoBehaviour
{
    [SerializeField] private GameObject placedObjects;

    private GetItemFromNO getItemFromNO;

    private GetForgeType getForgeType;

    private void Awake()
    {
        getItemFromNO = GameObject.Find("Global").GetComponent<GetItemFromNO>();

        getForgeType = GetComponent<GetForgeType>();
    }

    public List<ForgeStorage> GetAllForges()
    {
        List<ForgeStorage> forgesStorage = new List<ForgeStorage>();

        ForgeOpenHandler[] forges = placedObjects.GetComponentsInChildren<ForgeOpenHandler>();

        foreach (ForgeOpenHandler forge in forges)
        {
            ForgeStorage storage = new ForgeStorage
            {
                PositionX = forge.transform.position.x,
                PositionY = forge.transform.position.y,

                Id = forge.ForgeID
            };

            if (forge.InputItem != null)
            {
                storage.Storage.Add(new System.Tuple<int, int>(forge.InputItem.ItemNO, forge.InputItem.Amount));
            }
            else
            {
                storage.Storage.Add(new System.Tuple<int, int>(-1, -1));
            }

            if (forge.FuelItem != null)
            {
                storage.Storage.Add(new System.Tuple<int, int>(forge.FuelItem.ItemNO, forge.GetFuelItem().Amount));
            }
            else
            {
                storage.Storage.Add(new System.Tuple<int, int>(-1, -1));
            }

            if (forge.OutputItem != null)
            {
                storage.Storage.Add(new System.Tuple<int, int>(forge.OutputItem.ItemNO, forge.OutputItem.Amount));
            }
            else
            {
                storage.Storage.Add(new System.Tuple<int, int>(-1, -1));
            }

            forgesStorage.Add(storage);
        }

        return forgesStorage;
    }

    public void SetAllForges(List<ForgeStorage> forgesStorage)
    {
        ForgeOpenHandler[] forges = placedObjects.GetComponentsInChildren<ForgeOpenHandler>();

        foreach (ForgeOpenHandler forge in forges)
        {
            Destroy(forge.gameObject);
        }

        foreach(ForgeStorage forge in forgesStorage)
        {
            if(forge != null)
            {
                GameObject forgeInstantiate = Instantiate(getForgeType.GetForgeObject(forge.Id));

                forgeInstantiate.transform.position = new Vector3(forge.PositionX, forge.PositionY);

                forgeInstantiate.transform.parent = placedObjects.transform;

                ForgeOpenHandler forgeOpen = forgeInstantiate.GetComponent<ForgeOpenHandler>();

                if(forgeOpen != null)
                {
                    Item newItem;

                    newItem = getItemFromNO.ItemFromNo(forge.Storage[0].Item1);

                    if(newItem != null)
                    {
                        forgeOpen.InputItem = newItem.Copy();
                        forgeOpen.InputItem.Amount = forge.Storage[0].Item2;
                    }

                    newItem = getItemFromNO.ItemFromNo(forge.Storage[1].Item1);

                    if (newItem != null)
                    {
                        forgeOpen.FuelItem = newItem.Copy();
                        forgeOpen.FuelItem.Amount = forge.Storage[1].Item2;
                    }

                    newItem = getItemFromNO.ItemFromNo(forge.Storage[2].Item1);

                    if (newItem != null)
                    {
                        forgeOpen.OutputItem = newItem.Copy();
                        forgeOpen.OutputItem.Amount = forge.Storage[2].Item2;
                    }
                }
                else
                {
                    Destroy(forgeInstantiate);
                }
            }
        }
    }
}
