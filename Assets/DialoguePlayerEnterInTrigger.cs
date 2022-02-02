using UnityEngine;

public class DialoguePlayerEnterInTrigger : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogue;

    [SerializeField] private bool startAnimator = false;

    [SerializeField] private Transform teleportPlayerToPoint;

    private SetDialogueToPlayer setDialogueToPlayer;

    private Animator animator;

    private bool canStartDialogue = true;

    private void Awake()
    {
        setDialogueToPlayer = GameObject.Find("Global").GetComponent<SetDialogueToPlayer>();

        if(startAnimator)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (setDialogueToPlayer != null && dialogue != null && canStartDialogue)
        {
            setDialogueToPlayer.SetDialogue(dialogue, this);

            if (startAnimator == false)
            {
                DestroyObject();
            }
            else
            {

                canStartDialogue = false;

            }
        }
    }

    public void DialogueEnd()
    {
        animator.SetTrigger("Start");
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void AnimationEnd()
    {
        GameObject.Find("PlayerHouse").GetComponent<TeleportPlayerToHouse>().Teleport();

        DestroyObject();
    }
}
