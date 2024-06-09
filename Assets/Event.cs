using UnityEngine;

public class Event : MonoBehaviour
{
    [SerializeField] protected bool canTrigger;

    public bool CanTrigger { get => canTrigger; set => canTrigger = value; }
}
