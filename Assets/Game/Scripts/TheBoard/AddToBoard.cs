using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToBoard : MonoBehaviour
{
    public Camera keyItemCamera; // Reference to the camera that displays key items
    public Transform boardContainer; // Reference to the container holding the board quads

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = keyItemCamera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit))
    //        {
    //            KeyItem keyItem = hit.collider.GetComponent<KeyItem>();
    //            if (keyItem != null)
    //            {
    //                PlaceKeyItemOnBoard(keyItem);
    //            }
    //        }
    //    }
    //}

    //void PlaceKeyItemOnBoard(KeyItem keyItem)
    //{
    //    // Logic to place the key item on the board
    //    // For example, find the corresponding quad using keyItem.ItemId and place the key item there
    //    Debug.Log("Placing key item: " + keyItem.itemName);

    //    // Find the corresponding quad on the board
    //    Transform targetQuad = FindQuadForItem(keyItem.ItemId);
    //    if (targetQuad != null)
    //    {
    //        keyItem.transform.SetParent(targetQuad);
    //        keyItem.transform.localPosition = Vector3.zero;
    //    }
    //}

    //Transform FindQuadForItem(string itemId)
    //{
    //    // Find the quad on the board that matches the itemId
    //    foreach (Transform quad in boardContainer)
    //    {
    //        if (quad.name == itemId) // Assuming quads are named after itemIds
    //        {
    //            return quad;
    //        }
    //    }
    //    return null;
    //}

}
