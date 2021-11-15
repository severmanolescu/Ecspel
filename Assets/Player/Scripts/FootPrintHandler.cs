using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrintHandler : MonoBehaviour
{
    [SerializeField] private GameObject footPrefabUp;
    [SerializeField] private GameObject footPrefabDown;
    [SerializeField] private GameObject footPrefabLeft;
    [SerializeField] private GameObject footPrefabRight;

    [SerializeField] private Transform spawnLocation;

    private void SpawnFootPrintUpRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabUp, spawnLocation.position + new Vector3(.15f, .5f, 0), footPrefabUp.transform.rotation);
        }
    }
    private void SpawnFootPrintUpLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabUp, spawnLocation.position - new Vector3(.15f, -.5f, 0), footPrefabUp.transform.rotation);
        }
    }
    private void SpawnFootPrintDownRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabDown, spawnLocation.position + new Vector3(.15f, .5f, 0), footPrefabDown.transform.rotation);
        }
    }
    private void SpawnFootPrintDownLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabDown, spawnLocation.position - new Vector3(.15f, -.5f, 0), footPrefabDown.transform.rotation);
        }
    }
    private void SpawnFootPrintLeftRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabLeft, spawnLocation.position + new Vector3(0, .70f, 0), footPrefabLeft.transform.rotation);
        }
    }
    private void SpawnFootPrintLeftLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabLeft, spawnLocation.position - new Vector3(0, -.62f, 0), footPrefabLeft.transform.rotation);
        }
    }
    private void SpawnFootPrintRightRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabRight, spawnLocation.position + new Vector3(0, .70f, 0), footPrefabRight.transform.rotation);
        }
    }
    private void SpawnFootPrintRightLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabRight, spawnLocation.position - new Vector3(0, -.62f, 0), footPrefabRight.transform.rotation);
        }
    }
}
