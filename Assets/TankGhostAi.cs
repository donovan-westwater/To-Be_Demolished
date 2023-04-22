using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankGhostAi : ZombieAi
{
    // Start is called before the first frame update
    public float healthMulti = 4f;
    public float speedDecrease = .5f;
    float startSpeed = 0;
    void Start()
    {
        base.health *= healthMulti;
        startSpeed = base.zombieAgent.speed;
        base.sightDist = 0; //Shouldnt be able to see the player
    }
    private void Update()
    {
        base.zombieAgent.speed = startSpeed* speedDecrease;
    }
}
