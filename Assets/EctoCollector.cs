using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EctoCollector : MonoBehaviour
{
    public float magnetForce = 500;

    public static List<Rigidbody> caughtRigidbodies = new List<Rigidbody>();

    private void Awake()
    {
        caughtRigidbodies.Clear();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < caughtRigidbodies.Count; i++)
        {
            Vector3 dir = (transform.position - (caughtRigidbodies[i].transform.position + caughtRigidbodies[i].centerOfMass));
            caughtRigidbodies[i].velocity =  20*Mathf.Exp(-dir.magnitude/7)*dir.normalized * magnetForce * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Currency")) return;
        if (other.GetComponent<Rigidbody>())
        {
            Rigidbody r = other.GetComponent<Rigidbody>();

            if (!caughtRigidbodies.Contains(r))
            {
                //Add Rigidbody
                caughtRigidbodies.Add(r);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Equals("Currency")) return;
        if (other.GetComponent<Rigidbody>())
        {
            Rigidbody r = other.GetComponent<Rigidbody>();

            if (caughtRigidbodies.Contains(r))
            {
                //Remove Rigidbody
                caughtRigidbodies.Remove(r);
            }
        }
    }
}
