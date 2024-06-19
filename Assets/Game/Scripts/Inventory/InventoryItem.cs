using Game.Scripts.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryItem : MonoBehaviour
{
    public string ItemId;
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPreview;
    public int btnIndex;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void Interact()
    {
        if (gameManager != null)
        {
            if (CompareTag("KeyItem"))
            {
                gameManager.AddKeyItem(gameObject.name);
            }
            else if (CompareTag("ResourceItem"))
            {
                gameManager.AddResourceItem(gameObject.name, 1); // Adjust amount as needed
            }

            Destroy(gameObject); // Remove the item from the scene
        }
    }

    public abstract void Use();
    public abstract string GetDescription();
}
