using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D box;
    float direction = 1;
    float horizontal_movespeed;
    public float collider_offset;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        AdjoustSize();
    }

    void Movement()
    {
        rb.velocity = new Vector2(direction * horizontal_movespeed * Time.deltaTime, 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        direction *= -1;
        GameController.SideCollision();
    }

    private void AdjoustSize()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        horizontal_movespeed = enemies[0].GetComponent<EnemyController>().horizontal_movespeed;
        GameObject min_x = enemies[0];
        GameObject max_x = enemies[0];
        for(int i = 1;i<enemies.Length;++i)
        {
            if(enemies[i].transform.position.x < min_x.transform.position.x)
            {
                min_x = enemies[i];
            }
            if (enemies[i].transform.position.x > max_x.transform.position.x)
            {
                max_x = enemies[i];
            }
        }

        box.transform.SetPositionAndRotation((min_x.transform.position + max_x.transform.position) / 2, Quaternion.identity);
        box.size = new Vector2((max_x.transform.position.x+collider_offset) - (min_x.transform.position.x - collider_offset), 1);
    }

    private void OnEnable()
    {
        GameController.OnEnemyDeath += AdjoustSize;
    }
    private void OnDisable()
    {
        GameController.OnEnemyDeath -= AdjoustSize;
    }
}
