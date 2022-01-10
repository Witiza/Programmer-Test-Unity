using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public float horizontal_movespeed;
    public float vertical_step;
    int direction = 1;
    Animator animator;
    public float bullet_offset = -0.5f;
    public int attack_chance = 1;
    public float attack_cooldown = 2f;
    public float speed_multiplier = 0.1f;
    public LayerMask mask;

    bool paused = false;

    public GameObject bullet;
    public GameObject explosion_particles;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CheckAttackAvailability();
    }
    void CheckAttackAvailability()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,50,mask);

        if (!hit)
        {
            StopAllCoroutines(); //Done so the coroutines do not stack
            StartCoroutine(AttackCooldown());
        }
 

    }
    void Movement()
    {
        if (!paused)
        {
            rb.velocity = new Vector2(direction * horizontal_movespeed * Time.deltaTime, 0);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    private void AttemptAttack()
    {
        if(Random.Range(0, 100)<attack_chance)
        {
            Vector2 bullet_position = rb.position;
            bullet_position.y += bullet_offset;
            Instantiate(bullet, bullet_position, Quaternion.identity);
        }
        StartCoroutine(AttackCooldown());
    }
    private void ChangeDirection()
    {
        direction *= -1;
        Vector2 new_positon = rb.position;
        new_positon.y += vertical_step;
        rb.position = new_positon;
    }
    public void Death()
    {
        tag = "Dead";
        //We do this so we can update the movement just one time, instead of every time OtherEnemyDeath is called
        GameObject.FindGameObjectWithTag("EnemyMovement").GetComponent<EnemyMovement>().horizontal_movespeed += horizontal_movespeed * speed_multiplier;
        GameController.EnemyDied();
        Instantiate(explosion_particles, rb.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OtherEnemyDeath()
    {
        if(tag == "Enemy")
        {
            horizontal_movespeed += horizontal_movespeed * speed_multiplier;
            CheckAttackAvailability();
            animator.speed *= 1.5f;
        }
    }

    private void PlayerHit()
    {
        StopAllCoroutines();
        StartCoroutine(Pause(0.5f));
    }
    private void GameFinished()
    {
        StartCoroutine(Pause(5f));
    }
    private void OnEnable()
    {
        GameController.OnSideCollision += ChangeDirection;
        GameController.OnEnemyDeath += OtherEnemyDeath;
        GameController.OnPlayerHit += PlayerHit;
        GameController.OnGameFinish += GameFinished;
    }
    private void OnDisable()
    {
        GameController.OnSideCollision -= ChangeDirection;
        GameController.OnEnemyDeath -= OtherEnemyDeath;
        GameController.OnPlayerHit -= PlayerHit;
        GameController.OnGameFinish -= GameFinished;
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(Random.Range(attack_cooldown/2,attack_cooldown*2));
        AttemptAttack();
    }

    IEnumerator Pause(float seconds)
    {
        paused = true;
        yield return new WaitForSeconds(seconds);
        paused = false;
        CheckAttackAvailability();
    }
}
