using UnityEngine;

public class DeleteCaveChestItems : MonoBehaviour
{
    public void DeleteAllStorage()
    {
        ChestOpenHandler[] chests = gameObject.GetComponentsInChildren<ChestOpenHandler>();

        Debug.Log(chests.Length);

        foreach (ChestOpenHandler chest in chests)
        {
            chest.DeleteAllItems();
        }
    }
}
