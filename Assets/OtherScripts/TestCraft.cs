using UnityEngine;
using UnityEngine.InputSystem;

public class TestCraft : MonoBehaviour
{
    public float initialSize = 10f;
    public float initialSpeed = 20f;

    public float speed = 1f;

    private new Camera camera;

    private bool start = false;

    public void Awake()
    {
        camera = GetComponent<Camera>();
    }

    public void FixedUpdate()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            start = true;
        }

        if (start == true)
        {
            camera.orthographicSize -= speed * Time.deltaTime;
        }
    }

}
