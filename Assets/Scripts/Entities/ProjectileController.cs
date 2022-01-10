using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float bullet_speed;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, bullet_speed*Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(0, bullet_speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tag == "PlayerBullet")
        {
            if (collision.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyController>().Death();
                Destroy(gameObject);
            }
            else if (collision.tag == "Obstacle")
            {
                Destroy(gameObject);
            }
        }
        else if (tag == "EnemyBullet")
        {
            Debug.Log(collision.tag);
            if (collision.tag == "Player")
            {
                Debug.Log("GAME OVER");
            }
            else if (collision.tag == "Obstacle")
            {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
    
}
