using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTimeDegradation : MonoBehaviour
{
    [SerializeField] private GameObject objectWhereToStart;

    public GameObject ObjectWhereToStart { get => objectWhereToStart; set => objectWhereToStart = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ObjectWhereToStart != null && collision.CompareTag("Player"))
        {
            ObjectWhereToStart.AddComponent<TimeDegradation>();
        }
    }
}
