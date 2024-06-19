using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Object : InventoryItem
{
 // Icon representing the object


    public override void Use()
    {
        // Implement general use logic for object items
        Debug.Log("Using object item: " + itemName);
    }
}
