using System.Collections;
using UnityEngine;

public class SetDialogueToPlayer : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject initialDialogue;

    private DialogueChanger dialogueChanger;

    private PlayerMovement playerMovement;

    private DialoguePlayerEnterInTrigger dialoguePlayerEnter;

    private void Awake()
    {
        dialogueChanger = GameObject.Find("Player/Canvas/Dialogue").GetComponent<DialogueChanger>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        StartCoroutine(WaitBeforShowDialogue());
    }

    private IEnumerator WaitBeforShowDialogue()
    {
        yield return new WaitForSeconds(1);

        if (initialDialogue != null)
        {
            SetDialogue(initialDialogue);
        }
    }

    public void SetDialogue(DialogueScriptableObject dialogue, DialoguePlayerEnterInTrigger dialoguePlayerEnter = null)
    {
        playerMovement.Dialogue = true;

        dialogueChanger.ShowDialogue(dialogue);

        this.dialoguePlayerEnter = dialoguePlayerEnter;
    }

    public void DialogueEnd()
    {
        playerMovement.Dialogue = false;

        if (dialoguePlayerEnter != null)
        {
            dialoguePlayerEnter.DialogueEnd();
        }
    }
}
