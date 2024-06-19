using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Document : InventoryItem
{
    public override void Interact(CharacterBase player)
    {
        inventoryHandler.AddDocument(this);
    }
}
