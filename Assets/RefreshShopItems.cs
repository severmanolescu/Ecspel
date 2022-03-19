using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshShopItems : MonoBehaviour
{
    private List<OpenShopHandler> openShopHandlers = new List<OpenShopHandler>();

    public void AddShop(OpenShopHandler openShop)
    {
        if(openShop != null && !openShopHandlers.Contains(openShop))
        {
            openShopHandlers.Add(openShop); 
        }
    }

    public void Refresh(int days)
    {
        foreach (OpenShopHandler handler in openShopHandlers)
        {
            handler.RefreshItems(days);
        }
    }
}
