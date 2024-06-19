using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
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

    #region The items
    public TMP_Text objectiveBox;
    //Populate this list to show strings
    public List<string> objectives = new List<string>();

    //Call this on start of Mission / Accordingly
    public void PopulateObjectives()
    {
        foreach (var objective in objectives) 
        {
            objectiveBox.text += $"{objective} \n";
        }
    }

    public GameObject itemButton;
    public Transform itemPanel;

    //Replace this with the actual class of item with necessary variables
    public List<GameObject> items = new List<GameObject>();
    //Call this on start of Mission / Accordingly
    public void PopulateInventory()
    {
        foreach (var item in items)
        {
            Instantiate(itemButton, itemPanel);
        }
    }

    public GameObject collectibleButton;
    public Transform collectiblePanel;

    //Replace this with the actual class of collectible with necessary variables
    public List<GameObject> collectibles = new List<GameObject>();
    //Call this on start of Mission / Accordingly
    public void PopulateCollectible()
    {
        foreach (var collectible in collectibles)
        {
            Instantiate(collectibleButton, collectiblePanel);
        }
    }
    #endregion
}
