using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGridCellValuesByObjects : MonoBehaviour
{
    public Grid<GridNode> grid;

    public Grid<GridNode> Grid { get => grid; set => grid = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (grid != null && !collision.CompareTag("CheckCell") && collision.CompareTag("Untagged"))
        {
            StopAllCoroutines();

            if (GetComponent<BoxCollider2D>().isTrigger == true)
            {
                Destroy(gameObject);
            }
            else
            {
                GridNode gridNode = grid.GetGridObject(transform.position);

                gridNode.isWalkable = false;

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.05f);

        Destroy(gameObject);
    }

    public void SetComponents()
    {
        gameObject.AddComponent<Rigidbody2D>().gravityScale = 0;

        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();

        boxCollider.isTrigger = true;
        boxCollider.size = new Vector2(.75f, .75f);

        gameObject.tag = "CheckCell";
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }
}