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

    private void Start()
    {
        EventBus.Subscribe<ItemInspectedEvent>(OnItemInspected);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<ItemInspectedEvent>(OnItemInspected);
    }

    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        if (item is KeyItem)
        {
            item.transform.SetParent(keyItemContainer, false);
        }
        else if (item is Resource)
        {
            item.transform.SetParent(resourceItemContainer, false);
        }
        CreateItemButton(item);

        EventBus.Publish(new ItemAddedEvent(item));
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        Destroy(item.gameObject);

        EventBus.Publish(new ItemRemovedEvent(item));
    }

    public void UseItem(InventoryItem item)
    {
        item.Use();
    }

    private void CreateItemButton(InventoryItem item)
    {
        GameObject buttonObject = Instantiate(itemButtonPrefab, contentTransform);
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(() => InspectItem(item));

        Text buttonText = buttonObject.GetComponentInChildren<Text>();
        buttonText.text = item.itemName;

        Image buttonImage = buttonObject.GetComponentsInChildren<Image>()[1];
        buttonImage.sprite = item.itemIcon;
    }

    private void InspectItem(InventoryItem item)
    {
        EventBus.Publish(new ItemInspectedEvent(item));
        // Display item details in the inspection panel
        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.itemIcon;
        itemDescriptionText.text = item.GetDescription(); // Assuming GetDescription() returns item details
    }

    private void OnItemInspected(ItemInspectedEvent inspectedEvent)
    {
        Debug.Log("Item inspected: " + inspectedEvent.Item.itemName);
    }
}