using System.Collections;
using System.Collections.Generic;
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

        if(playerLocation.transform.position.x >= MinX && playerLocation.transform.position.x <= MaxX)
        {
            position.x = playerLocation.position.x;
        }
        else
        {
            position.x = transform.position.x;
        }

        if (playerLocation.transform.position.y >= MinY && playerLocation.transform.position.y <= MaxY)
        {
            position.y = playerLocation.position.y;
        }
        else
        {
            position.y = transform.position.y;
        }

        position.z = transform.position.z;

        transform.position = position;
    }
}
