using UnityEngine;

public class DeactivateCamera : MonoBehaviour
{
    [SerializeField] private new GameObject camera;

    public void Deactivate()
    {
        camera.SetActive(false);
    }
}
