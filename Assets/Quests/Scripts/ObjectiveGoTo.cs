using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New GoTo Objective", order = 1)]
[Serializable]
public class ObjectiveGoTo : Objective
{
    [SerializeField] private Vector2 goToPoint;

    [SerializeField] private float circleRadius = 3f;

    public ObjectiveGoTo(string objectiveName, bool completed, Vector2 goToPoint, float circleRadius) :
        base(objectiveName, completed)
    {
        this.goToPoint = goToPoint;
        this.circleRadius = circleRadius;
    }

    public Vector2 GoToPoint { get => goToPoint; set => goToPoint = value; }
    public float CircleRadius { get => circleRadius; set => circleRadius = value; }
}
