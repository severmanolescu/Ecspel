using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountPlayedMinutes : MonoBehaviour
{
    [SerializeField] private ulong minutes = 0;

    [SerializeField] private ushort seconds = 0;

    public ulong Minutes { get => minutes; set => minutes = value; }
    public ushort Seconds { get => seconds; set => seconds = value; }

    private void Start()
    {
        StartCoroutine(CountMinutes());
    }

    private IEnumerator CountMinutes()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            Seconds++;

            if(Seconds >= 60)
            {
                Minutes++;

                Seconds = 0;
            }
        }
    }
}
