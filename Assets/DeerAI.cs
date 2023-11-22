using UnityEngine;

public class DeerAI : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boxCollider;

    public bool moveToRandomPosition = false;

    private Vector3 GetNewLocation()
    {
        return new Vector3(Random.Range(boxCollider.transform.position.x - boxCollider.size.x / 2, boxCollider.transform.position.x + boxCollider.size.x / 2),
                       Random.Range(boxCollider.transform.position.y - boxCollider.size.y / 2, boxCollider.transform.position.y + boxCollider.size.y / 2),
                       transform.position.z);
    }

    private void Update()
    {
        if(moveToRandomPosition)
        {
            GetComponent<PathFindingAnimals>().ChangeLocation(GetNewLocation());

            moveToRandomPosition = false;
        }
    }
}
