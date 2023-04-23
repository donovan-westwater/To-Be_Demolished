using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombHealth : MonoBehaviour
{
    public Image healthBarImage;

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Mathf.Clamp(GameManager.instance.buildingHealth / 1000f, 0, 1f);
    }
}
