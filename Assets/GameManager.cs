using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject zombiePrefab;
    public static GameManager instance;
    float ctime = 0f;
    float timer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ctime += Time.deltaTime;
        if (ctime > timer)
        {
            ctime = 0;
            zombiePrefab.SetActive(true);
            foreach (GameObject s in spawnPoints)
            {
                GameObject.Instantiate(zombiePrefab, s.transform.position,s.transform.rotation);
            }
            zombiePrefab.SetActive(false);
        }
    }
}
