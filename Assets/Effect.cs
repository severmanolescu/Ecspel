using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/New Effect", order = 1)]
[Serializable]
public class Effect : ScriptableObject
{
    [Header("Duration in seconds:")]
    [SerializeField] private int duration;

    [Header("The power of the effectt: ")]
    [SerializeField] private float power;
}
