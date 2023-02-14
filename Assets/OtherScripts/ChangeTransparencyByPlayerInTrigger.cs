using UnityEngine;

public class ChangeTransparencyByPlayerInTrigger : MonoBehaviour
{
    [SerializeField] private float mTransparency = .5f;

    private SpriteRenderer treeCrown;

    private void Awake()
    {
        treeCrown = GetComponentsInChildren<SpriteRenderer>()[1];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && treeCrown != null)
        {
            Color newColor = treeCrown.color;

            newColor.a = mTransparency;

            treeCrown.color = newColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && treeCrown != null)
        {
            Color newColor = treeCrown.color;

            newColor.a = 1f;

            treeCrown.color = newColor;
        }
    }
}
