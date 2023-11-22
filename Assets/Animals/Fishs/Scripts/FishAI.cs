using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    [SerializeField] private float swimSpeed = 1f;

    private BoxCollider2D boxCollider;

    private Animator animator;

    private bool moving = false;

    private Vector3 moveToLocation;

    private void Start()
    {
        boxCollider = GetComponentInParent<BoxCollider2D>();

        animator = GetComponent<Animator>();
    }

    public void ChangeIdleState()
    {
        if(!moving)
        {
            int state = Random.Range(0, 5);

            SetAnimation(state);
        }
    }

    private void SetSwimmingValueAnimator(bool value)
    {
        animator.SetBool("Swim", value);
    }

    private void SetAnimation(int state)
    {
        switch (state)
        {
            case 1: StartSwimming(); break;
            default: SetSwimmingValueAnimator(false); break;
        }
    }

    private void GetNewLocation()
    {
        moveToLocation = new Vector3(Random.Range(boxCollider.transform.position.x - boxCollider.size.x / 2, boxCollider.transform.position.x + boxCollider.size.x / 2),
                                     Random.Range(boxCollider.transform.position.y - boxCollider.size.y / 2, boxCollider.transform.position.y + boxCollider.size.y / 2),
                                     transform.position.z);
    }

    private void ChangeDirection()
    {
        if (transform.position.x > moveToLocation.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void StartSwimming()
    {
        GetNewLocation();

        SetSwimmingValueAnimator(true);

        ChangeDirection();

        moving = true;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            Vector3 moveDir = (moveToLocation - transform.position).normalized;

            transform.position = transform.position + moveDir * swimSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, moveToLocation) <= 0.05)
            {
                SetSwimmingValueAnimator(false);

                moving = false;
            }
        }
    }
}
