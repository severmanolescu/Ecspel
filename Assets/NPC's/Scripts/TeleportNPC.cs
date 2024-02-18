using UnityEngine;

public class TeleportNPC : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    public void TeleportObject(NpcBehavior toTeleportObejct)
    {
        if (teleportToPoint != null)
        {
            toTeleportObejct.transform.position = teleportToPoint.position;

            toTeleportObejct.GoToNextBehaviour();
        }
    }
}
