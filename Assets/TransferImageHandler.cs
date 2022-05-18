using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransferImageHandler : MonoBehaviour
{
    [SerializeField] private Image backImage;

    [SerializeField] private Color startColor;

    private void Awake()
    {
        backImage.color = startColor;

        backImage.enabled = false;
    }

    private IEnumerator WaitForImage()
    {
        while (startColor.a < 1f)
        {
            startColor.a += 0.01f;

            backImage.color = startColor;

            yield return new WaitForSeconds(0.01f);
        }
    }

    public void StartTransfer()
    {
        startColor.a = 0f;

        backImage.color = startColor;

        backImage.enabled = true;

        StartCoroutine(WaitForImage());
    }

    public void HideImage()
    {
        backImage.enabled = false;
    }
}
