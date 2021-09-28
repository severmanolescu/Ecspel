using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        dialogueText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        dialogueText.text = "";
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        bool stepOver = false;

        foreach (char letter in sentence.ToCharArray())
        {
            if(letter == '\t' || letter == ' ')
            {
                dialogueText.text += letter;

                continue;
            }
            else if(letter == '<')
            {
                stepOver = true;

                dialogueText.text += letter;

                continue;
            }
            else if(stepOver)
            {
                dialogueText.text += letter;

                if(letter == '>')
                {
                    stepOver = false;
                }

                continue;
            }
            
            dialogueText.text += letter;

            yield return new WaitForSeconds(DefaulData.dialogueSpeed);
        }
    }

    IEnumerator Wait(string sentence)
    {
        yield return new WaitForSeconds(3f);

        SetDialogue(sentence);
    }

    public void SetDialogue(string dialogue)
    {
        if(dialogue != null)
        {
            StopAllCoroutines();
            StartCoroutine(TypeSentence(dialogue));
        }
    }

    public void StopDialogue()
    {
        StopAllCoroutines();
    }
}
