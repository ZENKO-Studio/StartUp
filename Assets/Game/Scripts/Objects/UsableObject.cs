using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @Sami 06/12/24
/// This is the class for any usable object like Gloves / Etc
/// </summary>

public class UsableObject : InventoryItem
{
    [Tooltip("Whether the item should be removed after use")]
    public bool bShouldBeRemoved = false;

    public override void Interact(CharacterBase player)
    {
        if(inventoryHandler != null) 
        {
            inventoryHandler.AddUsable(this);
        }
    }

    public virtual void Use()
    {
        // Implement general use logic for object items
        Debug.Log("Using object item: " + itemName);
    }
}
