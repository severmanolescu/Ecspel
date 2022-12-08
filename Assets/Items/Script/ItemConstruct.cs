using UnityEngine;
using UnityEngine.InputSystem;

public class ItemConstruct : MonoBehaviour
{
    private new BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;

    private bool canDrag = true;
    private bool canPlace = true;

    private void Awake()
    {
        collider = gameObject.GetComponent<BoxCollider2D>();

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (canDrag)
        {
            if (canPlace)
            {
                spriteRenderer.color = Color.green;
            }
            else
            {
                spriteRenderer.color = Color.red;
            }

            Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            objectPosition.z = gameObject.transform.position.z;

            gameObject.transform.position = objectPosition;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (canPlace)
            {
                StopDrag();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        canPlace = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canPlace = true;
    }

    public void StartDrag()
    {
        canDrag = true;

        collider.isTrigger = true;
    }

    public void StopDrag()
    {
        collider.isTrigger = false;

        canDrag = false;

        spriteRenderer.color = Color.white;
    }
}
