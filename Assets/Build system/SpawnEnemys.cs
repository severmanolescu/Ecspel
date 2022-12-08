using UnityEngine;

public class SpawnEnemys : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;

    private SpawnEnemyInArea[] enemies;

    public Transform SpawnLocation { get => spawnLocation; set => spawnLocation = value; }

    public void SpawnEnemy(LocationGridSave locationGrid)
    {
        enemies = GetComponentsInChildren<SpawnEnemyInArea>();

        foreach (SpawnEnemyInArea enemy in enemies)
        {
            enemy.SpawnEnemy(locationGrid);
        }
    }
}
