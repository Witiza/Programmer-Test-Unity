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

    
    void Awake()
    {
        input = new PlayerInput();
        input.Player.Move.performed += ctx => input_direction = ctx.ReadValue<float>();
        input.Player.Move.canceled += ctx => input_direction = 0f;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Movement()
    {
        rb.velocity = new Vector2(input_direction * player_speed, 0);
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void OnEnable()
    {
        input.Player.Enable();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

}
