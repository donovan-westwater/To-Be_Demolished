using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ectoplasum : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.transform.tag.Equals("Player")) return;
        GameManager.instance.ectoAmount += 1;
        EctoCollector.caughtRigidbodies.Remove(this.GetComponent<Rigidbody>());
        Destroy(this.gameObject);
    }
}
