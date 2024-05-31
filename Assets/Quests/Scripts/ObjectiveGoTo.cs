using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Objective Go To", order = 1)]
[Serializable]
public class ObjectiveGoTo : Objective
{
    [SerializeField] private Vector2 goToPoint;

    [SerializeField] private float circleRadius = 3f;

    public Vector2 GoToPoint { get => goToPoint; set => goToPoint = value; }
    public float CircleRadius { get => circleRadius; set => circleRadius = value; }
}
