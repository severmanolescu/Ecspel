using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUseByMouse : MonoBehaviour, IPointerExitHandler
{
    [SerializeField] private GameObject playerInventory;

    [SerializeField] private Button button;

    private Image image;

    private ItemSlot itemSlot = null;

    private void Awake()
    {
        button.gameObject.SetActive(false);

        image = GetComponent<Image>();

        image.enabled = false;
    }

    public void ShowButton(ItemSlot itemSlot)
    {
        transform.position = itemSlot.transform.position;

        this.itemSlot= itemSlot;

        button.gameObject.SetActive(true);

        image.enabled = true;
    }

    public void HideButton()
    {
        button.gameObject.SetActive(false);

        itemSlot = null;

        image.enabled = false;
    }

    public void ButtonPressed()
    {
        itemSlot.ItemUse();

        HideButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideButton();
    }

    private void Update()
    {
        if(playerInventory.activeSelf == false)
        {
            HideButton();
        }
    }
}
