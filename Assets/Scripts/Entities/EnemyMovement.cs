using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class tells the enemies when they need to change direction
public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D box;

    float direction = 1;
    public float horizontal_movespeed;
    public float collider_offset;
    bool paused = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        //We do this so we can change the enemies movespeed by just modifiying their prefab
        horizontal_movespeed = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyController>().horizontal_movespeed;
        //We do this initially so changing the amount of enemies onscreen wont affect this script.
        AdjoustSize();
    }

    void Movement()
    {
        if (!paused)
        {
            rb.velocity = new Vector2(direction * horizontal_movespeed * Time.deltaTime, 0);
        }
        else
            rb.velocity = Vector2.zero; 
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            direction *= -1;
            GameController.SideCollision();
        }
    }

    //We only care about the two furthest enemies in the X axis, as it is where the enemies are bouncing.
    private void AdjoustSize()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
            return;

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

    private void PlayerHit()
    {
        StartCoroutine(Pause(0.5f));
    }
    private void GameFinished()
    {
        StartCoroutine(Pause(5f));
    }
    IEnumerator Pause(float seconds)
    {
        paused = true;
        yield return new WaitForSeconds(seconds);
        paused = false;
    }

    private void OnEnable()
    {
        GameController.OnEnemyDeath += AdjoustSize;
        GameController.OnPlayerHit += PlayerHit;
        GameController.OnGameFinish += GameFinished;
    }
    private void OnDisable()
    {
        GameController.OnEnemyDeath -= AdjoustSize;
        GameController.OnPlayerHit -= PlayerHit;
        GameController.OnGameFinish -= GameFinished;

    }

}
