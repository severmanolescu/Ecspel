using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;


    [SerializeField] private bool setRainSound = true;

    private AudioSource audioSource;

    public Transform TeleportToPoint { get => teleportToPoint; set => teleportToPoint = value; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void SetObject()
    {
        foreach (GameObject gameObject in objectsToSetActiveToFalse)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in objectsToSetActiveToTrue)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // collision.transform.position = TeleportToPoint.position;

            GameObject.Find("Global/Player/Canvas/Transition").GetComponent<TransitionHandler>().PlayTransition(TeleportToPoint.position, false);


            SetObject();
        }
    }
}
