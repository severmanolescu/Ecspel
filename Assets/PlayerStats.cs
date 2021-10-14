using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float maxHealth;
    private float maxStamina;

    private float health;
    private float stamina;

    public float Health { get { return health; } set { health = value; } }
    public float Stamina { get { return stamina; } set { stamina = value; } }

    private void Start()
    {
        health  = maxHealth = DefaulData.maxPlayerHealth;
        stamina = maxStamina = DefaulData.maxPlayerStamina;
    }
}
