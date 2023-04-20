using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Placables should have a base class to derive from!
public class Turret : MonoBehaviour
{
    // Start is called before the first frame update
    public float sightDst = 15f;
    public float fireRate = .5f;
    public float dmg = 5f;
    float health = 100f; //Going to ignore for now
    float timer = 0f;
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
                Vector3 dir = s.transform.position - this.transform.position;
                RaycastHit rOut;
                Physics.Raycast(this.transform.position, dir.normalized, out rOut);
                if (min > d && rOut.collider != null && rOut.collider.CompareTag("Enemy")) { 
                    min = d; 
                    tmp = s; 
                }
                else continue;
                
            }
            if (min < sightDst && tmp != null)
            {
                curTarget = tmp;
            }
        }        
        else{
            timer += Time.deltaTime;
            if(timer > fireRate)
            {
                timer = 0;
                Vector3 dir = curTarget.transform.position - this.transform.position;
                RaycastHit rOut;
                Physics.Raycast(this.transform.position, dir.normalized, out rOut);
                if (rOut.collider != null && rOut.collider.gameObject.Equals(curTarget))
                {
                    dir.y = 0;
                    Vector3 f = this.transform.GetChild(0).forward;
                    f.y = 0;
                    this.transform.GetChild(0).transform.rotation *= Quaternion.FromToRotation(this.transform.GetChild(0).forward, dir.normalized);
                    GameObject b = GameObject.Instantiate(bullet, head.transform.position, this.transform.GetChild(0).transform.rotation);
                    b.GetComponent<Bullet>().dir = dir.normalized;
                    b.GetComponent<Bullet>().dmg = dmg;
                    if (curTarget.GetComponent<ZombieAi>().health <= 0) curTarget = null;
                }
                else
                {
                    curTarget = null;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(Vector3.Distance(this.transform.position,GameManager.instance.player.transform.position) < 3f)
            {
                GameManager.instance.inventory[0]++;
                GameManager.instance.inventoryCount++;
                Destroy(this.gameObject);
            }
            

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")) health -= 1;
    }
}
