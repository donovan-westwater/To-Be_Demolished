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
    public GameObject zombiePrefab;
    public GameObject player;
    public GameObject gameOver;
    //UI
    public GameObject menu;
    public Text healthText;
    public Text buildingHealthText;
    public Text ectoAmountText;
    public static GameManager instance;
    [HideInInspector]
    public float health = 100f;
    public float buildingHealth = 1000f;
    public float ectoAmount = 0;
    public List<GameObject> enemies;
    public int[] inventory = { 1, 1 };
    int currentIndex = 0;
    GameObject curPlacable;
    float ctime = 0f;
    float timer = 4f;
    float rotAngle = 0;
    bool menuMode = false;
    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        enemies = new List<GameObject>();
        Time.timeScale = 1;
        gameOver.gameObject.SetActive(false);
        menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        ctime += Time.deltaTime;
        if (ctime > timer)
        {
            ctime = 0;
            zombiePrefab.SetActive(true);
            foreach (GameObject s in spawnPoints)
            {
                GameObject g = GameObject.Instantiate(zombiePrefab, s.transform.position,s.transform.rotation);
                enemies.Add(g);
            }
            zombiePrefab.SetActive(false);
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
        ectoAmount -= 10;
    }
    public void BuyBarricade()
    {
        if (ectoAmount < 5) return;
        inventory[1]++;
        ectoAmount -= 5;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
