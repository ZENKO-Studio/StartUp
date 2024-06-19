using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Inventory inventory;
    public GameObject keyItemPrefab;
    public GameObject resourceItemPrefab;
    public NellAttributes playerRef;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persist across scene loads
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
    }

    private void Start()
    {
        // Add some items to the inventory for testing
        AddKeyItem("Golden Key");
        // AddResourceItem("Salt", 10);
    }

    public void AddKeyItem(string name)
    {
        GameObject keyItemObject = Instantiate(keyItemPrefab);
        KeyItem keyItem = keyItemObject.GetComponent<KeyItem>();
        keyItem.itemName = name;
        inventory.AddItem(keyItem);
    }

    public void AddResourceItem(string name, int amount)
    {
        GameObject resourceItemObject = Instantiate(resourceItemPrefab);
        Resource resourceItem = resourceItemObject.GetComponent<Resource>();
        resourceItem.itemName = name;
        resourceItem.amount = amount;
        inventory.AddItem(resourceItem);
    }

    public void ToggleInventory(bool isOpen)
    {
        EventBus.Publish(new EventBus.ToggleInventoryEvent(isOpen));
    }
}
