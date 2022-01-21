using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemFromNO : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    public int GetItemNO(Item item)
    {
        switch(item.Name)
        {
            case "Log": return 1;  
            case "Stone": return 2;  
            case "StoneAxe": return 3;  
            case "Sword": return 4;  
            case "Pickaxe": return 5;  
            case "Hoe": return 6;  
            case "Iron": return 7;  
            case "Beans": return 8;  
            case "Beetroot": return 9;  
            case "Carrot": return 10;  
            case "Corn": return 11;  
            case "Cucumbers": return 12;  
            case "Garlic": return 13;  
            case "Grapes": return 14;  
            case "Green Bell Pepper": return 15;  
            case "Green Pepper": return 16;  
            case "Lettuce": return 17;  
            case "Melon": return 18;  
            case "Onion": return 19;  
            case "Potato": return 20;  
            case "Red Bell Pepper": return 21;  
            case "Strawberry": return 22;  
            case "Tomato": return 23;  
            case "Wheat": return 24;  
            case "Yellow Bell Pepper": return 25;  
            case "Beans Seeds": return 26;  
            case "Beetroot Seeds": return 27;  
            case "Cabbage Seeds": return 28;  
            case "Carrot seeds": return 29;  
            case "Corn Seeds": return 30;  
            case "Cucumbers Seeds": return 31;  
            case "Garlic Seeds": return 32;  
            case "Grapes Seeds": return 33;  
            case "Green Bell Pepper seeds": return 34;  
            case "Green Pepper Seeds": return 35;  
            case "Melon Seeds": return 36;  
            case "Onion seeds": return 37;  
            case "Potatoe seeds": return 38;  
            case "Red Bell Pepper Seed": return 39;  
            case "Strawberry Seeds": return 40;  
            case "Tomato Seeds": return 41;  
            case "WheatSeeds": return 42;  
            case "Yellow Bell Pepper Seed": return 43;  
            case "Slime": return 44;  
            case "Small chest": return 45;  
            case "Smith": return 46;  
            case "Fir sapling": return 47;  
            case "Pine sapling": return 48;  
            case "Tree sapling": return 49;  
            case "Tree sapling2": return 50;  
            case "Tree sapling3": return 51;

            default: return -1;
        }
    }

    public Item ItemFromNo(int itemNo)
    {
        if(itemNo > 0 && itemNo <= items.Count)
        {
            return items[itemNo - 1];
        }

        return null;
    }
}
