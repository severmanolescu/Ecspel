using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger selectedSlot;

    private Item item;

    private PlayerMovement playerMovement;

    private Animator animator;

    private Vector2 inputs = Vector2.zero;

    private Vector2 detectionZone = new Vector2(1f, 1f);

    public Vector2 Inputs { set { inputs = value; } }

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();

        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    private void AxeUse(Collider2D[] objects, int spawn)
    {
        foreach (Collider2D auxObject in objects)
        {
            if (auxObject.gameObject != this.gameObject)
            {
                DamageTree treeDamage = auxObject.GetComponent<DamageTree>();

                if (treeDamage != null)
                {
                    Axe axe = (Axe)item;

                    treeDamage.TakeDamage(axe.Damage, spawn, axe.Level);

                    return;
                }
                else if (auxObject.CompareTag("Crop"))
                {
                    auxObject.GetComponent<CropGrow>().Destroy();

                    return;
                }
            }
        }
    }

    private void PickaxeUse(Collider2D[] objects, int spawn)
    {
        foreach (Collider2D auxObject in objects)
        {
            if (auxObject.gameObject != this.gameObject)
            {
                StoneDamage stoneDamage = auxObject.GetComponent<StoneDamage>();

                Pickaxe pickaxe= (Pickaxe)item;

                if (stoneDamage != null)
                {
                    stoneDamage.TakeDamage(pickaxe.Damage, pickaxe.Level);

                    return;
                }
                else if (auxObject.CompareTag("Crop"))
                {
                    auxObject.GetComponent<CropGrow>().Destroy();

                    return;
                }
            }
        }
    }

    private void SwordUse(Collider2D[] objects, int spawn)
    {
        foreach (Collider2D auxObject in objects)
        {
            if (auxObject.gameObject.tag == "Enemy")
            {
                Weapon weapon = (Weapon)item;

                auxObject.GetComponent<EnemyHealth>().TakeDamage(weapon.AttackPower);

                auxObject.GetComponent<Rigidbody2D>().AddForce(-(playerMovement.transform.position - auxObject.transform.position) * 1000);
            }
        }
    }

    private void SetCircleCast()
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

        SwordUse(objects, GetSpawnLocation()); return;
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
                animator.SetBool("Axe", true);

                GameObject.Find("Global/BuildSystem").GetComponent<AxeHandler>().UseAxe(transform.position, GetSpawnLocation(), item);
            }
            else if (item is Pickaxe)
            {
                animator.SetBool("Pickaxe", true);

                GameObject.Find("Global/BuildSystem").GetComponent<PickaxeHandler>().UsePickaxe(transform.position, GetSpawnLocation(), item);
            }
            else if(item is Hoe)
            {
                animator.SetBool("Hoe", true);

                GameObject.Find("Global/BuildSystem").GetComponent<HoeSystemHandler>().PlaceSoil(transform.position, GetSpawnLocation(), (Hoe)item);
            }
            else if (item is Weapon)
            {
                animator.SetBool("Sword", true);

                SetCircleCast();
            }
        }
    }

    private void Update()
    {
        if(playerMovement.Speed == 0 && playerMovement.CanMove == true && playerMovement.TabOpen == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Item item = selectedSlot.Item;

                SelectedItemAction(item);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                GameObject.Find("Global/BuildSystem").GetComponent<HarvestCropHandler>().Harvest(Camera.main.ScreenToWorldPoint(Input.mousePosition));
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
