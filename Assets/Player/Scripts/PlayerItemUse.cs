using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger selectedSlot;

    [Header("Audio effects")]
    [SerializeField] private AudioClip attackClip;

    [SerializeField] private Item emptyBucket;
    [SerializeField] private Item fullBucket;

    [SerializeField] private Camera mainCamera;

    [SerializeField] private float position = 1f;
    [SerializeField] private float detectionRadius = 1f;

    [SerializeField] private LocationGridSave grid;

    [SerializeField] private int maxDistanceFromPlayer = 3;

    private PlayerStats playerStats;

    private AudioSource audioSource;

    private Item usedItem;

    private PlayerMovement playerMovement;

    private Animator animator;

    private SkillsHandler skillsHandler;

    private PlayerItemUseSpriteChange spriteChange;

    private AxeHandler axeHandler;
    private PickaxeHandler pickaxeHandler;
    private HoeSystemHandler hoeSystemHandler;
    private WateringCanHandler wateringCanHandler;
    private HarvestCropHandler harvestCropHandler;
    private BuildSystemHandler buildSystemHandler;

    private LetterHandler letterHandler;

    private Direction lastDirection;
    private GridNode objectGrid;

    private bool water = false;

    private Mouse mouse;

    private Vector2 inputs = Vector2.zero;

    private float attackDecrease = 1f;

    public Vector2 Inputs { set { inputs = value; } }
    public float AttackDecrease { set { attackDecrease = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();

        mouse = InputSystem.GetDevice<Mouse>();

        spriteChange = GetComponentInChildren<PlayerItemUseSpriteChange>();

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
        Weapon weapon = (Weapon)usedItem;

        float skillAttackBonus = weapon.AttackPower * 0.02f * skillsHandler.AttackLevel;

        audioSource.clip = attackClip;
        audioSource.Play();

        foreach (Collider2D auxObject in objects)
        {
            if (auxObject.gameObject.CompareTag("EnemyHit"))
            {
 //               auxObject.GetComponent<EnemyHitHandler>().AttackEnemy(weapon.AttackPower / attackDecrease + skillAttackBonus);
            }
            else if (auxObject.gameObject.CompareTag("Barrel"))
            {
                auxObject.GetComponent<BarrelHandler>().GetDamage(weapon.AttackPower / attackDecrease + skillAttackBonus);
            }
        }
    }

    private void SetCircleCastForSword()
    {
        Vector3 castPosition = transform.position;

        if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y <= -1)
        {
            castPosition.y -= position;
        }
        else if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y >= 1)
        {
            castPosition.y += position;
        }
        else if (inputs.x <= -1 && inputs.y == 0)
        {
            castPosition.x -= position;
        }
        else if (inputs.x >= 1 && inputs.y == 0)
        {
            castPosition.x += position;
        }

        Collider2D[] objects = Physics2D.OverlapCircleAll(castPosition, detectionRadius);

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
                Sickle sickle = (Sickle)usedItem;

                auxObject.GetComponent<GrassDamage>().GetDamage(sickle.Attack);
            }
        }
    }

    private void SetCircleCastForSickle()
    {
        Vector3 castPosition = transform.position;

        if (lastDirection == Direction.Down)
        {
            castPosition.y -= DefaulData.castPosition;
        }
        else if (lastDirection == Direction.Up)
        {
            castPosition.y += DefaulData.castPosition;
        }
        else if (lastDirection == Direction.Left)
        {
            castPosition.x -= DefaulData.castPosition;
        }
        else if (lastDirection == Direction.Right)
        {
            castPosition.x += DefaulData.castPosition;
        }

        Collider2D[] objects = Physics2D.OverlapCircleAll(castPosition, detectionRadius);

        SickleUse(objects);
    }

    private void ChangeAnimatorDirectionVariables(int horizontal, int vertical)
    {
        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);

        animator.SetFloat("HorizontalFacing", horizontal);
        animator.SetFloat("VerticalFacing", vertical);

        inputs.x = horizontal;
        inputs.y = vertical;
    }

    private Direction GetSpawnLocation()
    {
        Vector3 mousePosition = mouse.position.ReadValue();
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        GridNode mousePositionGrid = grid.Grid.GetGridObject(mainCamera.ScreenToWorldPoint(mousePosition));
        GridNode playerPosition = grid.Grid.GetGridObject(transform.position);

        if (usedItem is Weapon ||
            (mousePositionGrid != null &&
            mousePositionGrid.x >= playerPosition.x - maxDistanceFromPlayer &&
            mousePositionGrid.x <= playerPosition.x + maxDistanceFromPlayer &&
            mousePositionGrid.y >= playerPosition.y - maxDistanceFromPlayer &&
            mousePositionGrid.y <= playerPosition.y + maxDistanceFromPlayer))
        {
            objectGrid = mousePositionGrid;

            int differenceX = mousePositionGrid.x - playerPosition.x;
            int differenceY = mousePositionGrid.y - playerPosition.y;

            if(differenceY < 0)
            {
                if(Mathf.Abs(differenceX) <= Mathf.Abs(differenceY))
                {
                    ChangeAnimatorDirectionVariables(0, -1);

                    return lastDirection = Direction.Down;
                }
            }

            if (differenceY > 0)
            {
                if (Mathf.Abs(differenceX) <= Mathf.Abs(differenceY))
                {
                    ChangeAnimatorDirectionVariables(0, 1);

                    return lastDirection = Direction.Up;  
                }
            }

            if (differenceX > 0)
            {
                if (Mathf.Abs(differenceY) <= Mathf.Abs(differenceX))
                {
                    ChangeAnimatorDirectionVariables(1, 0);

                    return lastDirection = Direction.Right;
                }
            }

            ChangeAnimatorDirectionVariables(-1, 0);

            return lastDirection = Direction.Left;
        }
        else
        {
            objectGrid = null;

            if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y <= -1)
            {
                return lastDirection = Direction.Down;
            }
            else if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y >= 1)
            {
                return lastDirection = Direction.Up;
            }
            else if (inputs.x <= -1 && inputs.y == 0)
            {
                return lastDirection = Direction.Left;
            }
            else
            {
                return lastDirection = Direction.Right;
            }
        }
    }

    public void UseItem()
    {
        if (usedItem != null)
        {
            switch (usedItem)
            {
                case Axe:
                    {
                        axeHandler.UseAxe((int)lastDirection, usedItem, objectGrid);

                        break;
                    }
                case Pickaxe:
                    {
                        pickaxeHandler.UsePickaxe(usedItem, objectGrid);

                        break;
                    }
                case WateringCan:
                    {
                        wateringCanHandler.UseWateringcan((WateringCan)usedItem, objectGrid);

                        break;
                    }
            }
        }
    }

    private void SelectedItemAction(Item item)
    {
        usedItem = item;

        if (playerMovement.CanMove)
        {
            switch (item)
            {
                case Axe:
                    {
                        audioSource.clip = attackClip;
                        audioSource.Play();

                        spriteChange.StartUse(item, GetSpawnLocation());

                        break;
                    }
                case Pickaxe:
                    {
                        audioSource.clip = attackClip;
                        audioSource.Play();

                        spriteChange.StartUse(item, GetSpawnLocation());

                        break;
                    }
                case Hoe:
                    {
                        if (hoeSystemHandler.PlaceSoil((Hoe)item) == true ||
                            hoeSystemHandler.DestroyCrop((Hoe)item) == true)
                        {
                            audioSource.clip = attackClip;
                            audioSource.Play();

                            spriteChange.StartUse(item, GetSpawnLocation());
                        }

                        break;
                    }
                case Weapon:
                    {
                        GetSpawnLocation();

                        spriteChange.StartUse(item, Direction.Left);

                        SetCircleCastForSword();

                        break;
                    }
                case Sickle:
                    {
                        spriteChange.StartUse(item, GetSpawnLocation());

                        SetCircleCastForSickle();

                        break;
                    }
                case Consumable:
                    {
                        if (playerStats.Eat((Consumable)item))
                        {
                            selectedSlot.DecreseAmountSelected(1);

                            selectedSlot.ReinitializeSelectedSlot();
                        }

                        break;
                    }
                case WateringCan:
                    {
                        WateringCan wateringCan = (WateringCan)item;

                        if (wateringCan.RemainWater > 0)
                        {
                            spriteChange.StartUse(item, GetSpawnLocation());

                            animator.SetBool("Wateringcan", true);

                            wateringCan.RemainWater--;

                            selectedSlot.ReinitializeSelectedSlot();

                            if (wateringCan.RemainWater <= 0)
                            {
                                Item newItem = emptyBucket.Copy();
                                newItem.Amount = 1;

                                selectedSlot.SetItem(newItem);

                                selectedSlot.ReinitializeSelectedSlot();
                            }
                        }

                        break;
                    }
                case Letter:
                    {
                        letterHandler.gameObject.SetActive(true);

                        letterHandler.SetData((Letter)item);

                        break;
                    }
            }
        }
    }

    public void ChangeToNextSprite()
    {
        spriteChange.ChangeToNextSprite();
    }

    public void StopShow()
    {
        spriteChange.StopShow();
    }

    private void Update()
    {
        if (playerMovement.Speed == 0 && playerMovement.CanMove == true && playerMovement.TabOpen == false)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                SelectedItemAction(selectedSlot.Item);
            }
            else if (mouse.rightButton.isPressed)
            {
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
                    harvestCropHandler.Harvest();
                }
            }

            if (selectedSlot.Item is Hoe)
            {
                hoeSystemHandler.HoeHeadlight(playerMovement.transform.position);

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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Water"))
        {
            water = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Water"))
        {
            water = false;
        }
    }
}
