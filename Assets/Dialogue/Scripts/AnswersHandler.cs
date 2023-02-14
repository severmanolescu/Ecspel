using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswersHandler : MonoBehaviour
{
    [SerializeField] private GameObject answerPrefab;

    private List<GameObject> answerList = new List<GameObject>();

    private bool placedAllAnswers = false;

    private DialogueScriptableObject dialogue;

    private DialogueChanger dialogueChanger;

    public DialogueChanger DialogueChanger { set => dialogueChanger = value; }

    private void DeleteAllAnswers()
    {
        StopAllCoroutines();

        foreach(GameObject answer in answerList)
        {
            Destroy(answer);
        }
    }

    public void ShowAnswers(DialogueScriptableObject dialogue)
    {
        DeleteAllAnswers();

        StopAllCoroutines();

        StartCoroutine(PlaceAnswers(dialogue));
    }

    private IEnumerator PlaceAnswers(DialogueScriptableObject dialogue)
    {
        this.dialogue = dialogue;

        if(dialogue != null && dialogue.DialogueAnswers != null)
        {
            placedAllAnswers = false;

            int buttonIndex = 0;

            foreach (DialogueAnswersClass answer in dialogue.DialogueAnswers)
            {
                if (answer.NextDialogue != null)
                {
                    Button button = Instantiate(answerPrefab).GetComponent<Button>();

                    button.transform.SetParent(transform);

                    button.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                    int auxiliarIndex = buttonIndex;

                    button.onClick.AddListener(delegate { ButtonPressed(auxiliarIndex); });

                    answerList.Add(button.gameObject);

                    if (answer.NextDialogue.Quest != null &&
                        answer.NextDialogue.Quest.Count > 0)
                    {
                        button.GetComponent<SetDataToAnsware>().SetDataToAnswer(answer.Answer, true);
                    }
                    else
                    {
                        button.GetComponent<SetDataToAnsware>().SetDataToAnswer(answer.Answer, false);
                    }

                    yield return new WaitForSeconds(0.5f);
                }

                buttonIndex++;
            }

            placedAllAnswers = true;
        }
    }

    public void ButtonPressed(int index)
    {
        if(placedAllAnswers)
        {
            dialogueChanger.ShowDialogue(dialogue.DialogueAnswers[index].NextDialogue);

            DeleteAllAnswers();
        }
    }
}
