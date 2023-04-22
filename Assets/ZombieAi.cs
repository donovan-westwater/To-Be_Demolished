using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAi : MonoBehaviour
{
    public float health = 100f;
    public float sightDist = 10f;
    public GameObject targetBuildingObj;
    public GameObject ecoPrefab;
    public float dmg = 5f;
    public float dmgTickRate = 1f; 
    protected NavMeshAgent zombieAgent;
    protected GameObject playerObj;
    float timer = 0;
    bool canDmgNow = true;
    enum ZombieStates
    {
        Building,
        Player
    };
    ZombieStates curState = ZombieStates.Building;
    // Start is called before the first frame update
    void Awake()
    {
        targetBuildingObj = GameManager.instance.curTarget;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        zombieAgent = this.GetComponent<NavMeshAgent>();
        zombieAgent.destination = targetBuildingObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!zombieAgent.isOnNavMesh || !zombieAgent.isActiveAndEnabled) return;
        if (!canDmgNow) timer += Time.deltaTime;
        if(timer > dmgTickRate)
        {
            timer = 0;
            canDmgNow = true;
        }
        if (health <= 0)
        {
            GameManager.instance.enemies.Remove(this.gameObject);
            GameObject.Instantiate(ecoPrefab, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
            return;
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
                    zombieAgent.destination = targetBuildingObj.transform.position;
                    curState = ZombieStates.Building;
                }
            }
            else
            {
                zombieAgent.destination = targetBuildingObj.transform.position;
                curState = ZombieStates.Building;
            }
        }
        else
        {
            zombieAgent.destination = targetBuildingObj.transform.position;
            curState = ZombieStates.Building;
        }

        if (curState == ZombieStates.Building)
        {
            float d = Vector3.Distance(targetBuildingObj.transform.position, this.transform.position);
            if (d < 30f && canDmgNow)
            {
                RaycastHit hit;
                Vector3 dir = targetBuildingObj.transform.position - this.transform.position;
                if (Physics.Raycast(this.transform.position, dir.normalized, out hit, zombieAgent.radius+0.1f))
                {
                    if (hit.collider.gameObject.Equals(targetBuildingObj))
                    {
                        GameManager.instance.buildingHealth -= dmg;
                        canDmgNow = false;
                    }
                    
                }
            }
        }
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag.Equals("Player") && canDmgNow)
        {
            GameManager.instance.health -= dmg;
            canDmgNow = false;
        }
    }
}
