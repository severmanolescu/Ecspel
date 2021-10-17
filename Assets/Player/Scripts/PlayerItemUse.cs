using UnityEngine;

public class PlayerItemUse : MonoBehaviour
{
    [SerializeField] private QuickSlotsChanger selectedSlot;

    private PlayerMovement playerMovement;

    private Animator animator;

    private Vector2 inputs = Vector2.zero;

    private Vector2 detectionZone = new Vector2(.5f, .5f);

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
                TreeDamage treeDamage = auxObject.GetComponent<TreeDamage>();

                if (treeDamage != null)
                {
                    treeDamage.TakeDamage(2, spawn);
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

                if (stoneDamage != null)
                {
                    stoneDamage.TakeDamage(2);
                }
            }
        }
    }

    private void SwordUse(Collider2D[] objects, int spawn)
    {
        foreach (Collider2D auxObject in objects)
        {
            if (auxObject.gameObject != this.gameObject)
            {
                TreeDamage treeDamage = auxObject.GetComponent<TreeDamage>();

                if (treeDamage != null)
                {
                    treeDamage.TakeDamage(2, spawn);
                }
            }
        }
    }

    private void SetCircleCast(ushort itemUse)
    {
        Vector3 castPosition = gameObject.transform.position;

        int spawn = 1;

        if((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y <= -1)
        {
            castPosition.y -= DefaulData.castPosition * 3;

            spawn = 4;
        }
        else if ((inputs.x == 0 || inputs.x >= 1 || inputs.x <= -1) && inputs.y >= 1)
        {
            castPosition.y += DefaulData.castPosition;

            spawn = 3;
        }
        else if (inputs.x <= -1 && inputs.y == 0)
        {
            castPosition.x -= DefaulData.castPosition;

            spawn = 1;
        }
        else if (inputs.x >= 1 && inputs.y == 0)
        {
            castPosition.x += DefaulData.castPosition;

            spawn = 2;
        }

        Collider2D[] objects = Physics2D.OverlapBoxAll(castPosition, detectionZone, 0);

        switch(itemUse)
        {
            case 1: AxeUse(objects, spawn); return;
            case 2: PickaxeUse(objects, spawn); return;
            case 3: SwordUse(objects, spawn); return;
            default: return;
        }
    }

    private void SelectedItemAction(Item item)
    {
        if (playerMovement.CanMove)
        {
            if (item is Axe)
            {
                animator.SetBool("Axe", true);

                SetCircleCast(1);
            }
            else if (item is Pickaxe)
            {
                animator.SetBool("Pickaxe", true);

                SetCircleCast(2);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && playerMovement.Speed == 0)
        {
            Item item = selectedSlot.Item;

            SelectedItemAction(item);
        }
    }
}

// itemUse:
//  1 - Axe
//  2 - Pickaxe
//  3 - Sword
