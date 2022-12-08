using UnityEngine;

public class TipShow : MonoBehaviour
{
    [SerializeField] private Tip tip;

    public Tip Tip { get => tip; set => tip = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<StartTip>().ShowTip(Tip.tipDetails);

                Destroy(gameObject);
            }
        }
    }
}
