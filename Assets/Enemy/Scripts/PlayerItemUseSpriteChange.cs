using UnityEngine;

public class PlayerItemUseSpriteChange : MonoBehaviour
{
    private Animator animator;

    private SpriteRenderer spriteRenderer;

    private ItemUse item;

    private int spriteIndex = 0;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void StartUse(Item item, Direction direction)
    {
        if (item != null && item is ItemUse)
        {
            this.item = (ItemUse)item;

            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    {
                        spriteRenderer.sprite = this.item.ItemUseLateral;

                        break;
                    }
                case Direction.Down:
                    {
                        spriteRenderer.sprite = this.item.ItemUseFront[0];

                        spriteIndex = 1;

                        break;
                    }
                case Direction.Up:
                    {
                        spriteRenderer.sprite = this.item.ItemUseBack;

                        break;
                    }
            }

            animator.SetBool("ItemUse", true);
        }
        else if (item is Weapon)
        {
            spriteRenderer.sprite = item.ItemSprite;

            animator.SetBool("Sword", true);
        }
    }

    public void ChangeToNextSprite()
    {
        spriteRenderer.sprite = item.ItemUseFront[spriteIndex];

        spriteIndex++;
    }

    public void StopShow()
    {
        spriteRenderer.sprite = null;

        item = null;
    }
}
