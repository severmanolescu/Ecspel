using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivatePlayerFootSteps : MonoBehaviour
{
    [SerializeField] private GameObject footstep;

    private void Start()
    {
        footstep.SetActive(false);
    }
}
