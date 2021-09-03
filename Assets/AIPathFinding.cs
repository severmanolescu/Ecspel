using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinding : MonoBehaviour
{
    private bool canMove;

    public void MoveToLocation(Vector3 position, float speed)
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        }
    }

    public void SetCanMoveToTrue()
    {
        canMove = true;
    }
    public void SetCanMoveToFalse()
    {
        canMove = false;
    }
}
