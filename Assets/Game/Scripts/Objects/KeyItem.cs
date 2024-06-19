using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyItem : Object
{
    public override void Use()
    {
        // Implement specific use logic for key items
        Debug.Log("Using key item: " + itemName);
    }

    public override string GetDescription()
    {
        return "This is a key item: " + itemName;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                Inventory.Instance.AddItem(this);
                gameObject.SetActive(false);
            }
        }
    }
}

