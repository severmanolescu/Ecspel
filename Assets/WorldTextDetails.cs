using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        textMesh.text = text;

        StopAllCoroutines();

        StartCoroutine(WaitForSecondBeforClose());
    }
}
