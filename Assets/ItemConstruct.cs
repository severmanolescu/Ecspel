using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConstruct : MonoBehaviour
{
    private new BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;

    private bool canDrag  = true;
    private bool canPlace = true;

    private void Start()
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

            Vector3 objectPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            objectPosition.z = gameObject.transform.position.z;

            gameObject.transform.position = objectPosition;
        }

        if (Input.GetMouseButtonDown(0))
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
