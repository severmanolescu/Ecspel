public static class DefaulData
{
    //Others
    public static int maximInventorySlots = 30;
    public static int treeHealth = 10;

    // Player stats
    public static float maxPlayerHealth = 100f;
    public static float maxPlayerStamina = 100f;
    public static float playerWalkSpeed = 3f;
    public static float playerRunSpeed = 4f;

    // Items
    public static Item log      = new Item("Log","Lemne domnle!" , 0, 25, ItemSprites.Instance.log);
    public static Item stick    = new Item("Stick", "Good for crafting and fire", 0, 50, ItemSprites.Instance.stick);
    public static Item stone    = new Item("Stone", "Good for crafting and firepit", 0, 25, ItemSprites.Instance.stone);

    // Equip item
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
}
