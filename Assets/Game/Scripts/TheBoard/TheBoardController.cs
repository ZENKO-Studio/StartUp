using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static EventBus;

public class TheBoardController : MonoBehaviour
{
    Inventory _inventory;

    [SerializeField] Transform _buttonArea; //Referenc to the Panel where buttons will be

    public GameObject itemButtonPrefab; // Reference to the ItemButton prefab

    private bool bDragging = false;

    [SerializeField] Camera boardCam;
        
    private void Start()
    {
       
    }

    private void OnEnable()
    { 
         _inventory = Inventory.Instance;
     
        PopulateKeyItems();

        EventBus.Subscribe<ItemAddedEvent>(OnItemAdded);
        EventBus.Subscribe<ItemRemovedEvent>(OnItemRemoved);
    }

    private void OnDisable()
    {   
        EventBus.Unsubscribe<ItemAddedEvent>(OnItemAdded);
        EventBus.Unsubscribe<ItemRemovedEvent>(OnItemRemoved);
    }


    //Called In Start
    void PopulateKeyItems()
    {
        //Delete all Item Buttons
        foreach (var item in _inventory.items)
        {
            if(item is KeyItem) 
                CreateItemButton(item);
        }
    }

    private void CreateItemButton(InventoryItem item)
    {
        GameObject buttonObject = Instantiate(itemButtonPrefab);
        buttonObject.transform.SetParent(_buttonArea, false);

        Button button = buttonObject.GetComponent<Button>();
        EventTrigger t = button.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener( (X) => DragItem(item, buttonObject.transform.GetSiblingIndex()));
        t.triggers.Add(pointerDown);

        Image buttonImage = buttonObject.GetComponentInChildren<Image>();
        buttonImage.sprite = item.itemIcon;
    }

    private void RemoveItemButton(InventoryItem item)
    {
        Destroy(_buttonArea.GetChild(selectedIndex).gameObject);
    }

    private GameObject g;

    int selectedIndex = -1;
    InventoryItem selectedItem;

    private void DragItem(InventoryItem item, int btnInd)
    {
        Debug.Log("Item clicked: " + item.itemName);
        bDragging = true;
        g = Instantiate(item.itemPreview);
        g.transform.rotation = Quaternion.identity;
        g.transform.localScale *= 100f;
        g.SetActive(true);
        selectedItem = item;
        selectedIndex = btnInd;
    }


    private void Update()
    {
        if(Input.GetMouseButtonUp(0) && bDragging)
        {
            bDragging = false;
            _inventory.RemoveItem(selectedItem);
        }

        if(bDragging)
        {
            if(g != null)
            {
                Ray r = boardCam.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(r, out RaycastHit hitInfo))
                {
                    if(hitInfo.collider.tag == "TheBoard" || hitInfo.collider.tag == "Slot")
                    {
                        g.transform.position = hitInfo.point;
                        g.transform.rotation = Quaternion.LookRotation(hitInfo.normal);
                    }
                }
            }
        }
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
}
