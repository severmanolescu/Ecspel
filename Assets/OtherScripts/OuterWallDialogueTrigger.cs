using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterWallDialogueTrigger : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues;

    private SetDialogueToPlayer setDialogueToPlayer;

    private void Awake()
    {
        setDialogueToPlayer = GameObject.Find("Global").GetComponent<SetDialogueToPlayer>();
    }

    private Dialogue GetRandomDialogue()
    {
        if(dialogues != null && dialogues.Count > 0)
        {
            return dialogues[Random.Range(0, dialogues.Count - 1)];
        }

        return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            setDialogueToPlayer.SetDialogue(GetRandomDialogue());
        }
    }
}
