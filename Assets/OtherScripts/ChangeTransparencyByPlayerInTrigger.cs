using UnityEngine;

public class ChangeTransparencyByPlayerInTrigger : MonoBehaviour
{
    [SerializeField] private float mTransparency = .5f;

    private SpriteRenderer treeCrown;
    private SpriteRenderer treeLeaves;

    private void Awake()
    {
        treeCrown = GetComponentsInChildren<SpriteRenderer>()[1];
        treeLeaves = GetComponentsInChildren<SpriteRenderer>()[2];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && treeCrown != null)
        {
            Color newColor = treeCrown.color;

            newColor.a = mTransparency;

            treeCrown.color = newColor;
            treeLeaves.color = newColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && treeCrown != null)
        {
            Color newColor = treeCrown.color;

            newColor.a = 1f;

            treeCrown.color = newColor;
            treeLeaves.color = newColor;
        }
    }
}
