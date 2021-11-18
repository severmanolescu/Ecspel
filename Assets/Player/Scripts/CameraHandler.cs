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

    private void Awake()
    {
        playerLocation = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        Vector3 position = new Vector3();

        if(playerLocation.transform.position.x >= minX && playerLocation.transform.position.x <= maxX)
        {
            position.x = playerLocation.position.x;
        }
        else
        {
            position.x = transform.position.x;
        }

        if (playerLocation.transform.position.y >= minY && playerLocation.transform.position.y <= maxY)
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
