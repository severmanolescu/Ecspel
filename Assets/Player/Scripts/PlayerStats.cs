using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Audio effects")]
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip eatClip;

    [Header("Animator for health and stamina")]
    [SerializeField] private Animator animatorHealth;
    [SerializeField] private Animator animatorStamina;

    [SerializeField] private float initialHealth;
    [SerializeField] private float initialStamina;

    private AudioSource audioSource;

    private float maxHealth;
    private float maxStamina;

    private Slider healthSlider;
    private Slider staminaSlider;

    private SpriteRenderer playerSprite;

    private SkillsHandler skillsHandler;

    private Animator animator;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen canvasTabsOpen;

    private DayTimerHandler dayTimer;

    private DieHandler dieHandler;

    private bool alreadyRunOutStamina = false;
    private bool alreadyDead = false;

    private float lastHP = 0;

    public float Health
    {
        get { return healthSlider.value; }
        set
        {
            lastHP = healthSlider.value;
            healthSlider.value = value;

            if (Health < lastHP)
            {
                PlayDamageClip();
                StartCoroutine(WaitDamage());
            }

            SetAnimatorHealth();
            CheckHealth();
        }
    }

    public float Stamina { get { return staminaSlider.value; } set { staminaSlider.value = value; SetAnimatorStamina(); CheckForStamina(); } }

    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float MaxStamina { get { return maxStamina; } set { maxStamina = value; } }

    public DieHandler DieHandler { set => dieHandler = value; }

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

        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        canvasTabsOpen = GetComponentInChildren<CanvasTabsOpen>();

        dayTimer = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    private void Start()
    {
        healthSlider.maxValue = Health = maxHealth = initialHealth;
        staminaSlider.maxValue = Stamina = maxStamina = initialStamina;

        SetToMaxStats();
    }

    private IEnumerator WaitForAnimationStamina(int duration, bool startOfAnimation)
    {
        yield return new WaitForSeconds(duration);

        if (startOfAnimation == true)
        {
            dayTimer.Sleep(true);

            StartCoroutine(WaitForAnimationStamina(1, false));
        }
        else
        {
            alreadyRunOutStamina = false;

            animator.SetBool("OutOfStamina", false);

            playerMovement.TabOpen = false;

            canvasTabsOpen.canOpenTabs = true;
        }
    }

    private void OutOfStamina()
    {
        alreadyRunOutStamina = true;

        animator.SetBool("OutOfStamina", true);
        animator.SetTrigger("Stamina");

        playerMovement.TabOpen = true;

        canvasTabsOpen.canOpenTabs = false;

        StartCoroutine(WaitForAnimationStamina(2, true));
    }

    private void CheckForStamina()
    {
        if (Stamina <= 0 && alreadyRunOutStamina == false)
        {
            OutOfStamina();
        }
    }

    private IEnumerator WaitForAnimationHealth(int duration)
    {
        yield return new WaitForSeconds(duration);

        dieHandler.Die();
    }

    public void Revive()
    {
        alreadyDead = false;

        animator.SetBool("OutOfHealth", false);

        playerMovement.TabOpen = false;

        canvasTabsOpen.canOpenTabs = true;
    }

    private void OutOfHealth()
    {
        alreadyDead = true;

        animator.SetBool("OutOfHealth", true);
        animator.SetTrigger("Health");

        playerMovement.TabOpen = true;

        canvasTabsOpen.canOpenTabs = false;

        StartCoroutine(WaitForAnimationHealth(2));
    }

    private void CheckHealth()
    {
        if (Health <= 0 && alreadyDead == false)
        {
            OutOfHealth();
        }
    }

    private void SetAnimatorHealth()
    {
        //if (Health >= healthSlider.maxValue / 2)
        //{
        //    animatorHealth.SetInteger("Speed", 0);
        //}
        //else if (Health >= healthSlider.maxValue / 3)
        //{
        //    animatorHealth.SetInteger("Speed", 1);
        //}
        //else if (Health >= healthSlider.maxValue / 4)
        //{
        //    animatorHealth.SetInteger("Speed", 2);
        //}
        //else
        //{
        //    animatorHealth.SetInteger("Speed", 3);
        //}
    }

    private void SetAnimatorStamina()
    {
        if (Stamina >= staminaSlider.maxValue / 2)
        {
            //animatorStamina.SetInteger("Speed", 0);
        }
        else if (Stamina >= staminaSlider.maxValue / 3)
        {
            //animatorStamina.SetInteger("Speed", 1);
        }
        else if (Stamina >= staminaSlider.maxValue / 4)
        {
            //animatorStamina.SetInteger("Speed", 2);
        }
        else
        {
            //animatorStamina.SetInteger("Speed", 3);
        }
    }

    public void TakeDamageEffect(float damage)
    {
        Health -= damage;
    }

    private void PlayDamageClip()
    {
        audioSource.clip = damageClip;
        audioSource.Play();
    }

    public void SetToMaxStats()
    {
        Health = healthSlider.maxValue;
        Stamina = staminaSlider.maxValue;
    }

    public void DecreseStamina(float stamina)
    {
        float skillStamina = skillsHandler.StaminaLevel * 0.025f * stamina;

        Stamina -= (stamina - skillStamina);
    }

    public bool Eat(Consumable consumable)
    {
        if (Health < healthSlider.maxValue || Stamina < staminaSlider.maxValue)
        {
            Health += consumable.Health;
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
        Health = healthSlider.maxValue;
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                Health -= 10f;
            }
            else if (Keyboard.current.iKey.wasPressedThisFrame)
            {
                Stamina -= 10f;
            }
        }
    }
}
