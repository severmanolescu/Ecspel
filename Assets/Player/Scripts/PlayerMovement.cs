using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    [Header("Audio effects")]
    [SerializeField] private AudioClip itemPickup;

    private AudioSource audioSource;

    private new Rigidbody2D rigidbody;

    private Animator animator;

    private Vector2 inputs;

    private  PlayerInventory playerInventory;

    private PlayerItemUse playerItem;

    private PlayerStats playerStats;

    private GameplayInputs gameplay;

    private Keyboard keyboard;

    private Joystick joystick;

    private bool canMove = true;

    private bool tabOpen = false;

    private float speed;

    private float slowEffect = 1f;
    private float fatiqueEffect = 1f;

    public float Speed { get { return speed; } }
    public bool CanMove { get { return canMove; } }
    public bool TabOpen { get { return tabOpen; } set { tabOpen = value; } }
    public float SlowEffect { get { return slowEffect; } set { slowEffect = value; } }
    public float FatiqueEffect { get { return fatiqueEffect; } set { fatiqueEffect = value; } }

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        animator = gameObject.GetComponent<Animator>();

        playerItem = gameObject.GetComponent<PlayerItemUse>();

        playerInventory = gameObject.GetComponentInChildren<PlayerInventory>();

        playerStats = GameObject.Find("Global/Player/Canvas/Stats").GetComponent<PlayerStats>();

        audioSource = gameObject.GetComponent<AudioSource>();

        gameplay = new GameplayInputs();

        keyboard = InputSystem.GetDevice<Keyboard>();

        joystick = InputSystem.GetDevice<Joystick>();
    }

    private void GetInputs()
    {
        inputs = Vector2.zero;

        if (keyboard.aKey.isPressed ||
            keyboard.leftArrowKey.isPressed ||
            (joystick != null && joystick.stick.right.ReadValue() < 1 && joystick.stick.right.ReadValue() > 0))
        {
            inputs.x = -1;
        }
        if (keyboard.dKey.isPressed ||
            keyboard.rightArrowKey.isPressed ||
           (joystick != null &&  joystick.stick.left.ReadValue() < 1 && joystick.stick.left.ReadValue() > 0))
        {
            if(inputs.x == 0)
            {
                inputs.x = 1;
            }
            else
            {
                inputs.x = 0;
            }
        }

        if (keyboard.sKey.isPressed ||
            keyboard.downArrowKey.isPressed ||
            (joystick != null &&  joystick.stick.up.ReadValue() < 1 && joystick.stick.up.ReadValue() > 0))
        {
            inputs.y = -1;
        }
        if (keyboard.wKey.isPressed ||
            keyboard.upArrowKey.isPressed ||
            (joystick != null && joystick.stick.down.ReadValue() > 0))
        {
            if (inputs.y == 0)
            {
                inputs.y = 1;
            }
            else
            {
                inputs.y = 0;
            }
        }
    }

    private void Update()
    {
        if (canMove == true && tabOpen == false)
        {
            GetInputs();

            inputs = inputs.normalized;        

            if (keyboard.leftShiftKey.isPressed || (joystick != null && joystick.allControls[10].IsPressed() == false))
            {
                inputs *= runSpeed;

                if (inputs != Vector2.zero)
                {
                    playerStats.Stamina -= 0.5f * Time.deltaTime * fatiqueEffect;
                }
            }
            else
            {
                inputs *= walkSpeed;
            }

            speed = inputs.SqrMagnitude();

            SetAnimatorVariables();

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
    }

    private void FixedUpdate()
    {
        if(canMove)
            rigidbody.MovePosition(rigidbody.position + inputs * Time.fixedDeltaTime / slowEffect);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("ItemWorld"))
        {
            if(playerInventory.AddItem(collision.gameObject.GetComponent<ItemWorld>().Item) == true)
            {
                collision.gameObject.GetComponent<ItemWorld>().DestroySelf();

                audioSource.clip = itemPickup;
                audioSource.Play();
            }
            else
            {
                collision.gameObject.GetComponent<ItemWorld>().ReinitializeItem();
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
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Speed", 0);
    }

    public void ChangeIdleAnimationDirection(int direction)
    {
        Vector3 directionToChange = Vector3.zero;

        switch(direction)
        {
            case 0: directionToChange = Vector3.left; break;
            case 1: directionToChange = Vector3.right; break;
            case 2: directionToChange = Vector3.up; break;
            case 3: directionToChange = Vector3.down; break;
        }

        if(directionToChange != Vector3.zero)
        {
            animator.SetFloat("HorizontalFacing", directionToChange.x);
            animator.SetFloat("VerticalFacing", directionToChange.y);
        }
    }
}
