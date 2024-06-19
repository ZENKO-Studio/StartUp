using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EventBus;
using static UnityEngine.UI.Toggle;

public class InventoryUiController : MonoBehaviour
{
    public GameObject inventoryCanvas;

    private void OnEnable()
    {
        EventBus.Subscribe<ToggleInventoryEvent>(OnToggleInventory);
        inventoryCanvas.SetActive(false); // Ensure the inventory is initially hidden
    }

    bool isOpen = false;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        isOpen = !isOpen;
    //        inventoryCanvas.SetActive(isOpen);

    //        if (isOpen)
    //        {
    //            Cursor.lockState = CursorLockMode.None; // Unlock the cursor
    //            Cursor.visible = true; // Make the cursor visible
    //        }
    //        else
    //        {
    //            Cursor.lockState = CursorLockMode.Locked; // Unlock the cursor
    //            Cursor.visible = false; // Make the cursor visible
    //        }
    //    }
    //}

    private void OnDisable()
    {
        EventBus.Unsubscribe<ToggleInventoryEvent>(OnToggleInventory);
    }

    private void OnToggleInventory(ToggleInventoryEvent toggleEvent)
    {
        inventoryCanvas.SetActive( toggleEvent.IsOpen );

    }

    #region Toggle Menu Pages
    //The 3 menu pages
    public List<GameObject> menuPages = new List<GameObject>();

    public void ResetMenuPages()
    {
        foreach (var page in menuPages)
        {
            page.SetActive(false);
        }
    }
    #endregion
}
