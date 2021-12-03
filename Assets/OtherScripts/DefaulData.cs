using System;
using UnityEditor;
using UnityEngine;

public static class DefaulData
{
    //Others
    public static int stoneHealth = 10;
    public static float maxLightSourceDistance = 10f;
    public static float dialogueSpeed = .07f;
    public static float degradation = .4f;
    public const int sortingOrderDefault = 5000;

    //Quest track
    public static float maxQuestDistante = 1.5f;
    public static float borderSizeQuestTrack = 3f;

    // Stones data
    public static float castPosition = 1f;

    // Trees data
    public static int treeHealth = 10;
    public static float stickGravity = .5f;
    public static int stickSpawnRate = 25;

    // Player stats
    public static float maxPlayerHealth = 100f;
    public static float maxPlayerStamina = 100f;
    public static float playerWalkSpeed = 7.5f;
    public static float playerRunSpeed = 10f;
    public static int   maximInventorySlots = 30;

    // Timer
    public static int dayStart = 5;
    public static int dayEnd   = 18;
    public static int dayNightCycleTime = 5;

    // Enemy stats
    // Slime
    public static float distanceToFind = 5f;
    public static float maxDinstanceToCatch = 10f;
    public static int slimeAttackRate = 1;

    // Little Slime
    public static float slimeLittleAttackPower = 2.5f;
    public static float slimeLittleAttackDistance = 1f;

    // Big Slime
    public static float slimeBigAttackPower = 5f;
    public static float slimeBigAttackDistance = 1.5f;    

    // Items
    public static Item log      = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Log.asset", typeof(Item));
    public static Item stick    = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Stick.asset", typeof(Item));
    public static Item stone    = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Stone.asset", typeof(Item));

    // Usable item
    public static Item pickAxe = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/FirstPickaxe.asset", typeof(Item));
    public static Item stoneAxe= (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/StoneAxe.asset", typeof(Item));
    public static Item hoe= (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Hoe.asset", typeof(Item));
    public static Item sword= (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Sword.asset", typeof(Item));

    //Enemy drops
    public static Item slime = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/EnemyDrop/Slime.asset", typeof(Item));

    //Berries
    public static Item yellowRaspberry = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Berries/YellowRaspberry.asset", typeof(Item));

    //Crops
    public static Item PotatoSeeds = (Item)AssetDatabase.LoadAssetAtPath("Assets/Items/Crops/PotatoSeeds.asset", typeof(Item));


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

    public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
    }

    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Color GetColorFromString(string color)
    {
        float red = Hex_to_Dec01(color.Substring(0, 2));
        float green = Hex_to_Dec01(color.Substring(2, 2));
        float blue = Hex_to_Dec01(color.Substring(4, 2));
        float alpha = 1f;
        if (color.Length >= 8)
        {
            // Color string contains alpha
            alpha = Hex_to_Dec01(color.Substring(6, 2));
        }
        return new Color(red, green, blue, alpha);
    }

    public static float Hex_to_Dec01(string hex)
    {
        return Hex_to_Dec(hex) / 255f;
    }

    public static int Hex_to_Dec(string hex)
    {
        return Convert.ToInt32(hex, 16);
    }
}
