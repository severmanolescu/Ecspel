using System.Collections;
using UnityEngine;

public class DestroyOvertime : MonoBehaviour
{
    [SerializeField] private float lifeDuration = 2f;

    private void Start()
    {
        StartCoroutine(WaitForDestroy());
    }

    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(lifeDuration);

        Destroy(gameObject);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
