using System.Collections.Generic;
using UnityEngine;

public class DialoguePlayerEnterInTrigger : Event
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private bool startAnimator = false;

    [SerializeField] private Transform teleportPlayerToPoint;

    [Header("-1 - current idle dirrection\n" +
            " 0 - left\n" +
            " 1 - right\n" +
            " 2 - up\n" +
            " 3 - down\n")]
    [Range(-1, 3)]
    [SerializeField] private int idleAnimationDirection = -1;

    [SerializeField] private List<GameObject> toDestroyObjects = new();

    private int dialogueId = -1;

    private SetDialogueToPlayer setDialogueToPlayer;

    private Animator animator;

    private PlayerMovement playerMovement;

    private bool canStartDialogue = true;

    public Dialogue Dialogue { get => dialogue; set => dialogue = value; }
    public bool StartAnimator { get => startAnimator; set => startAnimator = value; }
    public Transform TeleportPlayerToPoint { get => teleportPlayerToPoint; set => teleportPlayerToPoint = value; }
    public int IdleAnimationDirection { get => idleAnimationDirection; set => idleAnimationDirection = value; }
    public int DialogueId { get => dialogueId; set => dialogueId = value; }

    private void Awake()
    {
        setDialogueToPlayer = GameObject.Find("Global").GetComponent<SetDialogueToPlayer>();

        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        //if (dialogueId == -1)
        //{
        //    dialogueId = GameObject.Find("Global").GetComponent<GetObjectReference>().GetObjectId(gameObject);
        //}

        if (StartAnimator)
        {
            animator = GetComponent<Animator>();
        }
    }

    private void DestroyObjects()
    {
        foreach (GameObject @object in toDestroyObjects)
        {
            if (@object != null)
            {
                Destroy(@object);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")  &&
            setDialogueToPlayer != null     &&
            Dialogue != null                && 
            canStartDialogue                &&
            canTrigger)
        {
            setDialogueToPlayer.SetDialogue(Dialogue, this);

            playerMovement.ChangeIdleAnimationDirection(idleAnimationDirection);

            StartWalkToNPC startWalk = GetComponent<StartWalkToNPC>();

            if (StartAnimator == false && startWalk == null)
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
        if (startAnimator == true)
        {
            animator.SetTrigger("Start");

            playerMovement.TabOpen = true;
        }
        else
        {
            playerMovement.TabOpen = false;
        }

        DestroyObjects();
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void AnimationEnd()
    {
        GameObject.Find("PlayerHouse").GetComponent<TeleportPlayerToHouse>().Teleport();

        DestroyObject();

        playerMovement.TabOpen = false;
    }
}
