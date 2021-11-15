using UnityEngine;

public class ItemSprites : MonoBehaviour
{
    public static ItemSprites Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    [Header("Items")]
    public Sprite log;
    public Sprite stick;
    public Sprite stone;

    [Header("Equip")]
    public Sprite stonePickaxe;
    public Sprite pickaxe;

    [Header("Other")]
    public Transform ItemWorld;
}
