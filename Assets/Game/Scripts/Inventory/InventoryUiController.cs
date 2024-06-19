using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static EventBus;

public enum InventoryPage : byte
{
    Usables,
    Keys,
    Documents,
    Photos
}

public class InventoryUiController : MonoBehaviour
{
    public GameObject inventoryCanvas;

    //References to the panels that`ll hold the respective buttons
    public Transform usableItemsHolder;
    public Transform keysHolder;
    public Transform documentsHolder;
    public Transform photosHolder;

    public GameObject itemButtonPrefab; // Reference to the ItemButton prefab

    // References to the inspection UI elements
    public TMP_Text itemNameText;
    public Image itemIconImage;
    public TMP_Text itemDescriptionText;
    public Transform itemPreviewParent;

    bool isOpen = false;

    #region Enable Disable
    private void OnEnable()
    {
        EventBus.Subscribe<ToggleInventoryEvent>(OnToggleInventory);
        inventoryCanvas.SetActive(false); // Ensure the inventory is initially hidden
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ToggleInventoryEvent>(OnToggleInventory);
    }
    #endregion

    private void OnToggleInventory(ToggleInventoryEvent toggleEvent)
    {
        inventoryCanvas.SetActive( toggleEvent.IsOpen );
    }

    public GameObject CreateItemButton(InventoryItem item, InventoryPage pageToAdd)
    {
        print($"Adding Button for {item.name}");

        Transform btnParent = null;

        switch (pageToAdd)
        {
            case InventoryPage.Usables:
                btnParent = usableItemsHolder;
                break;
            case InventoryPage.Keys:
                btnParent = keysHolder;
                break;
            case InventoryPage.Documents:
                btnParent = documentsHolder;
                break;
            case InventoryPage.Photos:
                btnParent = photosHolder;
                break;
        }

        GameObject buttonObject = Instantiate(itemButtonPrefab);
        buttonObject.transform.SetParent(btnParent, false);

        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(() => InspectItem(item));

        //TMP_Text buttonText = buttonObject.GetComponentInChildren<TMP_Text>();
        //buttonText.text = item.itemName;

        Image buttonImage = buttonObject.GetComponentInChildren<Image>();
        buttonImage.sprite = item.itemIcon;

        return buttonObject;
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
        itemDescriptionText.text = item.itemDescription;

        if (item.itemPreview != null)
        {
            Debug.Log($"Instantiating Item {item.name}");
            GameObject g = Instantiate(item.itemPreview, itemPreviewParent);
            g.transform.localPosition = Vector3.zero;
            g.transform.rotation = Quaternion.identity;
            g.SetActive(true);
        }
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
