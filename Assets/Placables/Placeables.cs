using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Placeables : MonoBehaviour
{
    public float health = 100f;
    public Sprite menuIcon;
    public Sprite upgradeIcon;
    public int type = 0;
    public enum Upgrades
    {
        NONE,
        HEALTH

    };
    public Upgrades currentUpgrade = Upgrades.NONE;
    // Update is called once per frame
    public void Update()
    {
        if (health <= 0) Destroy(this.gameObject);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(this.transform.position, GameManager.instance.player.transform.position) < 3f)
            {
                GameManager.instance.inventory.Add(this);
                Destroy(this.gameObject);
            }


        }
    }
    public void CopyAttributes(Placeables refObject)
    {
        this.health = refObject.health;
        this.currentUpgrade = refObject.currentUpgrade;
        if (this.currentUpgrade == Upgrades.HEALTH) this.health *= 4;

    }
    public void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")) health -= 1;
    }
}
