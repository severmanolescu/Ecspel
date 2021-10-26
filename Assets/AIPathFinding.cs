using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinding : MonoBehaviour
{
    private float initialScaleX;

    private Vector3 changeScale;

    private bool canMove = true;

    private void Awake()
    {
        initialScaleX = transform.localScale.x;

        changeScale = transform.localScale;
    }

    public void MoveToLocation(Vector3 position, float speed)
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);

            if (position.x > transform.position.x)
            {
                changeScale.x = -initialScaleX;

                transform.localScale = changeScale;
            }
            else
            {
                changeScale.x = initialScaleX;

                transform.localScale = changeScale;
            }
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
