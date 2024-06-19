using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// @Sami
/// Base class for all characters (Player and Enemy Controller will be based out of this one)
/// Contains Events for OnHealthChange, OnStaminaChanged and OnCharacterDied
/// Fields { IsPlayer, Health, Stamina CharacterAnimator }
/// Methods { IsPlayerCharacter(), TakeDamage(), DepleteStamina }
/// Events { OnHealthChanged, OnStaminaChanged, OnCharacterDied }
/// </summary>

[RequireComponent(typeof(Animator))]
public class CharacterBase : MonoBehaviour
{
    protected bool bIsPlayer;

    public bool IsPlayerCharacter() { return bIsPlayer; }

    [Header("Character Health and Stamina")]
    [SerializeField]
    protected float health;

    [SerializeField]
    protected float stamina;

    [SerializeField]
    protected float staminaDepletionRate = 1f;

    [SerializeField]
    protected float staminaGenRate = 1f;

    protected float Health
    {
        get { return health; } 
        set
        {
            health = value;
            Debug.Log($"{gameObject.name} Taking Damage Health: {health}");
            OnHealthChanged?.Invoke();
        }
    }

    protected float Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;
            Debug.Log($"{gameObject.name} Stamina Changing: {stamina}");
            OnStaminaChanged?.Invoke();
        }
    }

    //Can be used to Updated the HUD for player and if we have health bars for NPCs
    public UnityEvent OnHealthChanged = new UnityEvent();
    
    //Can be used to Updated the HUD for player and if we have health bars for NPCs
    public UnityEvent OnStaminaChanged = new UnityEvent();

    protected Animator characterAnimator;

    //Can be used to trigger anything on Enemy Death (just a possibility)
    public UnityEvent OnCharacterDead = new UnityEvent();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

    //Can be called from any class to damage this particular character
    public virtual void TakeDamage(float damage)
    {
        Health -= damage;

        if(health < 0)
        {
            OnCharacterDead?.Invoke();
        }
    }

    //Can be called from any class to damage this particular character
    public virtual void DepleteStamina()
    {
        Stamina -= staminaDepletionRate * Time.deltaTime;
    }

    //Can be called from any class to damage this particular character
    public virtual void GenerateStamina()
    {
        Stamina += staminaGenRate * Time.deltaTime;
    }

    public float GetHealth() { return health; }
    
    public float GetStamina() { return stamina; }
}
