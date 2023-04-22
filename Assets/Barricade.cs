using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : Placeables
{
    public Sprite barIcon;
    public float barHealth;
    private void Start()
    {
        if (base.currentUpgrade == Upgrades.HEALTH) this.barHealth *= 4;
        base.health = barHealth;
        this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = currentColor;
        base.menuIcon = barIcon;
        base.type = 0;
    }
}
