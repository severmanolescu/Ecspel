using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Craft", menuName = "Craft/New Craft", order = 1)]
[Serializable]
public class Craft : ScriptableObject
{
    [SerializeField] private List<ItemWithAmount> needItem = new List<ItemWithAmount>();

    [SerializeField] private ItemWithAmount receiveItem;

    [SerializeField] private int stamina;

    [Header("0 - Weapon \n1 - Tools\n2 - Resources")]
    [SerializeField] private int filterType;

    public Craft(List<ItemWithAmount> needItem, ItemWithAmount receiveItem, int stamina, int filterType)
    {
        NeedItem = needItem;
        ReceiveItem = receiveItem;
        Stamina = stamina;
        FilterType = filterType;
    }

    public List<ItemWithAmount> NeedItem { get => needItem; set => needItem = value; }
    public ItemWithAmount ReceiveItem { get => receiveItem; set => receiveItem = value; }
    public int Stamina { get => stamina; set => stamina = value; }
    public int FilterType { get => filterType; set => filterType = value; }
}
