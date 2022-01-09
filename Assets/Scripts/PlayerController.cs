using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerInput input;
    public float player_speed;
    float input_direction;

    public GameObject bullet;
    public float bullet_cd;
    public float bullet_offset;
    bool bullet_available;
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
    }

    void Movement()
    {
        rb.velocity = new Vector2(input_direction * player_speed*Time.deltaTime, 0);
    }
    void Shoot()
    {
        if(bullet_available)
        {
            bullet_available = false;
            Vector2 bullet_position = rb.position;
            bullet_position.y += bullet_offset;
            GameObject current_bullet = Instantiate(bullet,bullet_position,Quaternion.identity);
         
            StartCoroutine(BulletCooldown());
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    IEnumerator BulletCooldown()
    {
        yield return new WaitForSeconds(bullet_cd);
        bullet_available = true;
    }
}
