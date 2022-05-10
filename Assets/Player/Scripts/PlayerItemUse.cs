using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger selectedSlot;

    [Header("Audio effects")]
    [SerializeField] private AudioClip attackClip;

    [SerializeField] private Item emptyBucket;
    [SerializeField] private Item fullBucket;

    private PlayerStats playerStats;

    private AudioSource audioSource;

    private Item item;

    private PlayerMovement playerMovement;

    private Animator animator;

    private SkillsHandler skillsHandler;

    private AxeHandler axeHandler;
    private PickaxeHandler pickaxeHandler;
    private HoeSystemHandler hoeSystemHandler;
    private WateringCanHandler wateringCanHandler;
    private HarvestCropHandler harvestCropHandler;
    private BuildSystemHandler buildSystemHandler;

    private LetterHandler letterHandler;

    private bool water = false;

    private Mouse mouse;

    private Vector2 inputs = Vector2.zero;

    private Vector2 detectionZone = new Vector2(1.5f, 1.5f);

    private float attackDecrease = 1f;

    private bool leftMousePress = true;
    private bool rightMousePress = true;

    public Vector2 Inputs { set { inputs = value; } }
    public float AttackDecrease { set { attackDecrease = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();

        mouse = InputSystem.GetDevice<Mouse>();

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        skillsHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();

        axeHandler = GameObject.Find("Global/BuildSystem").GetComponent<AxeHandler>();
        pickaxeHandler = axeHandler.GetComponent<PickaxeHandler>();
        hoeSystemHandler = axeHandler.GetComponent<HoeSystemHandler>();
        wateringCanHandler = axeHandler.GetComponent<WateringCanHandler>();
        harvestCropHandler = axeHandler.GetComponent<HarvestCropHandler>();
        buildSystemHandler = axeHandler.GetComponent<BuildSystemHandler>();

        letterHandler = GameObject.Find("Global/Player/Canvas/Letters").GetComponent<LetterHandler>();
    }

    private void SwordUse(Collider2D[] objects)
    {
        Weapon weapon = (Weapon)item;

        float skillAttackBonus = weapon.AttackPower * 0.02f * skillsHandler.AttackLevel;

        foreach (Collider2D auxObject in objects)
        {
            audioSource.clip = attackClip;
            audioSource.Play();

            if (auxObject.gameObject.CompareTag("Enemy"))
            {
                auxObject.GetComponent<EnemyHealth>().TakeDamage(weapon.AttackPower / attackDecrease + skillAttackBonus);

                auxObject.GetComponent<Rigidbody2D>().AddForce(-(playerMovement.transform.position - auxObject.transform.position) * 1000);
            }
            else if(auxObject.gameObject.CompareTag("Barrel"))
            {
                auxObject.GetComponent<BarrelHandler>().GetDamage(weapon.AttackPower / attackDecrease + skillAttackBonus);
            }
            else if(auxObject.gameObject.CompareTag("EnemyNoForce"))
            {
                auxObject.GetComponent<EnemyHealth>().TakeDamage(weapon.AttackPower / attackDecrease + skillAttackBonus);
            }
        }
    }

    private void SetCircleCastForSword()
    {
        Vector3 castPosition = gameObject.transform.position;

        if((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y <= -1)
        {
            castPosition.y -= DefaulData.castPosition;
        }
        else if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y >= 1)
        {
            castPosition.y += DefaulData.castPosition;
        }
        else if (inputs.x <= -1 && inputs.y == 0)
        {
            castPosition.x -= DefaulData.castPosition;
        }
        else if (inputs.x >= 1 && inputs.y == 0)
        {
            castPosition.x += DefaulData.castPosition;
        }

        Collider2D[] objects = Physics2D.OverlapBoxAll(castPosition, detectionZone, 0);

        SwordUse(objects);
    }

    private void SickleUse(Collider2D[] objects)
    {
        foreach (Collider2D auxObject in objects)
        {
            audioSource.clip = attackClip;
            audioSource.Play();

            if (auxObject.gameObject.tag == "Grass")
            {
                Sickle sickle = (Sickle)item;

                auxObject.GetComponent<GrassDamage>().GetDamage(sickle.Attack);
            }
        }
    }

    private void SetCircleCastForSickle()
    {
        Vector3 castPosition = gameObject.transform.position;

        if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y <= -1)
        {
            castPosition.y -= DefaulData.castPosition;
        }
        else if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y >= 1)
        {
            castPosition.y += DefaulData.castPosition;
        }
        else if (inputs.x <= -1 && inputs.y == 0)
        {
            castPosition.x -= DefaulData.castPosition;
        }
        else if (inputs.x >= 1 && inputs.y == 0)
        {
            castPosition.x += DefaulData.castPosition;
        }

        Collider2D[] objects = Physics2D.OverlapBoxAll(castPosition, detectionZone, 0);

        SickleUse(objects);
    }

    private int GetSpawnLocation()
    {
        if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y <= -1)
        {
            return 4;
        }
        else if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y >= 1)
        {
            return 3;
        }
        else if (inputs.x <= -1 && inputs.y == 0)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private void SelectedItemAction(Item item)
    {
        this.item = item;

        if (playerMovement.CanMove)
        {
            if (item is Axe)
            {
                audioSource.clip = attackClip;
                audioSource.Play();

                animator.SetBool("Axe", true);

                axeHandler.UseAxe(transform.position, GetSpawnLocation(), item);
            }
            else if (item is Pickaxe)
            {
                audioSource.clip = attackClip;
                audioSource.Play();

                animator.SetBool("Pickaxe", true);

                pickaxeHandler.UsePickaxe(transform.position, GetSpawnLocation(), item);
            }
            else if(item is Hoe)
            {
                audioSource.clip = attackClip;
                audioSource.Play();

                animator.SetBool("Hoe", true);

                hoeSystemHandler.PlaceSoil(transform.position, GetSpawnLocation(), (Hoe)item);
            }
            else if (item is Weapon)
            {
                animator.SetBool("Sword", true);

                SetCircleCastForSword();
            }
            else if (item is Sickle)
            {
                animator.SetBool("Sickle", true);

                SetCircleCastForSickle();
            }
            else if (item is Consumable)
            {
                if (playerStats.Eat((Consumable)item))
                {
                    selectedSlot.DecreseAmountSelected(1);

                    selectedSlot.ReinitializeSelectedSlot();
                }
            }
            else if(item is WateringCan)
            {
                WateringCan wateringCan = (WateringCan)item;

                if (wateringCan.RemainWater > 0)
                {
                    animator.SetBool("Wateringcan", true);

                    wateringCan.RemainWater--;

                    selectedSlot.ReinitializeSelectedSlot();

                    wateringCanHandler.UseWateringcan(transform.position, GetSpawnLocation(), (WateringCan)item);

                    if(wateringCan.RemainWater <= 0)
                    {
                        Item newItem = emptyBucket.Copy();
                        newItem.Amount = 1;

                        selectedSlot.SetItem(newItem);

                        selectedSlot.ReinitializeSelectedSlot();
                    }
                }
            }
            else if(item is Letter)
            {
                letterHandler.gameObject.SetActive(true);

                letterHandler.SetData((Letter)item);
            }
        }
    }

    private void Update()
    {
        if(playerMovement.Speed == 0 && playerMovement.CanMove == true && playerMovement.TabOpen == false)
        {
            if (mouse.leftButton.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[5].IsPressed() == false && leftMousePress == false))
            {
                leftMousePress = true;

                Item item = selectedSlot.Item;

                SelectedItemAction(item);
            }
            else if (mouse.rightButton.isPressed || (Joystick.current != null && Joystick.current.allControls[9].IsPressed() == false && rightMousePress == false))
            {
                rightMousePress = true;

                if (selectedSlot.Item is WateringCan && water == true)
                {
                    WateringCan newItem = (WateringCan)fullBucket.Copy();
                    newItem.Amount = 1;

                    newItem.RemainWater = newItem.NoOfUses;

                    selectedSlot.SetItem(newItem);

                    selectedSlot.ReinitializeSelectedSlot();

                    animator.SetBool("WateringcanFill", true);
                }
                else
                {
                    harvestCropHandler.Harvest(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
                }
            }

            if (Joystick.current != null && Joystick.current.allControls[5].IsPressed() == true)
            {
                leftMousePress = false;
            }
            if (Joystick.current != null && Joystick.current.allControls[9].IsPressed() == true)
            {
                rightMousePress = false;
            }
        }
        if (selectedSlot.Item is Hoe)
        {
            hoeSystemHandler.HoeHeadlight(playerMovement.transform.position, GetSpawnLocation());

            buildSystemHandler.StopPlace();
        }
        else if (selectedSlot.Item is Placeable && playerMovement.TabOpen == false)
        {
            buildSystemHandler.StartPlace(selectedSlot.Item);
            hoeSystemHandler.StopHeadlight();
        }
        else
        {
            buildSystemHandler.StopPlace();
            hoeSystemHandler.StopHeadlight();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Water"))
        {
            water = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Water"))
        {
           water = false;
        }
    }
}
