using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject zombiePrefab;
    public GameObject player;
    public GameObject gameOver;
    public Text healthText;
    public Text buildingHealthText;
    public static GameManager instance;
    [HideInInspector]
    public float health = 100f;
    public float buildingHealth = 1000f;
    float ctime = 0f;
    float timer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        Time.timeScale = 1;
        gameOver.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        buildingHealthText.text = "Building Health: " + buildingHealth;
        healthText.text = "Health: " + health;
        if (health <= 0 || buildingHealth <= 0)
        {
            gameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            return;
        }
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
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
