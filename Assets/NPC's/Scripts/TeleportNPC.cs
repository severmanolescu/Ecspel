using UnityEngine;

public class TeleportNPC : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private bool teleportIn = true;

    public void Teleport(NpcBehavior toTeleportNPC)
    {
        if (teleportToPoint != null)
        {
            toTeleportNPC.transform.position = teleportToPoint.position;

            if(teleportIn)
            {
                toTeleportNPC.GetComponent<SpriteRenderer>().sortingLayerName = "Interior";
            }
            else
            {
                toTeleportNPC.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            }

            toTeleportNPC.GoToNextBehaviour();
        }
    }
}
