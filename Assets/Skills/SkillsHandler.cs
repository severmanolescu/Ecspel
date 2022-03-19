using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillsHandler : MonoBehaviour
{
    [SerializeField] private SkillPowerHandler powerSkill;
    [SerializeField] private SkillPowerHandler attackSkill;
    [SerializeField] private SkillPowerHandler farmingSkill;
    [SerializeField] private SkillPowerHandler staminaSkill;
    [SerializeField] private SkillPowerHandler healthSkill;
    [SerializeField] private SkillPowerHandler luckSkill;

    [SerializeField] private GameObject powerSkillButton;
    [SerializeField] private GameObject attackSkillButton;
    [SerializeField] private GameObject farmingSkillButton;
    [SerializeField] private GameObject staminaSkillButton;
    [SerializeField] private GameObject healthSkillButton;
    [SerializeField] private GameObject luckSkillButton;

    [SerializeField] private TextMeshProUGUI coins;

    private CoinsHandler coinsHandler;

    private PlayerStats playerStats;

    [SerializeField] private SkillsDetailsHandler skillsDetails;

    private int powerLevel;
    private int attackLevel;
    private int farmingLevel;
    private int staminaLevel;
    private int healthLevel;
    private int luckLevel;

    public int PowerLevel { get => powerLevel; set => ChangePowerLevel(value); }
    public int AttackLevel { get => attackLevel; set => ChangeAttackLevel(value); }
    public int FarmingLevel { get => farmingLevel; set => ChangeFarmingLevel(value); }
    public int StaminaLevel { get => staminaLevel; set => ChangeStaminaLevel(value); }
    public int HealthLevel { get => healthLevel; set => ChangeHealthLevel(value); }
    public int LuckLevel { get => luckLevel; set => ChangeLuckLevel(value); }

    private void Awake()
    {
        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();
        coinsHandler = GameObject.Find("Global/Player/Canvas/PlayerItems/Coins").GetComponent<CoinsHandler>();

        gameObject.SetActive(false);
    }

    public void ShowSkills()
    {
        coins.text = coinsHandler.Amount.ToString();
    }

    public void GetCoins(int skill, out int value, out int playerCoins)
    {
        switch (skill)
        {
            case 0: value = (powerLevel + 1) * 50; break;
            case 1: value = (attackLevel + 1) * 50; break;
            case 2: value = (farmingLevel + 1) * 50; break;
            case 3: value = (staminaLevel + 1) * 50; break;
            case 4: value = (healthLevel + 1) * 50; break;
            case 5: value = (luckLevel + 1) * 50; break;
            default: value = 0; break;
        }

        playerCoins = coinsHandler.Amount;
    }

    public void IncresePowerLevel()
    {
        int skillValue = (powerLevel + 1) * 50;

        if (powerLevel < 10 && coinsHandler.Amount >= skillValue)
        {
            ChangePowerLevel(PowerLevel + 1, skillValue);            
        }
    }
    public void IncreseAttackLevel()
    {
        int skillValue = (attackLevel + 1) * 50;

        if (attackLevel < 10 && coinsHandler.Amount >= skillValue)
        {
            ChangeAttackLevel(AttackLevel + 1, skillValue);
        }
    }
    public void IncreseFarmingLevel()
    {
        int skillValue = (farmingLevel + 1) * 50;

        if (farmingLevel < 10 && coinsHandler.Amount >= skillValue)
        {
            ChangeFarmingLevel(FarmingLevel + 1, skillValue);
        }
    }
    public void IncreseStaminaLevel()
    {
        int skillValue = (staminaLevel + 1) * 50;

        if (staminaLevel < 10 && coinsHandler.Amount >= skillValue)
        {            
            ChangeStaminaLevel(StaminaLevel + 1, skillValue);
        }
    }
    public void IncreseHealthLevel()
    {
        int skillValue = (healthLevel + 1) * 50;

        if (healthLevel < 10 && coinsHandler.Amount >= skillValue)
        {            
            ChangeHealthLevel(HealthLevel + 1, skillValue);
        }
    }
    public void IncreseLuckLevel()
    {
        int skillValue = (luckLevel + 1) * 50;

        if (luckLevel < 10 && coinsHandler.Amount >= skillValue)
        {            
            ChangeLuckLevel(luckLevel + 1, skillValue);
        }
    }

    private void ChangePowerLevel(int level, int skillPrice = 0)
    {
        powerLevel = level;

        powerSkill.ChangeLevel(level);

        skillsDetails.UpdateDetials();

        coinsHandler.Amount -= skillPrice;

        ShowSkills();

        if (powerLevel >= 10)
        {
            if (powerSkillButton != null)
            {
                Destroy(powerSkillButton);
            }

            skillsDetails.HideDetails();
        }
    }
    private void ChangeAttackLevel(int level, int skillPrice = 0)
    {
        attackLevel = level;

        attackSkill.ChangeLevel(level);

        skillsDetails.UpdateDetials();

        coinsHandler.Amount -= skillPrice;

        ShowSkills();

        if (attackLevel >= 10)
        {
            if (attackSkillButton != null)
            {
                Destroy(attackSkillButton);
            }

            skillsDetails.HideDetails();
        }
    }
    private void ChangeFarmingLevel(int level, int skillPrice = 0)
    {
        farmingLevel = level;

        farmingSkill.ChangeLevel(level);

        skillsDetails.UpdateDetials();

        coinsHandler.Amount -= skillPrice;

        ShowSkills();

        if (farmingLevel >= 10)
        {
            if (farmingSkillButton != null)
            {
                Destroy(farmingSkillButton);
            }

            skillsDetails.HideDetails();
        }
    }
    private void ChangeStaminaLevel(int level, int skillPrice = 0)
    {
        staminaLevel = level;

        staminaSkill.ChangeLevel(level);

        skillsDetails.UpdateDetials();

        coinsHandler.Amount -= skillPrice;

        ShowSkills();

        if (staminaLevel >= 10)
        {
            if (staminaSkillButton != null)
            {
                Destroy(staminaSkillButton);
            }

            skillsDetails.HideDetails();
        }
    }
    private void ChangeHealthLevel(int level, int skillPrice = 0)
    {
        healthLevel = level;
        healthSkill.ChangeLevel(level);

        skillsDetails.UpdateDetials();

        playerStats.ChangeHealthSkillLevel(healthLevel);

        coinsHandler.Amount -= skillPrice;

        ShowSkills();

        if (healthLevel >= 10)
        {
            if (healthSkillButton != null)
            {
                Destroy(healthSkillButton);
            }

            skillsDetails.HideDetails();
        }
    }
    private void ChangeLuckLevel(int level, int skillPrice = 0)
    {
        luckLevel = level;
        luckSkill.ChangeLevel(level);

        skillsDetails.UpdateDetials();

        coinsHandler.Amount -= skillPrice;

        ShowSkills();

        if (luckLevel >= 10)
        {
            if (luckSkillButton != null)
            {
                Destroy(luckSkillButton);
            }

            skillsDetails.HideDetails();
        }
    }

    public List<int> GetAllSkillsLevels()
    {
        List<int> list = new List<int>();

        list.Add(powerLevel);
        list.Add(attackLevel);
        list.Add(farmingLevel);
        list.Add(staminaLevel);
        list.Add(healthLevel);
        list.Add(LuckLevel);

        return list;
    }

    public void SetSkillsLevels(List<int> list)
    {
        if(list != null && list.Count == 6)
        {
            ChangePowerLevel(list[0]);
            ChangeAttackLevel(list[1]);
            ChangeFarmingLevel(list[2]);
            ChangeStaminaLevel(list[3]);
            ChangeHealthLevel(list[4]);
            ChangeLuckLevel(list[5]);
        }
    }
}
