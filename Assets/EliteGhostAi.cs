using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteGhostAi : ZombieAi
{
    public float healthMulti = 1.5f;
    public float dmgMulti = 2f;
    public float sightMulti = 1.1f;
    public float speedDecrease = .75f;

    // Start is called before the first frame update
    void Start()
    {
        base.health *= healthMulti;
        base.dmg *= dmgMulti;
        base.sightDist *= sightMulti;
        base.zombieAgent.speed *= speedDecrease;
    }
}
