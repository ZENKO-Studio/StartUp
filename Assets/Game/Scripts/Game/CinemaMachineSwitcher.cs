using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CinemaMachineSwitcher : MonoBehaviour
{

    [Header("Callbacks")]
    public UnityEvent onEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterBase player))
        {
            OnEnter();
        }
    }

    private void OnEnter()
    {
        onEnter.Invoke();
    }

}

