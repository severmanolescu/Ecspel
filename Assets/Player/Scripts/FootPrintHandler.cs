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
        Instantiate(footPrefabUp, spawnLocation.position + new Vector3(.15f, 0, 0), footPrefabUp.transform.rotation);
    }
    private void SpawnFootPrintUpLeft()
    {
        Instantiate(footPrefabUp, spawnLocation.position - new Vector3(.15f, 0, 0), footPrefabUp.transform.rotation);
    }
    private void SpawnFootPrintDownRight()
    {
        Instantiate(footPrefabDown, spawnLocation.position + new Vector3(.15f, 0, 0), footPrefabDown.transform.rotation);
    }
    private void SpawnFootPrintDownLeft()
    {
        Instantiate(footPrefabDown, spawnLocation.position - new Vector3(.15f, 0, 0), footPrefabDown.transform.rotation);
    }
    private void SpawnFootPrintLeftRight()
    {
        Instantiate(footPrefabLeft, spawnLocation.position + new Vector3(0, .15f, 0), footPrefabLeft.transform.rotation);
    }
    private void SpawnFootPrintLeftLeft()
    {
        Instantiate(footPrefabLeft, spawnLocation.position - new Vector3(0, .05f, 0), footPrefabLeft.transform.rotation);
    }
    private void SpawnFootPrintRightRight()
    {
        Instantiate(footPrefabRight, spawnLocation.position + new Vector3(0, .15f, 0), footPrefabRight.transform.rotation);
    }
    private void SpawnFootPrintRightLeft()
    {
        Instantiate(footPrefabRight, spawnLocation.position - new Vector3(0, .05f, 0), footPrefabRight.transform.rotation);
    }
}
