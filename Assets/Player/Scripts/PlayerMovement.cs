using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float runStaminaUse;

    [SerializeField] private bool spawnFootprints = true;

    [Header("Audio effects")]
    [SerializeField] private AudioClip itemPickup;

    private new Rigidbody2D rigidbody;

    private Animator animator;

    private Vector2 inputs;

    private PlayerInventory playerInventory;

    private PlayerItemUse playerItem;

    private PlayerStats playerStats;

    private Keyboard keyboard;

    private PlaySound playSound;

    private bool canMove = true;

    private bool tabOpen = false;

    private bool menuOpen = false;

    private bool dialogue = false;

    private float speed;

    private float slowEffect = 1f;
    private float fatiqueEffect = 1f;

    public float Speed { get { return speed; } }
    public bool CanMove { get { return canMove; } }
    public bool TabOpen { get { return tabOpen; } set { tabOpen = value; } }
    public float SlowEffect { get { return slowEffect; } set { slowEffect = value; } }
    public float FatiqueEffect { get { return fatiqueEffect; } set { fatiqueEffect = value; } }

    public bool Dialogue { get => dialogue; set => dialogue = value; }
    public bool MenuOpen { get => menuOpen; set => menuOpen = value; }
    public bool SpawnFootprints { get => spawnFootprints; set => spawnFootprints = value; }

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        animator = gameObject.GetComponent<Animator>();

        playerItem = gameObject.GetComponent<PlayerItemUse>();

        playerInventory = gameObject.GetComponentInChildren<PlayerInventory>();

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        keyboard = InputSystem.GetDevice<Keyboard>();

        playSound = GetComponentInChildren<PlaySound>();
    }

    private void GetInputs()
    {
        inputs = Vector2.zero;

        if (keyboard.aKey.isPressed)
        {
            inputs.x = -1;
        }
        if (keyboard.dKey.isPressed)
        {
            inputs.x += 1;
        }

        if (keyboard.sKey.isPressed)
        {
            inputs.y = -1;
        }
        if (keyboard.wKey.isPressed)
        {
            inputs.y += 1;
        }
    }

    private void Update()
    {
        
    }

    private void SetAnimatorVariables()
    {
        if (inputs.x > 0 && inputs.y > 0) //Up right
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", inputs.y);
        }
        else if (inputs.x < 0 && inputs.y > 0) //Up left
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", inputs.y);
        }
        else if (inputs.x > 0 && inputs.y < 0) //Down right
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", inputs.y);
        }
        else if (inputs.x < 0 && inputs.y < 0) //Down left
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", inputs.y);
        }
        else
        {
            animator.SetFloat("Horizontal", inputs.x);
            animator.SetFloat("Vertical", inputs.y);
        }

        animator.SetFloat("Speed", speed);

        if (inputs != Vector2.zero)
        {
            animator.SetFloat("HorizontalFacing", inputs.x);
            animator.SetFloat("VerticalFacing", inputs.y);

            playerItem.Inputs = inputs;
        }
    }

    private void FixedUpdate()
    {
        if (canMove == true && tabOpen == false && dialogue == false)
        {
            GetInputs();

            inputs = inputs.normalized;

            if (keyboard.leftShiftKey.isPressed)
            {
                inputs *= runSpeed;

                if (inputs != Vector2.zero)
                {
                    playerStats.DecreseStamina(runStaminaUse * Time.deltaTime * fatiqueEffect);
                }
            }
            else
            {
                inputs *= walkSpeed;
            }

            speed = inputs.SqrMagnitude();

            SetAnimatorVariables();

            rigidbody.MovePosition(rigidbody.position + inputs * Time.fixedDeltaTime / slowEffect);

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemWorld"))
        {
            ItemWorld itemWorld = collision.GetComponent<ItemWorld>();

            if (itemWorld.EnteredInPlayer == false)
            {
                itemWorld.EnteredInPlayer = true;

                if (playerInventory.AddItemWithAnimation(itemWorld.Item) == 0)
                {
                    collision.gameObject.GetComponent<ItemWorld>().DestroySelf();

                    playSound.Play(itemPickup);
                }
                else
                {
                    collision.gameObject.GetComponent<ItemWorld>().ReinitializeItem();

                    itemWorld.EnteredInPlayer = false;
                }
            }

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
        animator.SetFloat("Vertical",   0);
        animator.SetFloat("Speed",      0);
    }

    public void ChangeIdleAnimationDirection(int direction)
    {
        Vector3 directionToChange = Vector3.zero;

        switch (direction)
        {
            case 0: directionToChange = Vector3.left;   break;
            case 1: directionToChange = Vector3.right;  break;
            case 2: directionToChange = Vector3.up;     break;
            case 3: directionToChange = Vector3.down;   break;
        }

        if (directionToChange != Vector3.zero)
        {
            animator.SetFloat("HorizontalFacing",   directionToChange.x);
            animator.SetFloat("VerticalFacing",     directionToChange.y);
        }
    }

    public void ChangeIdleAnimationDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:   ChangeIdleAnimationDirection(1); break;
            case Direction.Left:    ChangeIdleAnimationDirection(0); break;
            case Direction.Up:      ChangeIdleAnimationDirection(2); break;
            case Direction.Down:    ChangeIdleAnimationDirection(3); break;
        }
    }
}
