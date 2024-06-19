using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider staminaBar;

    [SerializeField] HUDMenu hudMenu;

    // Start is called before the first frame update
    void OnEnable()
    {

        if (GameManager.Instance.playerRef != null)
        {
            GameManager.Instance.playerRef.OnHealthChanged.AddListener(UpdateHealthbar);
            GameManager.Instance.playerRef.OnStaminaChanged.AddListener(UpdateStaminabar);
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

    void OnDisable()
    {
        if (GameManager.Instance.playerRef != null)
        {
            GameManager.Instance.playerRef.OnHealthChanged.RemoveListener(UpdateHealthbar);
            GameManager.Instance.playerRef.OnStaminaChanged.RemoveListener(UpdateStaminabar);
        }
    }
}
