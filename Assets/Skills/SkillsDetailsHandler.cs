using UnityEngine;
using TMPro;

public class SkillsDetailsHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coins;

    private TextMeshProUGUI details;

    private ShowSkillDetails showSkillDetails;

    private void Awake()
    {
        details = GetComponentInChildren<TextMeshProUGUI>();

        details.text = string.Empty;

        gameObject.SetActive(false);
    }

    public void SetDetails(string details, int coins, int playerCoins, ShowSkillDetails showSkillDetails)
    {
        this.showSkillDetails = showSkillDetails;

        if(details.CompareTo(string.Empty) != 0)
        {
            this.details.text = details;

            this.coins.text = coins.ToString();

            if(coins <= playerCoins)
            {
                this.coins.color = Color.white;
            }
            else
            {
                this.coins.color = Color.red;
            }
        }
    }

    public void HideDetails()
    {
        coins.text = string.Empty;
        details.text = string.Empty;

        gameObject.SetActive(false);
    }

    public void UpdateDetials()
    {
        if (showSkillDetails != null)
        {
            showSkillDetails.UpdateDetails();
        }
    }
}
