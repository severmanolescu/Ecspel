using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private float maxHealth;
    private float maxStamina;

    private Slider healthSlider;
    private Slider staminaSlider;

    private SpriteRenderer playerSprite;

    public float Health { get { return healthSlider.value; } set { healthSlider.value = value; StartCoroutine(WaitDamage()); } }
    public float Stamina { get { return staminaSlider.value; } set { staminaSlider.value = value; } }

    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float MaxStamina { get { return maxStamina; } set { maxStamina = value; } }

    IEnumerator WaitDamage()
    {
        playerSprite.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        playerSprite.color = Color.white;
    }

    private void Awake()
    {
        Slider[] slider = GetComponentsInChildren<Slider>();

        healthSlider = slider[0];
        staminaSlider = slider[1];

        playerSprite = GetComponentInParent<SpriteRenderer>();
    }

    private void Start()
    {
        healthSlider.maxValue = healthSlider.value  = maxHealth = DefaulData.maxPlayerHealth;
        staminaSlider.maxValue = staminaSlider.value = maxStamina = DefaulData.maxPlayerStamina;
    }
}
