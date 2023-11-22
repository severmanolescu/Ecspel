using UnityEngine;

public class LoadGamePositionNPC : MonoBehaviour
{
    [SerializeField] private Vector3 position;

    public void LoadGame()
    {
        transform.localPosition = position;
    }
}
