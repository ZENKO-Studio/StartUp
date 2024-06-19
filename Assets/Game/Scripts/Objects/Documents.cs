using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Documents : Object
{
    Image img;
    Sprite image;

    [Tooltip("If its a letter, set this to false")]
    [SerializeField] bool bImage = true;

    [SerializeField] bool bIsKey = true;

    private void Start()
    {
        img = GetComponentInChildren<Image>();
    }

    //This is for photos only
    public void SetImage(Sprite s)
    {
        if (!bImage)
            return;

        image = s;
        img.sprite = s;

        //Cause technically we are storing found images on pickup 
        PickUp();
    }

    public  void PickUp()
    {
        if (bImage)
        {
            if (bIsKey)
            {
                //Store in Image Container
            }
            else
            {
                //Store Temporarily
            }
        }
        else
        {
            //Store in Letters Container
        }
    }
}
