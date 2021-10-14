using UnityEditor;
using UnityEngine;

public static class DefaulData
{
    //Others
    public static int stoneHealth = 10;
    public static float maxLightSourceDistance = 10f;
    public static float dialogueSpeed = .07f;
    public static float degradation = .4f;

    //Quest track
    public static float maxQuestDistante = 1.5f;
    public static float borderSizeQuestTrack = 3f;

    // Stones data
    public static float castPosition = .1f;

    // Trees data
    public static int treeHealth = 10;
    public static float stickGravity = .5f;
    public static int stickSpawnRate = 25;

    // Player stats
    public static float maxPlayerHealth = 100f;
    public static float maxPlayerStamina = 100f;
    public static float playerWalkSpeed = 3f;
    public static float playerRunSpeed = 4f;
    public static int   maximInventorySlots = 30;

    // Timer
    public static int dayStart = 5;
    public static int dayEnd   = 18;
    public static float maxDayIntensity = 1f;
    public static float maxNightIntensity = .2f;
    public static int dayNightCycleTime = 5;

    // Enemy stats
    // Slime:
    
    public static float distanceToFind = 5f;
    public static float maxDinstanceToCatch = 10f;
    public static int slimeAttackRate = 1;

    // Little Slime
    public static float slimeLittleAttackPower = 5f;
    public static float slimeLittleAttackDistance = .6f;

    // Big Slime
    public static float slimeBigAttackPower = 5f;
    public static float slimeBigAttackDistance = .6f;    

    // Items
    public static Item log      = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Log.asset", typeof(Item));
    public static Item stick    = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Stick.asset", typeof(Item));
    public static Item stone    = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Stone.asset", typeof(Item));

    // Usable item
    public static Item pickAxe = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/FirstPickaxe.asset", typeof(Item));
    public static Item stoneAxe= (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/StoneAxe.asset", typeof(Item));

    public static Item GetItemWithAmount(Item item, int amount)
    {
        Item auxItem = item.Copy();

        auxItem.Amount = amount;

        return auxItem;
    }

    public static void ItemSwap(Item item1, Item item2)
    {
        Item auxSwap = item1;

        item1 = item2;
        item2 = auxSwap;
    }

    public static Vector3 GetRandomMove()
    {
        return new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    public static float GetAngleFromVectorFloat(Vector3 direction)
    {
        direction = direction.normalized;

        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }

}
