/**@Sami 12/06/2024
 * 
 **/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EventBus;

public class InventoryHandler : Singleton<InventoryHandler>
{
    [SerializeField] InventoryUiController inventoryUI;

    //Just for Debugging Purpose, Will be removed later
    public List<InventoryItem> items = new List<InventoryItem>();

    //4 Dictionaries with Item and Button (Since we have to add and remove both)
    public Dictionary<Photo, GameObject> photos = new Dictionary<Photo, GameObject>(); 
    public Dictionary<Document, GameObject> documents = new Dictionary<Document, GameObject>(); 
    public Dictionary<Key, GameObject> keys = new Dictionary<Key, GameObject>(); 
    public Dictionary<UsableObject, GameObject> usables = new Dictionary<UsableObject, GameObject>(); 

    private void OnEnable()
    {
        // Initialize the inventory with existing items
        foreach (var item in items)
        {
            //CreateItemButton(item);
        }

        //EventBus.Subscribe<ItemInspectedEvent>(OnItemInspected);
        //EventBus.Subscribe<ItemAddedEvent>(OnItemAdded);
        //EventBus.Subscribe<ItemRemovedEvent>(OnItemRemoved);
    }

    private void OnDisable()
    {
        //EventBus.Unsubscribe<ItemInspectedEvent>(OnItemInspected);
        //EventBus.Unsubscribe<ItemAddedEvent>(OnItemAdded);
        //EventBus.Unsubscribe<ItemRemovedEvent>(OnItemRemoved);
    }



    public void AddItem(InventoryItem item)
    {
        items.Add(item);
        //CreateItemButton(item);

        EventBus.Publish(new ItemAddedEvent(item));
    }

    public void RemoveItem(InventoryItem item)
    {
        items.Remove(item);
        
        EventBus.Publish(new ItemRemovedEvent(item));
    }

    private void OnItemInspected(ItemInspectedEvent inspectedEvent)
    {
        Debug.Log("Item inspected: " + inspectedEvent.Item.itemName);
    }

    internal void AddUsable(UsableObject usableObject)
    {
        print($"Adding {usableObject.name}");
        usables.Add(usableObject, inventoryUI.CreateItemButton(usableObject, InventoryPage.Usables));
        print(usables.ToString());
    }

    internal void AddDocument(Document document)
    {

        print($"Adding {document.name}");
        documents.Add(document, inventoryUI.CreateItemButton(document, InventoryPage.Documents));
        print(usables.ToString());
    }

    internal void AddPhoto(Photo photo)
    {

        print($"Adding {photo.name}");

        photos.Add(photo, inventoryUI.CreateItemButton(photo, InventoryPage.Photos));
        print(usables.ToString());
    }

    internal void AddKey(Key key)
    {
        print($"Adding {key.name}");
        keys.Add(key, inventoryUI.CreateItemButton(key, InventoryPage.Keys));
        print(usables.ToString());
    }
}