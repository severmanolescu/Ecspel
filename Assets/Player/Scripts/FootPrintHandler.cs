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
    [SerializeField] private AudioClip footstepWood;
    [SerializeField] private AudioClip footstepWood1;

    private AudioSource audioSource;

    public AudioClip Footstep { get => footstep; set => footstep = value; }
    public AudioClip Footstep1 { get => footstep1; set => footstep1 = value; }
    public AudioClip FootstepWood { get => footstepWood; set => footstepWood = value; }
    public AudioClip FootstepWood1 { get => footstepWood1; set => footstepWood1 = value; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeFootprintSpawnLocationState(bool state)
    {
        if(spawnLocation != null)
        {
            spawnLocation.gameObject.SetActive(state);
        }
    }

    private void SpawnFootPrintUpRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabUp, spawnLocation.position + new Vector3(.15f, .5f, 0), footPrefabUp.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstepClip();
        }
        else
        {
            PlayFootstepWoodClip();
        }
    }
    private void SpawnFootPrintUpLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabUp, spawnLocation.position - new Vector3(.15f, -.5f, 0), footPrefabUp.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstep1Clip();
        }
        else
        {
            PlayFootstepWood1Clip();
        }
    }
    private void SpawnFootPrintDownRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabDown, spawnLocation.position + new Vector3(.15f, .5f, 0), footPrefabDown.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstepClip();
        }
        else
        {
            PlayFootstepWoodClip();
        }
    }
    private void SpawnFootPrintDownLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabDown, spawnLocation.position - new Vector3(.15f, -.5f, 0), footPrefabDown.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstep1Clip();
        }
        else
        {
            PlayFootstepWood1Clip();
        }
    }
    private void SpawnFootPrintLeftRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabLeft, spawnLocation.position + new Vector3(0, .70f, 0), footPrefabLeft.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstepClip();
        }else
        {
            PlayFootstepWoodClip();
        }
    }
    private void SpawnFootPrintLeftLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabLeft, spawnLocation.position - new Vector3(0, -.62f, 0), footPrefabLeft.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstep1Clip();
        }
        else
        {
            PlayFootstepWood1Clip();
        }
    }
    private void SpawnFootPrintRightRight()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabRight, spawnLocation.position + new Vector3(0, .70f, 0), footPrefabRight.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstepClip();
        }
        else
        {
            PlayFootstepWoodClip();
        }
    }
    private void SpawnFootPrintRightLeft()
    {
        if (spawnLocation.gameObject.activeSelf == true)
        {
            GameObject instantiateObject = Instantiate(footPrefabRight, spawnLocation.position - new Vector3(0, -.62f, 0), footPrefabRight.transform.rotation);

            instantiateObject.GetComponent<SpriteRenderer>().sortingLayerName = "Ground";
            instantiateObject.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PlayFootstep1Clip();
        }
        else
        {
            PlayFootstepWood1Clip();
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

    private void PlayFootstepWoodClip()
    {
        audioSource.clip = FootstepWood;
        audioSource.Play();
    }

    private void PlayFootstepWood1Clip()
    {
        audioSource.clip = FootstepWood1;
        audioSource.Play();
    }
}
