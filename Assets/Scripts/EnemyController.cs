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
    public float bullet_offset = -0.5f;
    public int attack_chance = 1;
    public float attack_cooldown = 2f;
    public LayerMask mask;

    public GameObject bullet;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CheckAttackAvailability();
    }
    void CheckAttackAvailability()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,50,mask);

        if (!hit)
        {
            Debug.Log("Should start");
            StartCoroutine(AttackCooldown());
        }
 

    }
    void Movement()
    {
        rb.velocity = new Vector2(direction * horizontal_movespeed*Time.deltaTime, 0);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }
    private void Update()
    {
       
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
        GameController.EnemyDied();
        Destroy(gameObject);
    }

    public void OtherEnemyDeath()
    {
        if(tag == "Enemy")
        {
            CheckAttackAvailability();
        }
    }
    private void OnEnable()
    {
        GameController.OnSideCollision += ChangeDirection;
        GameController.OnEnemyDeath += OtherEnemyDeath;
    }
    private void OnDisable()
    {
        GameController.OnSideCollision -= ChangeDirection;
        GameController.OnEnemyDeath -= OtherEnemyDeath;

    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(Random.Range(attack_cooldown/2,attack_cooldown*2));
        AttemptAttack();
    }
}
