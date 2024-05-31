using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EventBus;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public Transform keyItemContainer;
    public Transform resourceItemContainer;
    public Transform contentTransform; // Reference to the ScrollView content
    public GameObject itemButtonPrefab; // Reference to the ItemButton prefab

    // References to the inspection UI elements
    public TMP_Text itemNameText;
    public Image itemIconImage;
    public TMP_Text itemDescriptionText;
    public Transform itemPreviewParent;
    public Transform boardContainer;

    public Camera newCamera; // Reference to the new camera

    private void OnEnable()
    {
        EventBus.Subscribe<ItemInspectedEvent>(OnItemInspected);
        EventBus.Subscribe<ItemAddedEvent>(OnItemAdded);
        EventBus.Subscribe<ItemRemovedEvent>(OnItemRemoved);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ItemInspectedEvent>(OnItemInspected);
        EventBus.Unsubscribe<ItemAddedEvent>(OnItemAdded);
        EventBus.Unsubscribe<ItemRemovedEvent>(OnItemRemoved);
    }

    private void Start()
    {
        // Initialize the inventory with existing items
        foreach (var item in items)
        {
            CreateItemButton(item);
        }

        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = newCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                KeyItem keyItem = hit.collider.GetComponent<KeyItem>();
                if (keyItem != null)
                {
                    PlaceKeyItemOnBoard(keyItem);
                }
            }
        }
    }

    void PlaceKeyItemOnBoard(KeyItem keyItem)
    {
        // Implement logic to place the key item on the board
        // For example, find the corresponding quad using keyItem.ItemId and place the key item there
        Debug.Log("Placing key item: " + keyItem.itemName);

        // Example logic to place the key item on a quad
        Transform targetQuad = FindQuadForItem(keyItem.ItemId);
        if (targetQuad != null)
        {
            keyItem.transform.SetParent(targetQuad);
            keyItem.transform.localPosition = Vector3.zero;
        }
    }

    Transform FindQuadForItem(string itemId)
    {
        // Find the quad on the board that matches the itemId
        foreach (Transform quad in boardContainer)
        {
            if (quad.name == itemId) // Assuming quads are named after itemIds
            {
                return quad;
            }
        }
        return null;
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        RemoveItemButton(item);
        EventBus.Publish(new ItemRemovedEvent(item));
    }

    public void UseItem(InventoryItem item)
    {
        item.Use();
    }

    private void CreateItemButton(InventoryItem item)
    {
        GameObject buttonObject = Instantiate(itemButtonPrefab);
        buttonObject.transform.SetParent(contentTransform, false);

        if (item is KeyItem)
        {
            buttonObject.transform.SetParent(keyItemContainer, false);
        }
        else if (item is Resource)
        {
            buttonObject.transform.SetParent(resourceItemContainer, false);
        }

        item.btnIndex = buttonObject.transform.GetSiblingIndex();
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(() => InspectItem(item));

        TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
        buttonText.text = item.itemName;

        Image buttonImage = buttonObject.GetComponentInChildren<Image>();
        buttonImage.sprite = item.itemIcon;
    }

    private void RemoveItemButton(InventoryItem item)
    {
        Transform container = item is KeyItem ? keyItemContainer : resourceItemContainer;
        if (container != null && item.btnIndex < container.childCount)
        {
            Destroy(container.GetChild(item.btnIndex).gameObject);
        }
    }

    private void InspectItem(InventoryItem item)
    {
        // Remove previously previewed items from preview
        for (int i = 0; i < itemPreviewParent.childCount; i++)
        {
            if (itemPreviewParent.GetChild(i) != itemPreviewParent)
                Destroy(itemPreviewParent.GetChild(i).gameObject);
        }

        EventBus.Publish(new ItemInspectedEvent(item));

        // Display item details in the inspection panel
        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.itemIcon;
        itemDescriptionText.text = item.GetDescription(); // Assuming GetDescription() returns item details

        if (item.itemPreview != null)
        {
            Instantiate(item.itemPreview, Vector3.zero, Quaternion.identity, itemPreviewParent);
        }
    }

    private void OnItemInspected(ItemInspectedEvent inspectedEvent)
    {
        Debug.Log("Item inspected: " + inspectedEvent.Item.itemName);
    }

    private void OnItemAdded(ItemAddedEvent addedEvent)
    {
        Debug.Log("Item added: " + addedEvent.Item.itemName);
        CreateItemButton(addedEvent.Item);
    }

    private void OnItemRemoved(ItemRemovedEvent removedEvent)
    {
        Debug.Log("Item removed: " + removedEvent.Item.itemName);
        RemoveItemButton(removedEvent.Item);
    }

    public void DisplayKeyItem(InventoryItem item)
    {
        if (item is KeyItem)
        {
            GameObject displayObject = Instantiate(item.itemPreview);
            displayObject.transform.position = new Vector3(0, 0, 0); // Set the position appropriately
            displayObject.layer = LayerMask.NameToLayer("KeyItemsLayer"); // Assign to KeyItemsLayer

            // Additional setup if needed
        }
    }

    // Call this method when adding a key item
    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        CreateItemButton(item);

        if (item is KeyItem)
        {
            DisplayKeyItem(item);
        }

        EventBus.Publish(new ItemAddedEvent(item));
    }
}

