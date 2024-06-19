using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NormalItem : Object
{
    public override void Use()
    {
        // Implement specific use logic for key items
        Debug.Log("Using normal item: " + itemName);
    }

    public override string GetDescription()
    {
        return "This is a normal item: " + itemName;
    }
}
