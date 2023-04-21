using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : Placeables
{
    public Sprite barIcon;
    public float barHealth;
    private void Start()
    {
        base.health = barHealth;
        base.menuIcon = barIcon;
        base.type = 0;
    }
}
