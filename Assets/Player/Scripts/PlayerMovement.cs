using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private new Rigidbody2D rigidbody;

    private Animator animator;

    private Vector2 inputs;

    private  PlayerInventory playerInventory;

    private PlayerItemUse playerItem;

    private PlayerStats playerStats;

    private bool canMove = true;

    private bool tabOpen = false;

    private float speed;

    public float Speed { get { return speed; } }
    public bool CanMove { get { return canMove; } }
    public bool TabOpen { get { return tabOpen; } set { tabOpen = value; } }

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        animator = gameObject.GetComponent<Animator>();

        playerItem = gameObject.GetComponent<PlayerItemUse>();

        playerInventory = gameObject.GetComponentInChildren<PlayerInventory>();

        playerStats = GameObject.Find("Global/Player/Canvas/Stats").GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (canMove == true && tabOpen == false)
        {
            inputs.x = Input.GetAxisRaw("Horizontal");
            inputs.y = Input.GetAxisRaw("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                inputs *= DefaulData.playerRunSpeed;

                if (inputs != Vector2.zero)
                {
                    playerStats.Stamina -= 0.5f * Time.deltaTime;
                }
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

                playerItem.Inputs = inputs;
            }
        }
        else
        {
            speed = 0;
            inputs = Vector2.zero;

            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 0f);
            animator.SetFloat("Speed", 0f);
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
            if(playerInventory.AddItem(collision.gameObject.GetComponent<ItemWorld>().Item) == true)
            {
                collision.gameObject.GetComponent<ItemWorld>().DestroySelf();
            }
            else
            {
                collision.gameObject.GetComponent<ItemWorld>().ReinitializeItem();
            }
        }
        else if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(5);
        }
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
