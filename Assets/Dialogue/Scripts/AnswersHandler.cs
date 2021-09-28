using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswersHandler : MonoBehaviour
{
    [SerializeField] private GameObject answerPrefab; 

    private List<DialogueanswersClass> dialogueAnswers;

    private List<GameObject> answers = new List<GameObject>();

    private DialogueChanger dialogueChanger;

    public void SetDialogueChanger(DialogueChanger dialogueChanger)
    {
        this.dialogueChanger = dialogueChanger;
    }

    private void PlaceAnswers()
    {
        foreach(DialogueanswersClass answer in dialogueAnswers)
        {
            GameObject ansferInstance = Instantiate(answerPrefab);

            ansferInstance.transform.SetParent(gameObject.transform);
            ansferInstance.transform.localScale = answerPrefab.transform.localScale;

            ansferInstance.GetComponentInChildren<TextMeshProUGUI>().text = answer.GetAnswer();

            if(answer.GetNextDialogue() == null)
            {
                ansferInstance.GetComponent<Button>().onClick.AddListener(DeleteAll);
            }
            else
            {
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { dialogueChanger.SetDialogue(answer.GetNextDialogue()); });
            }

            answers.Add(ansferInstance);
        }
    }

    public void DeleteAll()
    {
        foreach (GameObject answer in answers)
        {
            Destroy(answer);
        }

        answers.Clear();
    }

    public void SetAnswers(List<DialogueanswersClass> dialogueAnswers)
    {
        if (dialogueAnswers != null)
        {
            DeleteAll();

            this.dialogueAnswers = dialogueAnswers;

            PlaceAnswers();
        }
    }
}
