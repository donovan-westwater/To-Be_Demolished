using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAi : MonoBehaviour
{
    public float health = 100f;
    public float sightDist = 10f;
    public GameObject testBuildingObj;
    private NavMeshAgent zombieAgent;
    private GameObject playerObj;
    enum ZombieStates
    {
        Building,
        Player
    };
    ZombieStates curState = ZombieStates.Building;
    // Start is called before the first frame update
    void Awake()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        zombieAgent = this.GetComponent<NavMeshAgent>();
        zombieAgent.destination = testBuildingObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GameManager.instance.enemies.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
            if (Vector3.Distance(playerObj.transform.position,this.transform.position) < sightDist)
        {
            RaycastHit hit;
            Vector3 dir = playerObj.transform.position - this.transform.position;
            if(Physics.Raycast(this.transform.position, dir.normalized, out hit, sightDist))
            {
                if (hit.collider.gameObject.tag.Equals("Player"))
                {
                    zombieAgent.destination = playerObj.transform.position;
                    curState = ZombieStates.Player;
                }
                else
                {
                    zombieAgent.destination = testBuildingObj.transform.position;
                    curState = ZombieStates.Building;
                }
            }
        }
        
        if (curState == ZombieStates.Building)
        {
            float d = Vector3.Distance(testBuildingObj.transform.position, this.transform.position);
            if (d < 30f)
            {
                RaycastHit hit;
                Vector3 dir = testBuildingObj.transform.position - this.transform.position;
                if (Physics.Raycast(this.transform.position, dir.normalized, out hit, zombieAgent.radius+0.1f))
                {
                    GameManager.instance.buildingHealth -= 1f;
                }
            }
        }
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag.Equals("Player"))
        {
            GameManager.instance.health -= 1f;
        }
    }
}
