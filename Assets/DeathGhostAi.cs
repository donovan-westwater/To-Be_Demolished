using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGhostAi : ZombieAi
{
    public float healthMulti = .75f;
    public float speedInc = 1.5f;
    public float dmgMulti = 2.5f;
    public float sightMulti = 2f;
    float startSpeed = 0;
    Placeables target = null;
    // Start is called before the first frame update
    void Start()
    {
        base.health *= healthMulti;
        base.dmg *= dmgMulti;
        startSpeed = base.zombieAgent.speed;
        base.sightDist *= sightMulti;
    }

    // Update is called once per frame
    new void Update()
    {
        
        base.Update();
        if (base.health <= 0) return;
        base.zombieAgent.speed = startSpeed * speedInc;
        Collider[] colliders = Physics.OverlapSphere(this.transform.position,
            base.sightDist,Physics.IgnoreRaycastLayer);
        float min = 9999;
        if (colliders.Length < 1) return;
        Vector3 targetPos = Vector3.zero;
        foreach(Collider c in colliders)
        {
            if (c.gameObject.CompareTag("Placable"))
            {
                float d = Vector3.Distance(this.transform.position, c.transform.position);
                if (min > d)
                {                
                    if(c.transform.parent != null)
                    {
                        min = d;
                        target = c.transform.parent.GetComponentInParent<Placeables>();
                    }
                    
                }
            }
        }
        
        if (target != null)
        {
            if (target.health <= 0) target = null;           
            else base.zombieAgent.destination = target.transform.position;
        }
        
    }
}
