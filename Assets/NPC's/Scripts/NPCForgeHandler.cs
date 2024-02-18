using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCForgeHandler : MonoBehaviour
{
    [Header("Time in seconds for fire to stay on")]
    [SerializeField] private float fireTime;

    private ParticleSystem particleSystem;

    private BlacksmithHandler blacksmithHandler;

    private bool fireOn = false;

    public bool FireOn { get => fireOn; }

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public bool blacksmithStartShift(BlacksmithHandler blacksmithHandler)
    {
        if(blacksmithHandler != null)
        {
            this.blacksmithHandler = blacksmithHandler;

            return fireOn;
        }

        return fireOn;
    }

    public void FuelForge()
    {
        StopAllCoroutines();

        fireOn = true;

        particleSystem.Play();
       
        StartCoroutine(WaitForFire());
    }

    private void StopForge()
    {
        fireOn = false;

        particleSystem.Stop();
    }

    private IEnumerator WaitForFire()
    {
        yield return new WaitForSeconds(.8f * fireTime);

        if(blacksmithHandler != null)
        {
            blacksmithHandler.ForgeNeedsFuel();
        }

        yield return new WaitForSeconds(.2f * fireTime);

        StopForge();
    }
}
