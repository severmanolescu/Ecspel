using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private int maxX;
    [SerializeField] private int minX;

    [SerializeField] private int maxY;
    [SerializeField] private int minY;

    private Transform playerLocation;

    public int MaxX { get => maxX; set => maxX = value; }
    public int MinX { get => minX; set => minX = value; }
    public int MaxY { get => maxY; set => maxY = value; }
    public int MinY { get => minY; set => minY = value; }

    private void Awake()
    {
        playerLocation = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        Vector3 position = new Vector3();

        if (playerLocation.transform.position.x >= MinX && playerLocation.transform.position.x <= MaxX)
        {
            position.x = playerLocation.position.x;
        }
        else
        {
            if (playerLocation.transform.position.x < MinX)
            {
                position.x = minX;
            }
            else if (playerLocation.transform.position.x > MaxX)
            {
                position.x = maxX;
            }
        }

        if (playerLocation.transform.position.y >= MinY && playerLocation.transform.position.y <= MaxY)
        {
            position.y = playerLocation.position.y;
        }
        else
        {
            if (playerLocation.transform.position.y < MinY)
            {
                position.y = MinY;
            }
            else if (playerLocation.transform.position.y > MaxY)
            {
                position.y = MaxY;
            }
        }

        position.z = transform.position.z;

        transform.position = position;
    }
}
