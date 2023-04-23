using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGhostAi : ZombieAi
{
    public float speedDecrease = 0.25f;
    public float dmgInc = 3f;
    float baseSpeed = 0;
    float rtimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = base.zombieAgent.speed;
        base.dmg *= dmgInc;
        base.sightDist *= 2f;
    }

    // Update is called once per frame
    new void Update()
    {
        base.health = 99999;
        base.zombieAgent.speed = baseSpeed * speedDecrease;
        base.zombieAgent.destination = GameManager.instance.player.transform.position;
        if (!canDmgNow) rtimer += Time.deltaTime;
        if (rtimer > dmgTickRate)
        {
            rtimer = 0;
            canDmgNow = true;
        }
        if(Vector3.Distance(this.transform.position,GameManager.instance.player.transform.position)
            < base.sightDist)
        {
            FogScript.enableFog = 1;
        }
        else
        {
            FogScript.enableFog = 0;
        }
    }
    private void OnDestroy()
    {
        FogScript.enableFog = 0;
    }
}
