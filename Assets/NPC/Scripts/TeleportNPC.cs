using UnityEngine;

public class TeleportNPC : MonoBehaviour
{
    [SerializeField] private bool footsteps;

    private Transform teleportToPoint;

    private void Awake()
    {
        TeleportPlayer teleportPlayer = GetComponent<TeleportPlayer>();

        if (teleportPlayer != null)
        {
            teleportToPoint = teleportPlayer.TeleportToPoint;
        }
        else
        {
            TeleportPlayerKeyPress teleportPlayerKey = GetComponent<TeleportPlayerKeyPress>();

            if (teleportPlayerKey != null)
            {
                teleportToPoint = teleportPlayerKey.TeleportToPoint;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            collision.transform.position = teleportToPoint.position;

            NpcAIHandler npcAIHandler = collision.GetComponent<NpcAIHandler>();

            if (npcAIHandler != null)
            {

                FootPrintHandler footPrintHandler = npcAIHandler.GetComponent<FootPrintHandler>();

                if (footPrintHandler != null)
                {
                    footPrintHandler.ChangeFootprintSpawnLocationState(footsteps);
                }
            }
        }
    }
}
