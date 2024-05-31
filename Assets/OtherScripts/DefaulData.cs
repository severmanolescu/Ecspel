using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class DefaulData
{
    public static Vector3 nullVector = new Vector3(-100, -100, -100);

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
    public static float maxPlayerHealth = 500f;
    public static float maxPlayerStamina = 100f;
    public static float playerWalkSpeed = 7.5f;
    public static float playerRunSpeed = 10f;
    public static int maximInventorySlots = 30;

    // Timer
    public static int dayStart = 5;
    public static int dayEnd = 18;
    public static int dayNightCycleTime = 5;

    // Enemy stats
    // Slime
    public static float distanceToFind = 5f;
    public static float maxDinstanceToCatch = 10f;
    public static int slimeAttackRate = 1;

    // Little Slime
    public static float slimeLittleAttackPower = 2f;
    public static float slimeLittleAttackDistance = 2f;

    // Big Slime
    public static float slimeBigAttackPower = 5f;
    public static float slimeBigAttackDistance = 2f;

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

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ()
    {
        return GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), Camera.main);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    {
        return GetMouseWorldPositionWithZ(Mouse.current.position.ReadValue(), worldCamera);
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    public static string getBetween(string strSource, string strStart, string strEnd)
    {
        if (strSource.Contains(strStart) && strSource.Contains(strEnd))
        {
            int Start;
            int End;

            Start = strSource.IndexOf(strStart, 0) + strStart.Length;

            End = strSource.IndexOf(strEnd, Start);

            return strSource.Substring(Start, End - Start);
        }

        return "";
    }
    public static string getBetween(string strSource, string strStart)
    {
        if (strSource.Contains(strStart))
        {
            return strSource.Substring(strSource.IndexOf(strStart), strSource.Length - 2);
        }

        return "";
    }

    public static float map(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static Vector3 GetRandomPositionCollider(BoxCollider2D boxCollider, Transform currentObject)
    {
        return new Vector3(UnityEngine.Random.Range(boxCollider.transform.position.x - boxCollider.size.x / 2, boxCollider.transform.position.x + boxCollider.size.x / 2),
                           UnityEngine.Random.Range(boxCollider.transform.position.y - boxCollider.size.y / 2, boxCollider.transform.position.y + boxCollider.size.y / 2),
                           currentObject.position.z);
    }
}

public enum Direction
{
    Right,
    Left,
    Up,
    Down
}
public enum Animals
{
    Bird
}

[Serializable]
public class MyTuple
{
    [SerializeField] private ushort item1;
    [SerializeField] private ushort item2;

    public MyTuple(ushort item1, ushort item2)
    {
        this.item1 = item1;
        this.item2 = item2;
    }

    public ushort Item1 { get => item1; }
    public ushort Item2 { get => item2; }
}

[Serializable]
public class NpcDialogue
{
    [SerializeField] private NpcDialogueDetails details;

    [SerializeField] private List<DialogueNPCs> dialogues;

    public NpcDialogueDetails Details { get => details; }
    public List<DialogueNPCs> Dialogues { get => dialogues; }
}

[Serializable]
public class DialogueList
{
    [SerializeField] private string details;

    [SerializeField] private List<DialogueResponse> list;

    public string Details { get => details; set => details = value; }
    public List<DialogueResponse> List { get => list; set => list = value; }
}

public enum NpcDialogueDetails
{
    Digging,
    Finish
}