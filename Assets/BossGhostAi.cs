using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGhostAi : ZombieAi
{
    public float healthMulti = 6f;
    public float dmgMulti = 5f;
    public float speedDecrease = .25f;
    float baseSpeed = 0;
    void Start()
    {
        base.health *= healthMulti;
        base.dmg *= dmgMulti;
        baseSpeed = base.zombieAgent.speed;
        base.sightDist = 0; //Shouldn't bother the player
    }

    private new void Update()
    {
        base.zombieAgent.speed = baseSpeed * speedDecrease;
        base.Update();
    }
}
