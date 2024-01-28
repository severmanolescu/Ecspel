using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpTheTime : MonoBehaviour
{
    [SerializeField] private float timeSpeed = 1.0f;

    private void Update()
    {
        Time.timeScale = timeSpeed;
    }
}
