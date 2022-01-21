using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionPlayerAtLoad : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;

    [SerializeField] private GameObject houseCamera;

    [SerializeField] private GameObject houseLocation;
    [SerializeField] private GameObject footPrintSpawnLocation;

    public void PositionPlayer()
    {
        transform.localPosition = startPosition;

        houseCamera.SetActive(true);

        houseLocation.SetActive(true);

        footPrintSpawnLocation.SetActive(false);
    }
}
