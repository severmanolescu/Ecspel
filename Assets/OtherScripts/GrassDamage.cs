using System.Collections;
using UnityEngine;

public class GrassDamage : MonoBehaviour
{
    [SerializeField] private float health;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator WaitForSoundEffect()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        GetComponent<BoxCollider2D>().enabled = false;

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private void ChangeGridData(GridNode gridNode, Grid<GridNode> grid)
    {
        if (gridNode.x < grid.gridArray.GetLength(0) &&
            gridNode.y < grid.gridArray.GetLength(1) &&
            grid.gridArray[gridNode.x, gridNode.y] != null)
        {
            grid.gridArray[gridNode.x, gridNode.y].canPlace = true;
            grid.gridArray[gridNode.x, gridNode.y].canPlant = false;
            grid.gridArray[gridNode.x, gridNode.y].isWalkable = true;
            grid.gridArray[gridNode.x, gridNode.y].objectInSpace = null;
        }
    }

    public void GetDamage(float damage)
    {
        health -= damage;

        audioSource.Play();

        if (health <= 0)
        {
            Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

            GridNode gridNode = grid.GetGridObject(transform.position);

            if (gridNode != null)
            {
                ChangeGridData(gridNode, grid);
            }

            StartCoroutine(WaitForSoundEffect());
        }
    }
}
