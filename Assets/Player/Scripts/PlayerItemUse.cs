using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger selectedSlot;

    [Header("Audio effects")]
    [SerializeField] private AudioClip attackClip;

    private PlayerStats playerStats;

    private AudioSource audioSource;

    private Item item;

    private PlayerMovement playerMovement;

    private Animator animator;

    private SkillsHandler skillsHandler;

    private Mouse mouse;

    private Vector2 inputs = Vector2.zero;

    private Vector2 detectionZone = new Vector2(1.5f, 1.5f);

    private float attackDecrease = 1f;

    public Vector2 Inputs { set { inputs = value; } }
    public float AttackDecrease { set { attackDecrease = value; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();

        mouse = InputSystem.GetDevice<Mouse>();

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        skillsHandler = GetComponentInChildren<SkillsHandler>();
    }

    private void SwordUse(Collider2D[] objects)
    {
        Weapon weapon = (Weapon)item;

        float skillAttackBonus = weapon.AttackPower * 0.02f * skillsHandler.AttackLevel;

        foreach (Collider2D auxObject in objects)
        {
            audioSource.clip = attackClip;
            audioSource.Play();

            if (auxObject.gameObject.tag == "Enemy")
            {
                auxObject.GetComponent<EnemyHealth>().TakeDamage(weapon.AttackPower / attackDecrease + skillAttackBonus);

                auxObject.GetComponent<Rigidbody2D>().AddForce(-(playerMovement.transform.position - auxObject.transform.position) * 1000);
            }
            else if(auxObject.gameObject.tag == "Barrel")
            {
                auxObject.GetComponent<BarrelHandler>().GetDamage(weapon.AttackPower / attackDecrease + skillAttackBonus);
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

                GameObject.Find("Global/BuildSystem").GetComponent<AxeHandler>().UseAxe(transform.position, GetSpawnLocation(), item);
            }
            else if (item is Pickaxe)
            {
                audioSource.clip = attackClip;
                audioSource.Play();

                animator.SetBool("Pickaxe", true);

                GameObject.Find("Global/BuildSystem").GetComponent<PickaxeHandler>().UsePickaxe(transform.position, GetSpawnLocation(), item);
            }
            else if(item is Hoe)
            {
                audioSource.clip = attackClip;
                audioSource.Play();

                animator.SetBool("Hoe", true);

                GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>().PlaceSoil(transform.position, GetSpawnLocation(), (Hoe)item);
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
        }
    }

    private void Update()
    {
        if(playerMovement.Speed == 0 && playerMovement.CanMove == true && playerMovement.TabOpen == false)
        {
            if (mouse.leftButton.wasPressedThisFrame)
            {
                Item item = selectedSlot.Item;

                SelectedItemAction(item);
            }
            else if (mouse.rightButton.isPressed)
            {
                GameObject.Find("Global/BuildSystem").GetComponent<HarvestCropHandler>().Harvest(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            }
            
        }
        if (selectedSlot.Item is Hoe)
        {
            GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>().HoeHeadlight(playerMovement.transform.position, GetSpawnLocation());

            GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().StopPlace();
        }
        else if (selectedSlot.Item is Placeable && playerMovement.TabOpen == false)
        {
            GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().StartPlace(selectedSlot.Item);
            GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>().StopHeadlight();
        }
        else
        {
            GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().StopPlace();
            GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>().StopHeadlight();
        }
    }
}
