using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Craft", menuName = "Craft/New Craft", order = 1)]
[Serializable]
public class Craft : ScriptableObject
{
    [SerializeField] private List<ItemWithAmount> needItem = new List<ItemWithAmount>();

    [SerializeField] private ItemWithAmount receiveItem;

    [SerializeField] private int stamina;

    public Craft(List<ItemWithAmount> needItem, ItemWithAmount receiveItem, int stamina)
    {
        this.NeedItem = needItem;
        this.ReceiveItem = receiveItem;
        this.Stamina = stamina;
    }

    public List<ItemWithAmount> NeedItem { get => needItem; set => needItem = value; }
    public ItemWithAmount ReceiveItem { get => receiveItem; set => receiveItem = value; }
    public int Stamina { get => stamina; set => stamina = value; }
}
