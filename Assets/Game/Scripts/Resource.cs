using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Object
{
    public int amount;

    public override void Use()
    {
        // Implement specific use logic for resource items
        Debug.Log("Using resource item: " + itemName);
    }

    public override string GetDescription()
    {
        return $"This is a resource item: {itemName}\nAmount: {amount}";
    }
}

