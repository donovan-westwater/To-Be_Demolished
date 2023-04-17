using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public float sightDst = 15f;
    float health = 100f; //Going to ignore for now
    public GameObject bullet;
    private GameObject head;
    private GameObject curTarget = null;
    void Awake()
    {
        head = this.transform.GetChild(0).GetChild(0).gameObject;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(curTarget == null)
        {
            float min = 99999;
            GameObject tmp = null;
            foreach (GameObject s in GameManager.instance.enemies)
            {
                float d = Vector3.Distance(s.transform.position, this.transform.position);
                if (min > d) { min = d; tmp = s; }
                else continue;
                
            }
            if (min < sightDst && tmp != null)
            {
                curTarget = tmp;
            }
        }        
        else{
            Vector3 dir = curTarget.transform.position - this.transform.position;
            dir.y = 0;
            Vector3 f = this.transform.GetChild(0).forward;
            f.y = 0;
            this.transform.GetChild(0).transform.rotation *= Quaternion.FromToRotation(this.transform.GetChild(0).forward, dir.normalized);
            

            GameObject b = GameObject.Instantiate(bullet, head.transform.position, this.transform.GetChild(0).transform.rotation);
            b.GetComponent<Bullet>().dir = dir.normalized;
            if (curTarget.GetComponent<ZombieAi>().health <= 0) curTarget = null;
        }
    }
}
