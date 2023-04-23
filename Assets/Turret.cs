using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

//Placables should have a base class to derive from!
public class Turret : Placeables
{
    // Start is called before the first frame update
    private StudioEventEmitter myEmitter;
    public float sightDst = 15f;
    public float fireRate = .5f;
    public float dmg = 5f;
    float timer = 0f;
    public GameObject bullet;
    private GameObject head;
    private GameObject curTarget = null;
    
    public enum TurretUpgrades
    {
        NONE = 0,
        HEALTH = 1,
        DAMAGE = 2,
        FIRE_RATE = 4,
        SIGHT_RANGE = 8
    };
    public TurretUpgrades turretUpgrade = TurretUpgrades.NONE;
    void Awake()
    {
        head = this.transform.GetChild(0).GetChild(0).gameObject;
        base.type = 1;
        myEmitter = gameObject.GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        this.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = currentColor;
        if (curTarget == null)
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
                    myEmitter.Play();
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
    }
    public void CopyAttributes(Turret refObject)
    {
        this.health = refObject.health;
        this.upgradeIcon = refObject.upgradeIcon;
        this.turretUpgrade = refObject.turretUpgrade;
        this.currentColor = refObject.currentColor;
        if ((this.turretUpgrade & TurretUpgrades.HEALTH) == TurretUpgrades.HEALTH) this.health *= 2;
        if ((this.turretUpgrade & TurretUpgrades.FIRE_RATE) == TurretUpgrades.FIRE_RATE) this.fireRate /= 2;
        if ((this.turretUpgrade & TurretUpgrades.DAMAGE) == TurretUpgrades.DAMAGE) this.dmg *= 2;
        if ((this.turretUpgrade & TurretUpgrades.SIGHT_RANGE) == TurretUpgrades.SIGHT_RANGE) this.sightDst *= 2;

    }
}
