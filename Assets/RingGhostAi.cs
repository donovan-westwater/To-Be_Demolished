using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingGhostAi : ZombieAi
{
    public float speedDecrease = 0.25f;
    public float dmgInc = 3f;
    float baseSpeed = 0;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        baseSpeed = base.zombieAgent.speed;
        base.dmg *= dmgInc;
    }

    // Update is called once per frame
    new void Update()
    {
        base.health = 99999;
        base.zombieAgent.speed = baseSpeed * speedDecrease;
        base.zombieAgent.destination = GameManager.instance.player.transform.position;
        if (!canDmgNow) timer += Time.deltaTime;
        if (timer > dmgTickRate)
        {
            timer = 0;
            canDmgNow = true;
        }
    }
}
