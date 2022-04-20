using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    private TextMeshProUGUI dialogueText;

    private AudioSource audioSource;

    private string sentace;

    private void Awake()
    {
        dialogueText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        audioSource = GetComponent<AudioSource>(); 
    }

    private void Start()
    {
        dialogueText.text = "";
    }

    IEnumerator TypeSentence(string sentence)
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        dialogueText.text = "";

        bool stepOver = false;

        audioSource.Play();

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

        audioSource.Stop();
    }

    public void SetDialogue(string dialogue)
    {
        if(dialogue != null)
        {
            sentace = dialogue;

            StopAllCoroutines();

            gameObject.SetActive(true);

            StartCoroutine(TypeSentence(dialogue));
        }
    }

    public void ShowAllText()
    {
        StopAllCoroutines();

        audioSource.Stop();

        dialogueText.text = sentace;
    }

    public void StopDialogue()
    {
        StopAllCoroutines();

        audioSource.Stop();
    }
}
