using UnityEngine;

public static class DefaulData
{
    //Others
    public static int stoneHealth = 10;
    public static float maxLightSourceDistance = 10f;
    public static float dialogueSpeed = .07f;

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

    // Items
    public static Item log      = new Item("Log","Lemne domnle!" , 0, 25, ItemSprites.Instance.log);
    public static Item stick    = new Item("Stick", "Good for crafting and fire", 0, 50, ItemSprites.Instance.stick);
    public static Item stone    = new Item("Stone", "Good for crafting and firepit", 0, 25, ItemSprites.Instance.stone);

    // Usable item
    public static Item stoneAxe = new Axe("Stone Pickaxe", "Your first axe!", 0, 1, ItemSprites.Instance.stonePickaxe);
    public static Item pickaxe  = new Pickaxe("Stone Pickaxe", "Your first axe!", 0, 1, ItemSprites.Instance.pickaxe);

    public static Item GetItemWithAmount(Item item, int amount)
    {
        Item auxItem = item.Copy();

        auxItem.ChangeAmount(amount);

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
}
