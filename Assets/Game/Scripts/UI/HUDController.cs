using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Health And Stamina")]
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    [Header("Flashlight")]
    [SerializeField] Image flashlightImage;

    [SerializeField] Sprite flashOnSprite;
    [SerializeField] Sprite flashOffSprite;

    [SerializeField] HUDMenu hudMenu;

    NellController nellController;
    Flashlight flashlight;

    // Start is called before the first frame update
    void OnEnable()
    {
        nellController = GameManager.Instance.playerRef;

        if (nellController != null)
        {
            nellController.OnHealthChanged.AddListener(UpdateHealthbar);
            nellController.OnStaminaChanged.AddListener(UpdateStaminabar);
            flashlight = nellController.flashlight;

            if (flashlight != null)
                flashlight.OnFlashLightToggle.AddListener(UpdateFlashlightIcon);

            Debug.Log("Listener Added!");
        }
        else
        {
            Debug.Log("Listener Not Added!");

        }


        //hudMenu.Invoke("HideHUD", 5f);
    }

    

    //// Update is called once per frame
    //void Update()
    //{

    //}

    void UpdateHealthbar()
    {
        Debug.Log("UpdatingHealthBar");
        healthBar.value = GameManager.Instance.playerRef.GetHealth();
        //hudMenu.ShowHUD();
        //hudMenu.Invoke("HideHUD", 5f);
    }
    
    void UpdateStaminabar()
    {
        Debug.Log("UpdatingStaminaBar");
        staminaBar.value = GameManager.Instance.playerRef.GetStamina();
        //hudMenu.ShowHUD();
        //hudMenu.Invoke("HideHUD", 5f);
    }

    private void UpdateFlashlightIcon()
    {
        flashlightImage.sprite = flashlight.IsOn() ? flashOnSprite : flashOffSprite;
    }

    void OnDisable()
    {
        if (nellController != null)
        {
            nellController.OnHealthChanged.RemoveListener(UpdateHealthbar);
            nellController.OnStaminaChanged.RemoveListener(UpdateStaminabar);
        }

        if (flashlight != null)
            flashlight.OnFlashLightToggle.RemoveListener(UpdateFlashlightIcon);
    }
}
