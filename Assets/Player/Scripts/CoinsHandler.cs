using TMPro;
using UnityEngine;

public class CoinsHandler : MonoBehaviour
{
    public int amount;

    private TextMeshProUGUI amountText;

    public int Amount { get => amount; set { amount = value; ResetCoinAmount(); } }

    private void Awake()
    {
        amountText = GetComponentInChildren<TextMeshProUGUI>();

        amountText.text = amount.ToString();
    }

    private void ResetCoinAmount()
    {
        amountText.text = amount.ToString();
    }
}
