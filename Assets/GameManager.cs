using Array2DEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] placables;
    public Sprite[] icons;
    public Image curIcon;
    public Text placeAmount;
    public GameObject[] zombiePrefab;
    public GameObject player;
    public GameObject gameOver;
    public GameObject endstate;
    //UI
    public GameObject radioObject;
    public GameObject menu;
    public Text healthText;
    public Text buildingHealthText;
    public Text ectoAmountText;
    public static GameManager instance;
    [HideInInspector]
    public float health = 100f;
    public float buildingHealth = 1000f;
    public float ectoAmount = 0;
    [HideInInspector]
    public List<GameObject> enemies;
    public int[] inventory = { 1, 1 };
    public int inventoryCount = 2;
    int currentIndex = 0;
    GameObject curPlacable;
    //Wave System
    public float searchTime = 30f;
    public float timeBetweenSpawns = 1f;
    public float timeBetweenWaves = 30f;
    public Array2DInt numOfEneimesEachWave;
    [HideInInspector]
    public bool radioOn = false;
    bool[] emptyCheck;
    [HideInInspector]
    public static List<TargetBuilding> targets; 
    //[HideInInspector]
    public GameObject curTarget;
    Vector2 basePlayerSpeed = new Vector2(5,7);
    float ctime = 0f;
    float stime = 0f;
    float btime = 0f;
    int currentWave = -1;
    int spawnPointAvaliable = 0;
    float rotAngle = 0;
    bool waveMode = true;
    bool spawnMode = false;
    bool menuMode = false;
    //bool waveMode = true;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            targets = new List<TargetBuilding>();
        }
    }
    void Start()
    {
        
        enemies = new List<GameObject>();
        
        Time.timeScale = 1;
        gameOver.gameObject.SetActive(false);
        endstate.gameObject.SetActive(false);
        radioObject.SetActive(false);
        menu.SetActive(false);
        //Randomly Select New Target
        Random.InitState(System.DateTime.Now.Millisecond+System.DateTime.Now.Second);
        int r = Random.Range(0, targets.Count);
        targets[r].isTarget = true;
        curTarget = targets[r].gameObject;

        basePlayerSpeed.x = player.GetComponent<FirstPersonController>().walkSpeed;
        basePlayerSpeed.y = player.GetComponent<FirstPersonController>().sprintSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        player.GetComponent<FirstPersonController>().walkSpeed = Mathf.Clamp(1f-inventoryCount/50f
            ,.5f,1f)*basePlayerSpeed.x;
        player.GetComponent<FirstPersonController>().sprintSpeed = Mathf.Clamp(1f - inventoryCount / 50f
            , .5f, 1f) * basePlayerSpeed.y;
        buildingHealthText.text = "Building Health: " + buildingHealth;
        healthText.text = "Health: " + health;
        ectoAmountText.text = "Ecto: " + ectoAmount;
        if (health <= 0 || buildingHealth <= 0)
        {
            menu.SetActive(false);
            menuMode = false;
            gameOver.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            return;
        }
        if(currentWave >= numOfEneimesEachWave.GridSize.x)
        {
            menu.SetActive(false);
            menuMode = false;
            endstate.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            return;
        }
        if (waveMode)
        {
            ctime += Time.deltaTime;
            stime += Time.deltaTime;
        }
        else btime += Time.deltaTime;
        if(btime > searchTime && !waveMode) //THink of this as the end of a wave
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                GameObject.Destroy(enemies[i].gameObject);
            }
            enemies.Clear();
            //Randomly Select new target
            curTarget.GetComponent<TargetBuilding>().isTarget = false;
            int r = Random.Range(0, targets.Count);
            targets[r].isTarget = true;
            curTarget = targets[r].gameObject;
            btime = 0;
            waveMode = true;
        }
        if (ctime > timeBetweenWaves && waveMode)
        {
            spawnMode = true;
            currentWave++;
            spawnPointAvaliable++;
            emptyCheck = new bool[numOfEneimesEachWave.GridSize.y];
            for (int i = 0; i < emptyCheck.Length; i++)
            {
                emptyCheck[i] = false;
            }
            stime = 0;
            ctime = 0;
        }
        if (spawnMode && stime > timeBetweenSpawns)
        {
            
            int r = Random.Range(0, zombiePrefab.Length);
            int sl = Random.Range(0, spawnPointAvaliable);
            GameObject spawn = spawnPoints[sl];
            GameObject e = zombiePrefab[r];
            if (!emptyCheck[r])
            {
                e.SetActive(true);
                GameObject g = GameObject.Instantiate(e, spawn.transform.position, spawn.transform.rotation);
                enemies.Add(g);
                int n = numOfEneimesEachWave.GetCells()[r,currentWave]--;
                numOfEneimesEachWave.SetCell(currentWave, r, n - 1);
                emptyCheck[r] = numOfEneimesEachWave.GetCells()[r, currentWave] < 1;
                e.SetActive(false);
                stime = 0;
            }
            bool noSpawns = false;
            for (int index = 0; index < emptyCheck.Length; index++)
            {
                noSpawns = noSpawns & emptyCheck[index];
            }
            if (noSpawns) {
                Debug.Log("no spawns!");
                spawnMode = false;
                waveMode = false;
                stime = 0;
            }
        }
        if (menuMode)
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Cursor.lockState = CursorLockMode.Locked;
                menuMode = false;
                menu.SetActive(false);
            }
            return;
        }
        InputCheck();
    }
    void InputCheck()
    {
        placeAmount.text = "" + inventory[currentIndex];
        curIcon.sprite = icons[currentIndex];
        if(inventory[currentIndex] > 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                curPlacable = GameObject.Instantiate(placables[currentIndex]);
                curPlacable.GetComponent<MonoBehaviour>().enabled = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                inventory[currentIndex]--;
                inventoryCount--;
                if (inventory[currentIndex] <= 0) inventory[currentIndex] = 0;
                curPlacable.GetComponent<MonoBehaviour>().enabled = true;
                curPlacable = null;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(curPlacable);
                curPlacable = null;
            }
            
        }
        
        if(curPlacable != null)
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(r,out hit))
            {
                curPlacable.transform.position = hit.point;
                curPlacable.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                curPlacable.transform.Rotate(Vector3.up, rotAngle);
            }
            if (Input.GetKey(KeyCode.E))
            {
                rotAngle += 40f*Time.deltaTime;
            }else if (Input.GetKey(KeyCode.Q))
            {
                rotAngle -= 40f * Time.deltaTime;
            }
            
        }
        else
        {
            
            if (Input.mouseScrollDelta.y > 0) currentIndex++;
            if (Input.mouseScrollDelta.y < 0) currentIndex--;
            if (currentIndex < 0) currentIndex = 0;
            if (currentIndex >= inventory.Length) currentIndex = inventory.Length - 1;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                radioOn = !radioOn;
                radioObject.SetActive(radioOn);
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Cursor.lockState = CursorLockMode.None;
            menuMode = true;
            menu.SetActive(true);
        }
        
    }
    public void BuyTurret()
    {
        if (ectoAmount < 10) return;
        inventory[0]++;
        inventoryCount++;
        ectoAmount -= 10;
    }
    public void BuyBarricade()
    {
        if (ectoAmount < 5) return;
        inventory[1]++;
        inventoryCount++;
        ectoAmount -= 5;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
