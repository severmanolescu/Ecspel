using System.Collections;
using TMPro;
using UnityEngine;

public class WorldTextDetails : MonoBehaviour
{
    private TextMeshProUGUI textMesh;

    private Animator animator;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();

        animator = GetComponentInChildren<Animator>();
    }

    private IEnumerator WaitForSecondBeforClose()
    {
        animator.SetBool("Drop", true);

        yield return new WaitForSeconds(4);

        animator.SetBool("Drop", false);
    }

    public void ShowText(string text)
    {
        if (textMesh != null)
        {
            textMesh.text = text;

            StopAllCoroutines();

            StartCoroutine(WaitForSecondBeforClose());
        }
    }
}
