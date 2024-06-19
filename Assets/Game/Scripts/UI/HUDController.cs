using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (GameManager.Instance.playerRef != null)
            GameManager.Instance.playerRef.OnHealthChanged.AddListener(UpdateHealthbar);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealthbar()
    {
        healthBar.value = GameManager.Instance.playerRef.GetHealth();
    }

    void OnDisable()
    {
        if (GameManager.Instance.playerRef != null)
            GameManager.Instance.playerRef.OnHealthChanged.RemoveListener(UpdateHealthbar);
    }
}
