using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Sami 06/12/24
/// This is the class for any key for door 
/// </summary>

public class Key : UsableObject
{
    public int keyID = 0;   //To check with the door (Rohith preferes integers for it)

    public override void Interact(CharacterBase player)
    {
        inventoryHandler.AddKey(this);
    }
}
