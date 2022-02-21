using UnityEngine;

public class DialoguePlayerEnterInTrigger : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogue;

    [SerializeField] private bool startAnimator = false;

    [SerializeField] private Transform teleportPlayerToPoint;

    [Header("-1 - current idle dirrection\n" +
            " 0 - left\n" +
            " 1 - right\n" +
            " 2 - up\n" +
            " 3 - down\n")]
    [SerializeField] private int idleAnimationDirection = -1;

    [SerializeField] private int dialogueId;

    private SetDialogueToPlayer setDialogueToPlayer;

    private Animator animator;

    private PlayerMovement playerMovement;

    private bool canStartDialogue = true;

    public DialogueScriptableObject Dialogue { get => dialogue; set => dialogue = value; }
    public bool StartAnimator { get => startAnimator; set => startAnimator = value; }
    public Transform TeleportPlayerToPoint { get => teleportPlayerToPoint; set => teleportPlayerToPoint = value; }
    public int IdleAnimationDirection { get => idleAnimationDirection; set => idleAnimationDirection = value; }
    public int DialogueId { get => dialogueId; set => dialogueId = value; }

    private void Awake()
    {
        setDialogueToPlayer = GameObject.Find("Global").GetComponent<SetDialogueToPlayer>();

        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        if(StartAnimator)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (setDialogueToPlayer != null && Dialogue != null && canStartDialogue)
        {
            setDialogueToPlayer.SetDialogue(Dialogue, this);

            playerMovement.ChangeIdleAnimationDirection(idleAnimationDirection);

            if (StartAnimator == false)
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
