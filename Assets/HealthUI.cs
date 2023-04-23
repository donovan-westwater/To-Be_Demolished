using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image healthBarImage; 
    
    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(GameManager.instance.health / 100f, 0, 1f);
    }
}
