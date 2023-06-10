using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 dir = new Vector3(0,0,0);
    public float speed = 50f;
    public float dmg = 5f;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {
        Debug.Log("I have been shot!");
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position += dir.normalized * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 2f) Destroy(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Enemy"))
        {
            collision.collider.GetComponent<ZombieAi>().health -= dmg;
        }
        Debug.Log(collision.collider.tag);
        Destroy(this.gameObject);

    }
}
