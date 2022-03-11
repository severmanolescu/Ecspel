using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowSkillDetails : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [TextArea(3, 3)]
    [SerializeField] private string details;
    [SerializeField] private SkillsDetailsHandler detailsHandler;

    [SerializeField] private int skillTipe;

    private SkillsHandler skillHandler;

    private void Awake()
    {
        skillHandler = GetComponentInParent<SkillsHandler>();
    }

    public void UpdateDetails()
    {
        skillHandler.GetCoins(skillTipe, out int value, out int playerCoins);

        detailsHandler.SetDetails(details, value, playerCoins, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detailsHandler.gameObject.SetActive(true);

        UpdateDetails();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        detailsHandler.HideDetails();
    }
}
