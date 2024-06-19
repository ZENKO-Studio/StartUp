using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    public string requiredKeyItemName; // Name of the required key item
    public Animator doorAnimator; // Reference to the Animator component
    public float interactionDistance = 4f; // Distance within which the player can interact with the door

    private bool isOpen = false;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!isOpen && Vector3.Distance(playerTransform.position, transform.position) <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                InventoryHandler inventory = InventoryHandler.Instance;
                InventoryItem keyItem = inventory.items.Find(item => item.itemName == requiredKeyItemName);
                if (keyItem != null)
                {
                    OpenDoor();
                    inventory.RemoveItem(keyItem); // Optionally remove the key item from the inventory
                }
            }
        }
    }

    private void OpenDoor()
    {
        isOpen = true;
        doorAnimator.SetTrigger("FoundKey"); // Assuming "FoundKey" is the trigger to play the open animation
    }
}
