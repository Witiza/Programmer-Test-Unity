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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  
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
}
