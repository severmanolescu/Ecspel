using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Kill enemy", order = 1)]
public class KillEnemy : Quest
{
    [Header("Number of enemy to kill:")]
    public int number;

    [Header("Enemy id:\n" +
        "1. Slime\n" +
        "2. Potion\n" +
        "3. Puddle")]
    public int enemyId;

    public int Number { get { return number; } }

    public int EnemyId { get => enemyId; set => enemyId = value; }
}
