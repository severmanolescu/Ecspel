using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/New Effect", order = 1)]
[Serializable]
public class Effect : ScriptableObject
{
    [Header("Duration in seconds:")]
    [SerializeField] private int duration;

    [Header("The power of the effectt: ")]
    [SerializeField] private float power;

    [Header("Effect sprite:")]
    [SerializeField] private Sprite effectSprite;

    [Header("Effect type:")]
    [SerializeField] private EffectType effect;

    public Effect(int duration, float power, Sprite effectSprite, EffectType effectType)
    {
        this.Duration = duration;
        this.Power = power;
        this.EffectSprite = effectSprite;
        this.EffectType = effectType;
    }

    public int Duration { get => duration; set => duration = value; }
    public float Power { get => power; set => power = value; }
    public Sprite EffectSprite { get => effectSprite; set => effectSprite = value; }
    public EffectType EffectType { get => effect; set => effect = value; }
}

public enum EffectType
{
    Otrava,
    AtacInstant,
    AtacSlab,
    Incetinire,
    Oboseala
}
