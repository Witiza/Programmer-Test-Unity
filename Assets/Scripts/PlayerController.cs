using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    AudioSource sfx;

    PlayerInput input;

    public float player_speed;
    float input_direction;

    Animator animator;
    public GameObject bullet;
    public GameObject explosion_particles;
    public float bullet_cd;
    public float bullet_offset;
    bool bullet_available;
    bool paused = false;

    public int player_lives = 3;
    void Awake()
    {
        input = new PlayerInput();
        input.Player.Move.performed += ctx => input_direction = ctx.ReadValue<float>();
        input.Player.Move.canceled += ctx => input_direction = 0f;
        input.Player.Shoot.started += ctx => Shoot();
    }
    // Start is called before the first frame update
    void Start()
    {
        bullet_available = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sfx = GetComponent<AudioSource>();
    }

    void Movement()
    {
        if (!paused)
        {
            float speed = input_direction * player_speed * Time.deltaTime;
            rb.velocity = new Vector2(speed, 0);
            animator.SetFloat("Movement", speed);
            if (speed == 0)
                animator.SetTrigger("Stopped");
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    void Shoot()
    {
        if(bullet_available && !paused)
        {
            bullet_available = false;
            Vector2 bullet_position = rb.position;
            bullet_position.y += bullet_offset;
            Instantiate(bullet,bullet_position,Quaternion.identity);
            sfx.Play();
            StartCoroutine(BulletCooldown());
        }
    }
    void FixedUpdate()
    {
        Movement();
    }

    private void PlayerHit()
    {
        animator.SetTrigger("Hit");
        player_lives--;
        Instantiate(explosion_particles, rb.position, Quaternion.identity);
        if(player_lives >= 0)
        {
            for(int i = 0;i<3;++i)
            {
                Instantiate(explosion_particles, rb.position, Quaternion.identity);
            }
            GameObject.FindGameObjectWithTag("PlayerHP").GetComponent<Text>().text = "Lives: "+player_lives;
            StartCoroutine(Pause(0.5f));
        }
    }
    private void GameFinished()
    {
        StartCoroutine(Pause(5f));
    }

    private void OnEnable()
    {
        input.Player.Enable();
        GameController.OnPlayerHit += PlayerHit;
        GameController.OnGameFinish += GameFinished;
    }

    private void OnDisable()
    {
        input.Player.Disable();
        GameController.OnPlayerHit -= PlayerHit;
        GameController.OnGameFinish -= GameFinished;
    }

    IEnumerator BulletCooldown()
    {
        yield return new WaitForSeconds(bullet_cd);
        bullet_available = true;
    }

    IEnumerator Pause(float seconds)
    {
        paused = true;
        yield return new WaitForSeconds(seconds);
        paused = false;
    }
}
