using System.Collections.Generic;
using UnityEngine;

public class MenuSceneCameraHandler : MonoBehaviour
{
    [SerializeField] private List<Camera> cameras = new List<Camera>();

    [SerializeField] private float cameraSizeSpeed = 0.1f;

    [SerializeField] private float initialSize = 3.5f;
    [SerializeField] private float finalSize = 8f;

    private Camera currentCamera = null;

    private void Start()
    {
        foreach (Camera camera in cameras)
        {
            camera.gameObject.SetActive(false);
        }
    }

    private void ChangeCamera()
    {
        Camera newCamera = cameras[Random.Range(0, cameras.Count - 1)];

        while (newCamera == currentCamera)
        {
            newCamera = cameras[Random.Range(0, cameras.Count - 1)];
        }

        currentCamera = newCamera;
        currentCamera.orthographicSize = initialSize;
        currentCamera.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (currentCamera != null && currentCamera.orthographicSize <= finalSize)
        {
            currentCamera.orthographicSize += cameraSizeSpeed * Time.deltaTime;
        }
        else
        {
            if (currentCamera == null)
            {
                ChangeCamera();
            }
            else
            {
                currentCamera.gameObject.SetActive(false);

                ChangeCamera();
            }
        }
    }
}
