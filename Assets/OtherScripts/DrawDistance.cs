using UnityEngine;

public class DrawDistance : MonoBehaviour
{
    private GameObject mainGameObject = null;

    PositionInGrid positionInGrid = null;

    private void Start()
    {
        positionInGrid = GetComponentInChildren<PositionInGrid>();

        Transform[] objects = GetComponentsInChildren<Transform>();

        if(objects != null && objects.Length >= 2)
        {
            mainGameObject = objects[1].gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.gameObject.CompareTag("DrawDistance"))
        {
            if (mainGameObject != null)
            {
                mainGameObject.SetActive(true);

                if (positionInGrid != null)
                {
                    positionInGrid.InDrawDistance = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("DrawDistance"))
        {
            if(mainGameObject != null)
            {
                mainGameObject.SetActive(false);
            }
        }
    }
}
