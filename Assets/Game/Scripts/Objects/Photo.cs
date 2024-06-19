/**
 * @Sami 18/06/24
 * This is the class for the pictures that will be clicked by Camera
 * The picture prefab for the Capture thing should bve this
 */

using UnityEngine.UI;

public class Photo : InventoryItem
{
    //This is the UI where the Captured Image will be on
    public Image image;

    private void Start()
    {
        bInteractable = false;
        image = GetComponentInChildren<Image>();
    }

    public override void Interact(CharacterBase player)
    {
        inventoryHandler.AddPhoto(this);
    }
}

