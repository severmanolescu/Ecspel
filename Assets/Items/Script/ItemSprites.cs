using System.Collections.Generic;
using UnityEngine;

public class ItemSprites : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();

    public Transform ItemWorld;

    public List<Sprite> Sprites { get => sprites; set => sprites = value; }

    public Sprite GetItemSprite(int itemNO)
    {
        if(itemNO > 0 && itemNO <= sprites.Count)
        {
            return sprites[itemNO - 1];
        }

        return null;
    }

    public static ItemSprites Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
