using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Audio effects")]
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip eatClip;

    private AudioSource audioSource;

    private float maxHealth;
    private float maxStamina;

    private Slider healthSlider;
    private Slider staminaSlider;

    private SpriteRenderer playerSprite;

    private SkillsHandler skillsHandler;

    public float Health { get { return healthSlider.value; } set { healthSlider.value = value; PlayDamageClip(); StartCoroutine(WaitDamage()); } }
    public float Stamina { get { return staminaSlider.value; } set { staminaSlider.value = value;} }

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

        audioSource = GetComponent<AudioSource>();

        skillsHandler = GameObject.Find("Global/Player/Canvas/Skills").GetComponent<SkillsHandler>();
    }

    private void Start()
    {
        healthSlider.maxValue = healthSlider.value  = maxHealth = DefaulData.maxPlayerHealth;
        staminaSlider.maxValue = staminaSlider.value = maxStamina = DefaulData.maxPlayerStamina;
    }

    public void TakeDamageEffect(float damage)
    {
        healthSlider.value -= damage;

        PlayDamageClip();
    }

    private void PlayDamageClip()
    {
        audioSource.clip = damageClip;
        audioSource.Play();
    }    

    public void SetToMaxStats()
    {
        healthSlider.value = healthSlider.maxValue;
        staminaSlider.value = staminaSlider.maxValue;
    }

    public void DecreseStamina(float stamina)
    {
        float skillStamina = skillsHandler.StaminaLevel * 0.025f * stamina;

        staminaSlider.value -= (stamina - skillStamina);
    }

    public bool Eat(Consumable consumable)
    {
        if(Health < healthSlider.maxValue || Stamina < staminaSlider.maxValue)
        {
            healthSlider.value  += consumable.Health;
            Stamina += consumable.Stamina;

            audioSource.clip = eatClip;
            audioSource.Play();

            return true;
        }

        return false;
    }

    public void ChangeHealthSkillLevel(int newLevel)
    {
        float toAddHealth = newLevel * 0.04f * DefaulData.maxPlayerHealth;

        healthSlider.maxValue = DefaulData.maxPlayerHealth + toAddHealth;
        healthSlider.value = healthSlider.maxValue;
    }
}
