using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FootPrintHandler : MonoBehaviour
{
    [SerializeField] private GameObject footPrefabUp;
    [SerializeField] private GameObject footPrefabDown;
    [SerializeField] private GameObject footPrefabLeft;
    [SerializeField] private GameObject footPrefabRight;

    [SerializeField] private Transform spawnLocation;

    [Header("Audio effects")]
    [SerializeField] private AudioClip footstep;
    [SerializeField] private AudioClip footstep1;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void SpawnFootPrintUpRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabUp, spawnLocation.position + new Vector3(.15f, .5f, 0), footPrefabUp.transform.rotation);

            PlayFootstepClip();
        }
    }
    private void SpawnFootPrintUpLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabUp, spawnLocation.position - new Vector3(.15f, -.5f, 0), footPrefabUp.transform.rotation);

            PlayFootstep1Clip();
        }
    }
    private void SpawnFootPrintDownRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabDown, spawnLocation.position + new Vector3(.15f, .5f, 0), footPrefabDown.transform.rotation);

            PlayFootstepClip();
        }
    }
    private void SpawnFootPrintDownLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabDown, spawnLocation.position - new Vector3(.15f, -.5f, 0), footPrefabDown.transform.rotation);

            PlayFootstep1Clip();
        }
    }
    private void SpawnFootPrintLeftRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabLeft, spawnLocation.position + new Vector3(0, .70f, 0), footPrefabLeft.transform.rotation);

            PlayFootstepClip();
        }
    }
    private void SpawnFootPrintLeftLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabLeft, spawnLocation.position - new Vector3(0, -.62f, 0), footPrefabLeft.transform.rotation);

            PlayFootstep1Clip();
        }
    }
    private void SpawnFootPrintRightRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabRight, spawnLocation.position + new Vector3(0, .70f, 0), footPrefabRight.transform.rotation);

            PlayFootstepClip();
        }
    }
    private void SpawnFootPrintRightLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            Instantiate(footPrefabRight, spawnLocation.position - new Vector3(0, -.62f, 0), footPrefabRight.transform.rotation);

            PlayFootstep1Clip();
        }
    }

    private void PlayFootstepClip()
    {
        audioSource.clip = footstep;
        audioSource.Play();
    }

    private void PlayFootstep1Clip()
    {
        audioSource.clip = footstep1;
        audioSource.Play();
    }
}
