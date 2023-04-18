using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour
{
    public float health = 100f;

    private void Update()
    {
        if (health <= 0) Destroy(this.gameObject);
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(this.transform.position, GameManager.instance.player.transform.position) < 3f)
            {
                GameManager.instance.inventory[1]++;
                Destroy(this.gameObject);
            }


        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")) health -= 1;
    }
}
