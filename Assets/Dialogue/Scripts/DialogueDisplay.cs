using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogue;
    [SerializeField] private Animator questMarkAniamtor;

    private DialogueChanger dialogueChanger;

    private NpcPathFinding npcPathFinding;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen canvasTabs;

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        dialogueChanger = GameObject.Find("Global/Player/Canvas/Dialogue").GetComponent<DialogueChanger>();
        canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        npcPathFinding = GetComponent<NpcPathFinding>();
    }

    private void StopWalk()
    {
        npcPathFinding.Talking = true;
    }

    private void StartWalk()
    {
        npcPathFinding.Talking = false;

        playerMovement.Dialogue = false;
    }

    public void FinishTalk()
    {
        StartWalk();

        canvasTabs.ShowDefaultUIElements();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.isTrigger == false)
        {
            StartWalk();

            dialogueChanger.StopDialogue();
        }
    }

    public void ShowDialogue()
    {
        dialogueChanger.ShowDialogue(dialogue, this);

        playerMovement.Dialogue = true;

        StopWalk();

        npcPathFinding.SetAnimatorDirectionToPlayer(playerMovement.transform.position);

        canvasTabs.PrepareUIForDialogue();
    }
}
