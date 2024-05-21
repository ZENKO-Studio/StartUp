using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        if (GameHandler.Instance.playerRef != null)
            GameHandler.Instance.playerRef.OnHealthChanged.AddListener(UpdateHealthbar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealthbar()
    {
        healthBar.value = GameHandler.Instance.playerRef.GetHealth();
    }
}