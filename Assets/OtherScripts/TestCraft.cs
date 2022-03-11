using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCraft : MonoBehaviour
{
    [Range(0f,10f)]
    public float speed = 1f;

    private void Update()
    {
        Time.timeScale = speed;
    }
}
