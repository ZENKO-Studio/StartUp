using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for all characters (Player and Enemy Controller will be based out of this one)
/// Contains Events for OnHealthChange and OnCharacterDied
/// Fields { IsPlayer, Health, CharacterAnimator }
/// Methods { IsPlayerCharacter(), TakeDamage() }
/// Events { OnHealthChanged, OnCharacterDied }
/// </summary>

[RequireComponent(typeof(Animator))]
public class CharacterBase : MonoBehaviour
{
    protected bool bIsPlayer;

    public bool IsPlayerCharacter() { return bIsPlayer; }

    [SerializeField]
    protected float health;

    protected float Health
    {
        get { return health; } 
        set
        {
            health = value;
            OnHealthChanged?.Invoke();
        }
    }

    //Can be used to Updated the HUD for player and if we have health bars for NPCs
    public UnityEvent OnHealthChanged = new UnityEvent();

    protected Animator characterAnimator;

    public UnityEvent OnCharacterDead = new UnityEvent();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        characterAnimator = GetComponent<Animator>();
    }

    //Can be called from any class to damage this perticular character
    public virtual void TakeDamage(float damage)
    {
        Debug.Log($"{gameObject.name} Taking Damage Health: {health}");
        Health -= damage;

        if(health < 0)
        {
            OnCharacterDead?.Invoke();
        }
    }

    public float GetHealth() { return health; }
}
