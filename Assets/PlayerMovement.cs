using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    private Animator animator;

    private Vector2 inputs;

    private  PlayerInventory playerInventory;

    private PlayerItemUse playerItem;

    private bool canMove = true;

    private float speed;

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        animator = gameObject.GetComponent<Animator>();

        playerItem = gameObject.GetComponent<PlayerItemUse>();

        playerInventory = gameObject.GetComponentInChildren<PlayerInventory>();
    }

    private void Update()
    {
        if (canMove)
        {
            inputs.x = Input.GetAxisRaw("Horizontal");
            inputs.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputs *= DefaulData.playerRunSpeed;
            }
            else
            {
                inputs *= DefaulData.playerWalkSpeed;
            }

            speed = inputs.SqrMagnitude();

            animator.SetFloat("Horizontal", inputs.x);
            animator.SetFloat("Vertical", inputs.y);
            animator.SetFloat("Speed", speed);

            if (inputs != Vector2.zero)
            {
                animator.SetFloat("HorizontalFacing", inputs.x);
                animator.SetFloat("VerticalFacing", inputs.y);

                playerItem.SetInputs(inputs);
            }
        }
    }

    private void FixedUpdate()
    {
        if(canMove)
            rigidbody.MovePosition(rigidbody.position + inputs * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ItemWorld"))
        {
            if(playerInventory.AddItem(collision.gameObject.GetComponent<ItemWorld>().GetItem()) == true)
            {
                collision.gameObject.GetComponent<ItemWorld>().DestroySelf();
            }
            else
            {
                collision.gameObject.GetComponent<ItemWorld>().ReinitializeItem();
            }
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetPlayerMovementTrue()
    {
        canMove = true;
    }

    public void SetPlayerMovementFalse()
    {
        canMove = false;

        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Speed", 0);
    }
}
