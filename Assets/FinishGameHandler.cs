using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGameHandler : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    [SerializeField] private TransferImageHandler transferImageHandler;

    [SerializeField] private FinalTextHandler finalTextHandler;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
    }

    private void SetObject()
    {
        foreach (GameObject gameObject in objectsToSetActiveToFalse)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in objectsToSetActiveToTrue)
        {
            gameObject.SetActive(true);
        }
    }

    private IEnumerator WaitForBack(GameObject collision)
    {
        playerMovement.TabOpen = true;

        transferImageHandler.StartTransfer();

        yield return new WaitForSeconds(2f);

        SetObject();

        yield return new WaitForSeconds(0.5f);

        finalTextHandler.StartShowText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(WaitForBack(collision.gameObject));
        }
    }
}
