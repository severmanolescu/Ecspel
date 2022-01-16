using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SaveGame
{
    private int days;

    private int playedMinutes;

    private List<Item> playerInventory;

    private List<Item> chestStorage;

    public SaveGame(int days, int playedMinutes)
    {
        this.days = days;

        this.playedMinutes = playedMinutes;
    }

    public SaveGame()
    {

    }

    public int Days { get => days; set => days = value; }
    public int PlayedMinutes { get => playedMinutes; set => playedMinutes = value; }
    public List<Item> PlayerInventory { get => playerInventory; set => playerInventory = value; }
    public List<Item> ChestStorage { get => chestStorage; set => chestStorage = value; }
}
